using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 负责资源管理主逻辑
    /// </summary>
    public class AssetManager : MonoBehaviour
    {
        public delegate void LoadAssetCompleteHandler(AssetInfo info);

        /// <summary>
        /// 显示Log
        /// </summary>
        public bool showAssetLog = false;

        /// <summary>
        /// 同时最大的加载数
        /// </summary>
        private const int MAX_REQUEST = 10;
        private int _requestRemain = MAX_REQUEST;

        /// <summary>
        /// 当前申请要加载的队列
        /// </summary>
        private Queue<AssetLoader> _requestQueue = new Queue<AssetLoader>();
        /// <summary>
        /// 加载队列
        /// </summary>
        private HashSet<AssetLoader> _currentLoadQueue = new HashSet<AssetLoader>();
        /// <summary>
        /// 未完成的
        /// </summary>
        private HashSet<AssetLoader> _nonCompleteLoaderSet = new HashSet<AssetLoader>();
        /// <summary>
        /// 此时加载的所有Loader记录，(用于在全加载完成之后设置 minLifeTime)
        /// </summary>
        private HashSet<AssetLoader> _thisTimeLoaderSet = new HashSet<AssetLoader>();
        /// <summary>
        /// 已加载完成的缓存列表
        /// </summary>
        private Dictionary<string, AssetInfo> _loadedAssetBundle = new Dictionary<string, AssetInfo>();

        /// <summary>
        /// 已创建的所有Loader列表(包括加载完成和未完成的)
        /// </summary>
        private Dictionary<string, AssetLoader> _loaderCache = new Dictionary<string, AssetLoader>();

        /// <summary>
        /// 当前是否还在加载，如果加载，则暂时不回收
        /// </summary>
        private bool _isCurrentLoading;

        public Dictionary<string, ConfResItem> depMapDic;

        public int clearFrame = 0;

        /// <summary>
        /// 当前是否在加载状态
        /// </summary>
        public bool isCurrentLoading { get { return _isCurrentLoading; } }


        void Awake()
        {
            // base.Awake();
            InvokeRepeating("CheckUnusedBundle", 0, 20);
            //_particlesPool = gameObject.AddComponent<ParticlesPool>();
            //_spritesPool = gameObject.AddComponent<SpritesPool>();
            Singleton<AssetManager>.Create(this);
        }

        public IEnumerator Init(Action action, bool 是否需要解压资源)
        {
            if (是否需要解压资源)
            {
#if AB_MODE
                string files = "";
                files = File.ReadAllText(AssetPathProcessor.assetInfoFilesPath);

                List<ConfResItem> confResItems = LitJson.JsonMapper.ToObject<List<ConfResItem>>(files);
                depMapDic = new Dictionary<string, ConfResItem>();

                foreach (var item in confResItems)
                {
                    foreach (var it in Assist.encryptList)
                    {
                        if (item.file == it)
                        {
                            item.mark = true;
                            break;
                        }

                    }
                    depMapDic.Add(item.file, item);
                }
                Debug.LogError(depMapDic.Count);
            }
            else
            {
                string files = "";

                if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    files = File.ReadAllText(AssetPathProcessor.assetInfoFilesPath);
                }
                else
                {
                    files = Resources.Load<TextAsset>("files").text;
                }


                List<ConfResItem> confResItems = LitJson.JsonMapper.ToObject<List<ConfResItem>>(files);
                depMapDic = new Dictionary<string, ConfResItem>();

                foreach (var item in confResItems)
                {
                    depMapDic.Add(item.file, item);
                }
#endif
            }


            yield return null;
            action();
        }


        /// <param name="path"></param>
        /// <param name="handler">回调</param>
        /// <param name="param">加载完成后的参数</param>
        /// <param name="loadMulti">编辑器模式下的多资源加载</param>
        public AssetLoader LoadAssetAsync(string path, LoadAssetCompleteHandler handler = null, object param = null, bool loadMulti = false)
        {
            // 想要支持大小写命名格式
            // string filePath = path.ToLower();
            string filePath = path;
            string rootPath = AssetPathProcessor.editorModeAssetPath;

#if UNITY_EDITOR && !AB_MODE
            filePath = AssetPathProcessor.GetAssetRelativePath(filePath) + AssetPathProcessor.GetAssetShortName(filePath);
            rootPath = rootPath + filePath;
            
            // 这里将mp3转换为wav  音频统一使用wav格式
            if (!File.Exists(rootPath))
            {
                string shortname = filePath.Substring(0, filePath.LastIndexOf("."));
                string suffix = filePath.Substring(filePath.LastIndexOf(".") + 1);
                if (suffix == "mp3")
                {
                    filePath = shortname + ".wav";
                }
            }
#else
        string suffix = filePath.Substring(filePath.LastIndexOf(".") + 1);
        filePath = suffix + "/" + filePath;
        //filePath = AssetBundlePathResolver.GetAssetPath(filePath);
#endif
            AssetLoader loader = this.CreateLoader(filePath, param, loadMulti);
            if (loader == null)
            {
                handler(null);
            }
            else
            {
                _thisTimeLoaderSet.Add(loader);
                if (loader.isComplete)
                {
                    if (handler != null)
                    {
                        if (param != null)
                        {
                            loader.bundleInfo.param = param;
                        }

                        handler(loader.bundleInfo);
                    }
                }
                else
                {
                    if (handler != null)
                        loader.onComplete += handler;
                    _isCurrentLoading = true;

                    if (loader.state < LoadState.State_Loading)
                        _nonCompleteLoaderSet.Add(loader);
                    this.StartLoad();
                }
            }
            return loader;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="handler">回调函数</param>
        /// <param name="param"></param>
        /// <param name="loadMulti"></param>
        /// <returns></returns>
        public void LoadAssetSync(string path, LoadAssetCompleteHandler handler = null, bool loadMulti = false, object param = null)
        {
            // 想要支持大小写命名格式
            // string filePath = path.ToLower();
            string filePath = path;
            string rootPath = AssetPathProcessor.editorModeAssetPath;
#if UNITY_EDITOR && !AB_MODE
            filePath = AssetPathProcessor.GetAssetRelativePath(filePath) + AssetPathProcessor.GetAssetShortName(filePath);
            rootPath = rootPath + filePath;
            if (!File.Exists(rootPath))
            {
                string shortname = filePath.Substring(0, filePath.LastIndexOf("."));
                string suffix = filePath.Substring(filePath.LastIndexOf(".") + 1);
                if (suffix == "mp3")
                {
                    filePath = shortname + ".wav";
                }
            }
#else
        string suffix = filePath.Substring(filePath.LastIndexOf(".") + 1);
        filePath = suffix + "/" + filePath;
        //filePath = AssetBundlePathResolver.GetAssetPath(filePath);
#endif
            AssetLoader loader = this.CreateLoader(filePath, param, loadMulti, false);
            if (loader == null)
            {
                //D.LogError("no !!!!!!" +path);
                handler(null);
                return;
            }
            if (loader.isComplete && handler != null)
            {
                if (handler != null)
                {
                    handler(loader.bundleInfo);
                    return;
                }
            }

            if (handler != null)
            {
                loader.onComplete += handler;
            }
            _isCurrentLoading = true;

            _currentLoadQueue.Add(loader);
            loader.LoadSync();
        }


        public void LoadAsset(string path, bool isAsync, LoadAssetCompleteHandler handler, object param = null)
        {
            if (isAsync)
            {
                LoadAssetAsync(path, handler, param, false);
            }
            else
            {
                LoadAssetSync(path, handler, false, param);
            }
        }

        //         public void ChangeScene(string file, Handler progress, Handler complete)
        //         {
        //             StartCoroutine(CoChangeScene(file, progress, complete));
        //         }

        //         public IEnumerator CoChangeScene(string file, Handler progress, Handler complete)
        //         {
        // #if AB_MODE
        //         LoadAssetSync(file);
        // #endif
        //             string name = Path.GetFileNameWithoutExtension(file);
        //             AsyncOperation async = SceneManager.LoadSceneAsync(name);

        //             async.allowSceneActivation = false;
        //             while (async.progress < 0.9f)
        //             {
        //                 if (progress != null)
        //                     progress(0.5f + async.progress * 0.5f);

        //                 yield return new WaitForEndOfFrame();
        //             }

        //             if (progress != null)
        //                 progress(1.0f);

        //             if (complete != null)
        //                 complete(async);

        //             //UnloadUnusedBundle(true);
        //         }


        void StartLoad()
        {
            if (_nonCompleteLoaderSet.Count > 0)
            {
                List<AssetLoader> loaders = ListPool<AssetLoader>.Get();
                loaders.AddRange(_nonCompleteLoaderSet);
                _nonCompleteLoaderSet.Clear();

                var e = loaders.GetEnumerator();
                while (e.MoveNext())
                {
                    _currentLoadQueue.Add(e.Current);
                }

                e = loaders.GetEnumerator();
                while (e.MoveNext())
                {
                    e.Current.LoadAsync();
                }
                ListPool<AssetLoader>.Release(loaders);
            }
        }

        public AssetLoader CreateLoader(string abFileName, object param, bool loadMulti = false, bool isAsync_ = true)
        {
            AssetLoader loader = null;
            // 想要支持大小写命名格式
            // abFileName = abFileName.ToLower();
            // abFileName = abFileName;

            if (_loaderCache.ContainsKey(abFileName))
            {
                loader = _loaderCache[abFileName];
                loader.isAsync = isAsync_;
                if (loader.bundleInfo != null)
                    loader.bundleInfo.param = param;
            }
            else
            {
#if UNITY_EDITOR && !AB_MODE
                loader = this.CreateLoader(loadMulti);
                loader.bundleManager = this;
                loader.bundleName = abFileName;
                loader.param = param;
                loader.isAsync = isAsync_;
#else
            if (depMapDic != null && depMapDic.ContainsKey(abFileName))
            {
                loader = this.CreateLoader(loadMulti);
                loader.bundleManager = this;
                loader.bundleData = depMapDic[abFileName];
                loader.bundleName = abFileName;
                loader.param = param;
                loader.isAsync = isAsync_;
            }
            else
            {
                //D.LogError("dep Map Dic null !!");
            }
#endif
                if (loader != null)
                {
                    _loaderCache[abFileName] = loader;
                }
                else
                {
                    //D.LogError("_loader null!!");
                }
            }

            return loader;
        }


        //EDITOR模式，all_加载多个
        protected AssetLoader CreateLoader(bool all_ = false)
        {
#if UNITY_EDITOR && !AB_MODE
            return new EditorModeAssetBundleLoader(all_);
#else
        return new MobileAssetBundleLoader();
#endif
        }

        public AssetInfo CreateBundleInfo(AssetLoader loader, AssetInfo abi = null, AssetBundle assetBundle = null, byte[] bytes = null, string text = null)
        {

            if (abi == null)
                abi = new AssetInfo();
            // 想要支持大小写命名格式
            // abi.bundleName = loader.bundleName.ToLower();
            abi.bundleName = loader.bundleName;
            abi.bundle = assetBundle;
            abi.data = loader.bundleData;
            abi.bytes = bytes;
            abi.text = text;

            _loadedAssetBundle[abi.bundleName] = abi;
            return abi;
        }

        public void RemoveAll()
        {
            this.StopAllCoroutines();

            _currentLoadQueue.Clear();
            _requestQueue.Clear();
            foreach (AssetInfo abi in _loadedAssetBundle.Values)
            {
                abi.Dispose();
            }
            _loadedAssetBundle.Clear();
            _loaderCache.Clear();
        }


        /// <summary>
        /// useVagueFind 模糊查找
        /// </summary>
        public AssetInfo GetBundleInfo(string key, bool useVagueFind = false)
        {
            // 想要支持大小写命名格式
            // string filePath = key.ToLower();
            string filePath = key;

#if UNITY_EDITOR && !AB_MODE
            filePath = AssetPathProcessor.GetAssetRelativePath(filePath) + AssetPathProcessor.GetAssetShortName(filePath);
#else
        filePath = AssetPathProcessor.GetAssetPath(filePath);
#endif
            var e = _loadedAssetBundle.GetEnumerator();
            while (e.MoveNext())
            {
                AssetInfo abi = e.Current.Value;

                if (abi.bundleName == filePath || (useVagueFind && abi.bundleName.Contains(filePath)))
                    return abi;
            }
            return null;
        }

        /// 请求加载Bundle，限制同时加载最大数
        internal void RequestLoadBundle(AssetLoader loader)
        {
            if (loader.isAsync)
            {
                if (_requestRemain < 0) _requestRemain = 0;

                if (_requestRemain == 0)
                {
                    _requestQueue.Enqueue(loader);
                }
                else
                {
                    this.LoadBundle(loader);
                }
            }
            else
            {
                this.LoadBundle(loader);
            }
        }


        void LoadBundle(AssetLoader loader)
        {
            if (!loader.isComplete)
            {
                if (loader.isAsync)
                    loader.LoadBundleAsync();
                else
                    loader.LoadBundleSync();
            }

            if (loader.isAsync)
                _requestRemain--;
        }

        void CheckRequestList()
        {
            while (_requestRemain > 0 && _requestQueue.Count > 0)
            {
                AssetLoader loader = _requestQueue.Dequeue();
                this.LoadBundle(loader);
            }
        }


        public void LoadError(AssetLoader loader)
        {
            this.LoadComplete(loader);
        }

        public void LoadComplete(AssetLoader loader)
        {
            _requestRemain++;
            _currentLoadQueue.Remove(loader);

            //Debug.LogError("end load name = " + loader.bundleName);
            if (_currentLoadQueue.Count == 0 && _nonCompleteLoaderSet.Count == 0)
            {
                //Debug.LogError("_isCurrentLoading = false end load name = " + loader.bundleName);
                _isCurrentLoading = false;

                var e = _thisTimeLoaderSet.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetLoader cur = e.Current;
                    if (cur.bundleInfo != null)
                        cur.bundleInfo.ResetLifeTime();
                }
                _thisTimeLoaderSet.Clear();
            }
            else
            {
                this.CheckRequestList();
            }
        }

        public void RemoveBundleInfo(AssetInfo abi)
        {
            //Debug.LogError("RemoveBundleInfo name = " + abi.bundleName);
            abi.Dispose();
            _loadedAssetBundle.Remove(abi.bundleName);
            _loaderCache.Remove(abi.bundleName);
        }

        public void CheckUnusedBundle()
        {
            this.UnloadUnusedBundle(65);
        }

        /// <summary>
        /// 卸载不用的
        /// </summary>
        public void UnloadUnusedBundle(int count, bool force = false)
        {
            if (_isCurrentLoading == false || force)
            {
                List<string> keys = ListPool<string>.Get();
                keys.AddRange(_loadedAssetBundle.Keys);

                bool hasUnusedBundle = false;
                //一次最多卸载的个数，防止卸载过多太卡
                int unloadLimit = count;
                int unloadCount = 0;

                do
                {
                    hasUnusedBundle = false;
                    for (int i = 0; i < keys.Count && !_isCurrentLoading && unloadCount < unloadLimit; i++)
                    {
                        if (_isCurrentLoading && !force)
                            break;

                        string key = keys[i];
                        AssetInfo abi = _loadedAssetBundle[key];

                        if (abi.isUnused)
                        {
                            hasUnusedBundle = true;
                            unloadCount++;
                            //Debug.Log("delete bundlename = " + abi.bundleName);
                            this.RemoveBundleInfo(abi);

                            keys.RemoveAt(i);
                            i--;
                        }
                    }
                } while (hasUnusedBundle && !_isCurrentLoading && unloadCount < unloadLimit);

                ListPool<string>.Release(keys);
            }
        }

        //模糊删掉
        public void RemoveBundle(string key, bool useVagueFind = false)
        {
            //Debug.LogError("RemoveBundle key = " + key);

            AssetInfo abi = this.GetBundleInfo(key, useVagueFind);
            if (abi != null)
            {
                this.RemoveBundleInfo(abi);
            }
        }
    }
}
