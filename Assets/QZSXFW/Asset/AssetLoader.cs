using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 负责资源加载卸载存放资源
    /// </summary>
    public abstract class AssetLoader
    {
        internal AssetManager.LoadAssetCompleteHandler onComplete;

        public string bundleName;

        /// <summary>
        /// AB包信息
        /// </summary>
        public ConfResItem bundleData;

        public AssetInfo bundleInfo;

        public AssetManager bundleManager;

        public LoadState state = LoadState.State_None;
        /// <summary>
        /// 此加载器加载的资源单元依赖于其他资源单元, 因此必须等待其他资源单元加载完毕才能加载成功, 所以有这里储存依赖的资源单元的加载器
        /// </summary>
        public AssetLoader[] depLoaders;


        /// <summary>
        /// 参数
        /// </summary>
        public object param;

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool isAsync = true;

        /// <summary>
        /// 加载是否完成标志
        /// </summary>
        public virtual bool isComplete
        {
            get
            {
                return state == LoadState.State_Error || state == LoadState.State_Complete;
            }
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public virtual void LoadAsync() { }
        public virtual void LoadBundleAsync() { }

        /// <summary>
        /// 同步加载
        /// </summary>
        public virtual void LoadSync() { }
        public virtual void LoadBundleSync() { }

        protected virtual void Complete()
        {
            if (onComplete != null)
            {
                var handler = onComplete;
                onComplete = null;
                handler(bundleInfo);
            }
            bundleManager.LoadComplete(this);
        }

        protected virtual void Error()
        {
            if (onComplete != null)
            {
                var handler = onComplete;
                onComplete = null;
                handler(bundleInfo);
            }
            bundleManager.LoadError(this);
        }
    }

    public class MobileAssetBundleLoader : AssetLoader
    {
        /// <summary>
        /// 统计处于加载状态的依赖
        /// </summary>
        protected int _currentLoadingDepCount;
        protected AssetBundle _bundle;
        protected bool _hasError;
        protected byte[] _bytes;
        protected string _text;
        public string _assetBundleCachedFile;
        public string _assetBundleBuildInFile;

        /// <summary>
        /// 异步加载
        /// 开始加载, 同时根据状态执行逻辑
        /// </summary>
        public override void LoadAsync()
        {
            if (_hasError)
            {
                state = LoadState.State_Error;
            }
            if (state == LoadState.State_None)
            {
                state = LoadState.State_Loading;
                this.LoadDepends();
            }
            else if (state == LoadState.State_Error)
            {
                this.Error();
            }
            else if (state == LoadState.State_Complete)
            {
                this.Complete();
            }
        }

        public override void LoadSync()
        {
            if (bundleData != null)
            {
                // 遍历依赖列表 创建依赖文件的加载器
                depLoaders = new AssetLoader[bundleData.need.Count];
                for (int i = 0; i < bundleData.need.Count; i++)
                {
                    depLoaders[i] = bundleManager.CreateLoader(bundleData.need[i], null, false, false);
                }

                // 遍历列表开始 加载依赖
                _currentLoadingDepCount = 0;
                for (int i = 0; i < depLoaders.Length; i++)
                {
                    AssetLoader depLoader = depLoaders[i];
                    if (depLoader.state != LoadState.State_Error && depLoader.state != LoadState.State_Complete)
                    {
                        depLoader.LoadSync();
                    }
                }
            }

            // 设置加载状态  并检查依赖加载状态
            state = LoadState.State_Loading;
            this.CheckDepComplete();
        }

        void LoadDepends()
        {
            if (bundleData != null)
            {
                if (depLoaders == null)
                {
                    depLoaders = new AssetLoader[bundleData.need.Count];
                    for (int i = 0; i < bundleData.need.Count; i++)
                    {
                        depLoaders[i] = bundleManager.CreateLoader(bundleData.need[i], null);
                    }
                }

                // 遍历加载依赖项
                _currentLoadingDepCount = 0;
                for (int i = 0; i < depLoaders.Length; i++)
                {
                    AssetLoader depLoader = depLoaders[i];
                    if (depLoader.state != LoadState.State_Error && depLoader.state != LoadState.State_Complete)
                    {
                        _currentLoadingDepCount++;
                        depLoader.onComplete += OnDepComplete;
                        depLoader.LoadAsync();
                    }
                }
            }

            this.CheckDepComplete();
        }

        public override void LoadBundleSync()
        {
            _assetBundleCachedFile = AssetPathProcessor.persistentDataPath + AssetPathProcessor.DirBundleName + "/" + bundleName;
            _assetBundleBuildInFile = AssetPathProcessor.streamingAssetsPath + AssetPathProcessor.DirBundleName + "/" + bundleName;

            // 判断是否下载过新的AB资源, 有责加载新的资源, 没有则加载包内的资源
            string loadPath = "";
            if (File.Exists(_assetBundleCachedFile))
                loadPath = _assetBundleCachedFile;
            else
                loadPath = _assetBundleBuildInFile;

            AssetBundle req_ = null;

            if (bundleData.mark)
            {
                UnityWebRequest www = null;
                if (Application.platform == RuntimePlatform.Android)
                    www = UnityWebRequestAssetBundle.GetAssetBundle(loadPath);
                // www = UnityWebRequest.Get(loadPath);
                else
                    www = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + loadPath);

                www.SendWebRequest();

                // 阻塞，实现同步效果
                while (!www.isDone)
                {
                    System.Threading.Thread.Sleep(1 / 30);
                }

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // 处理成功响应
                    // AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(www);
                    var dataStream = new MemoryStream();
                    dataStream.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
                    var uniqueSalt = Encoding.UTF8.GetBytes(bundleName); // 打包时设置的salt名
                    // Stream暗号化解除                                                     
                    var uncryptor = new SeekableAesStream(dataStream, Assist.abEncryptString, uniqueSalt);
                    req_ = AssetBundle.LoadFromStream(uncryptor);
                }
                else
                {
                    // 处理失败或错误响应
                    string error = www.error;
                    Debug.Log(error);
                }

                www.Dispose();
                www = null;
            }
            else
            {
                // D.LogError("A加密" + loadPath);
                req_ = AssetBundle.LoadFromFile(loadPath, 0, Assist.abOffset);
            }
            _bundle = req_;
            this.Complete();
        }

        public override void LoadBundleAsync()
        {
            _assetBundleCachedFile = AssetPathProcessor.persistentDataPath + AssetPathProcessor.DirBundleName + "/" + bundleName;
            _assetBundleBuildInFile = AssetPathProcessor.streamingAssetsPath + AssetPathProcessor.DirBundleName + "/" + bundleName;

            if (File.Exists(_assetBundleCachedFile))
            {
                bundleManager.StartCoroutine(LoadFromFile());
            }
            else
            {
                bundleManager.StartCoroutine(LoadFromBuiltin());
            }
        }

        protected virtual IEnumerator LoadFromFile()
        {
            if (state != LoadState.State_Error)
            {
                AssetBundleCreateRequest req = null;

                if (bundleData.mark)
                {
                    UnityWebRequest www;
                    if (Application.platform == RuntimePlatform.Android)
                        www = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleCachedFile);
                    else
                        www = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + _assetBundleCachedFile);
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        var dataStream = new MemoryStream();
                        dataStream.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
                        var uniqueSalt = Encoding.UTF8.GetBytes(bundleName); // 打包时设置的salt名
                                                                             // Stream暗号化解除
                        var uncryptor = new SeekableAesStream(dataStream, Assist.abEncryptString, uniqueSalt);
                        req = AssetBundle.LoadFromStreamAsync(uncryptor);
                    }
                    else
                    {
                        // 处理失败或错误响应
                        string error = www.error;
                        Debug.Log(error);
                    }

                    www.Dispose();
                    www = null;
                }
                else
                {
                    // D.LogError("Async A加密" + _assetBundleCachedFile);
                    req = AssetBundle.LoadFromFileAsync(_assetBundleCachedFile, 0, Assist.abOffset);
                    yield return req;
                    if (req == null)
                    {
                        Debug.LogError("IEnumerator LoadFromFile error! _assetBundleCachedFile = " + _assetBundleCachedFile);
                    }
                }

                _bundle = req.assetBundle;

                this.Complete();
            }
        }

        protected virtual IEnumerator LoadFromBuiltin()
        {
            if (state != LoadState.State_Error)
            {
                AssetBundleCreateRequest abcr = null;

                if (bundleData.mark)
                {
                    UnityWebRequest www;
                    if (Application.platform == RuntimePlatform.Android)
                        www = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleBuildInFile);
                    else
                        www = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + _assetBundleBuildInFile);

                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        var dataStream = new MemoryStream();
                        dataStream.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
                        var uniqueSalt = Encoding.UTF8.GetBytes(bundleName); // 打包时设置的salt名
                        // Stream暗号化解除                                                     
                        var uncryptor = new SeekableAesStream(dataStream, Assist.abEncryptString, uniqueSalt);
                        abcr = AssetBundle.LoadFromStreamAsync(uncryptor);
                    }
                    else
                    {
                        // 处理失败或错误响应
                        string error = www.error;
                        Debug.Log(error);
                    }

                    www.Dispose();
                    www = null;
                }
                else
                {
                    //D.LogError("Async Buildin A加密" + _assetBundleCachedFile);
                    abcr = AssetBundle.LoadFromFileAsync(_assetBundleBuildInFile, 0, Assist.abOffset);
                }
                yield return abcr;

                if (abcr.assetBundle)
                {
                    _bundle = abcr.assetBundle;
                }
                else
                {
                    Debug.LogError("IEnumerator LoadFromBuiltin error! _assetBundleBuildInFile = " + _assetBundleBuildInFile);
                    yield break;
                }

                this.Complete();
            }
        }

        void CheckDepComplete()
        {
            if (_currentLoadingDepCount == 0)
            {
                bundleManager.RequestLoadBundle(this);
            }
        }


        void OnDepComplete(AssetInfo abi)
        {
            _currentLoadingDepCount--;
            this.CheckDepComplete();
        }

        protected override void Complete()
        {
            if (bundleInfo == null)
            {
                this.state = LoadState.State_Complete;

                this.bundleInfo = bundleManager.CreateBundleInfo(this, null, _bundle, _bytes, _text);
                this.bundleInfo.isReady = true;
                this.bundleInfo.onUnloaded = OnBundleUnload;
                this.bundleInfo.param = param;

                if (depLoaders != null)
                {
                    foreach (AssetLoader depLoader in depLoaders)
                    {
                        bundleInfo.AddDependency(depLoader.bundleInfo);
                    }
                }

                _bundle = null;
            }

            base.Complete();
        }

        private void OnBundleUnload(AssetInfo abi)
        {
            this.bundleInfo = null;
            this.state = LoadState.State_None;
        }

        protected override void Error()
        {
            _hasError = true;
            this.state = LoadState.State_Error;
            this.bundleInfo = null;
            base.Error();
        }
    }

#if UNITY_EDITOR

#if AB_MODE
    /// <summary>
    /// 编辑器模式并启用AB_MODE下用的加载器
    /// </summary>
    public class EditorModeAssetBundleLoader : MobileAssetBundleLoader
    {

    }
#else

    /// <summary>
    /// 编辑器模式下用的加载器
    /// </summary>
    public class EditorModeAssetBundleLoader : AssetLoader
    {
        bool _loadMulti = false;
        public EditorModeAssetBundleLoader(bool loadMulti_)
        {
            _loadMulti = loadMulti_;
        }

        class ABInfo : AssetInfo
        {
            public override Object mainObject
            {
                get
                {
                    if (_mainObject == null)
                    {
                        string newPath = string.Format("Assets/Resource/AssetBundles/{0}", bundleName);
                        _mainObject = AssetDatabase.LoadMainAssetAtPath(newPath);
                        if (_mainObject == null)
                        {
                            Debug.LogWarning("EditorModeAssetBundleLoader _mainObject == null, bundleName = " + bundleName);
                        }
                    }

                    return _mainObject;
                }
            }

            /// 获取集合中对象
            /// </summary>
            /// <param name="user">增加引用的对象</param>
            /// <param name="assetName">集合中单个资源名称</param>
            /// <returns></returns>
            public override Object Require(Object user, string assetName)
            {
                string suffix = Path.GetExtension(bundleName);
                string filePath = bundleName.Replace(suffix, string.Empty);
                filePath += "/" + assetName + suffix;

                return AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Resource/AssetBundles/{0}", filePath));
            }
        }

        class MultiABInfo : AssetInfo
        {
            public override Object[] mainObjects
            {
                get
                {
                    List<Object> list_ = new List<Object>();
                    string assetName_ = bundleName.Substring(0, bundleName.LastIndexOf("."));
                    string[] paths_ = System.IO.Directory.GetFiles(Application.dataPath + "/" + assetName_);
                    for (int i = 0; i < paths_.Length; ++i)
                    {
                        if (paths_[i].EndsWith(".meta") || paths_[i].EndsWith(".DS_Store"))
                            continue;

                        string filename_ = paths_[i].Replace("\\", "/");
                        string filename_2 = filename_.Substring(Application.dataPath.Length + 1);
                        Object obj_ = AssetDatabase.LoadMainAssetAtPath("Assets/" + filename_2);
                        list_.Add(obj_);
                    }

                    return list_.ToArray();
                }
            }
        }

        public override void LoadAsync()
        {
            if (bundleInfo == null)
            {
                AssetInfo abi = null;
                if (_loadMulti)
                {
                    abi = new MultiABInfo();
                }
                else
                {
                    abi = new ABInfo();
                }

                this.state = LoadState.State_Complete;
                this.bundleInfo = bundleManager.CreateBundleInfo(this, abi);
                this.bundleInfo.isReady = true;
                this.bundleInfo.param = this.param;
                this.bundleInfo.onUnloaded = OnBundleUnload;
            }

            bundleManager.StartCoroutine(this.LoadResource());
        }

        public override void LoadSync()
        {
            if (bundleInfo == null)
            {
                AssetInfo abi = null;
                // 这里负责创建一个空的AssetInfo
                if (_loadMulti)
                {
                    abi = new MultiABInfo();
                }
                else
                {
                    abi = new ABInfo();
                }

                if (bundleName.Contains("/m_13601/head_equip"))
                {
                    int a = 0;
                }

                this.state = LoadState.State_Complete;
                // 这里初始化AssetInfo
                this.bundleInfo = bundleManager.CreateBundleInfo(this, abi);
                this.bundleInfo.isReady = true;
                this.bundleInfo.param = this.param;
                this.bundleInfo.onUnloaded = OnBundleUnload;
            }

            if (onComplete != null)
            {
                var handler = onComplete;
                onComplete = null;
                handler(bundleInfo);
            }
        }

        private void OnBundleUnload(AssetInfo abi)
        {
            this.bundleInfo = null;
            this.state = LoadState.State_None;
        }

        IEnumerator LoadResource()
        {
            yield return new WaitForEndOfFrame();
            this.Complete();
        }
    }
#endif

#endif





}
