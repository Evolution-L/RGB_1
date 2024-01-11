using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace QZSXFrameWork.Asset
{
    internal class ABAssetLoader : AssetLoader
    {
        /// <summary>
        /// 储存当前资源单元的依赖项
        /// </summary>
        public List<AssetLoader> currentLoadingDepends = new List<AssetLoader>();
        public ABAssetLoader(string assetPath, ABConfig aBConfig, AssetManager assetManager) : base(assetManager)
        {
            assetInfo = new AssetInfo
            {
                assetPath = assetPath,
                abConfig = aBConfig
            };
        }
        public override void LoaderAsyn()
        {
            throw new System.NotImplementedException();
        }
        public override void LoaderSyn()
        {
            state = ALState.Loading;
            LoadDepend();
        }

        public void LoadABSyn()
        {
            var s = ALState.None;
            if (assetInfo.assetBundle == null)
            {
                var abCachePath = $"{AssetPathProcessor.aBPersistentPath}{assetInfo.assetPath}".Replace('/', '\\');
                var abBuildPath = $"{AssetPathProcessor.aBStreamingPath}{assetInfo.assetPath}".Replace('/', '\\');
                var loadPath = "";
                if (File.Exists(abCachePath))
                {
                    loadPath = abCachePath;
                }
                else if (File.Exists(abBuildPath))
                {
                    loadPath = abBuildPath;
                }
                else
                {
                    s = ALState.Error;
                    Debug.LogError($"找不到AB文件{abBuildPath}");
                    Debug.LogError($"找不到AB文件{abCachePath}");
                    return;
                }

                UnityWebRequest www = null;
                if (Application.platform == RuntimePlatform.Android)
                    www = UnityWebRequest.Get(loadPath);
                else
                    www = UnityWebRequest.Get("file:///" + loadPath);

                www.SendWebRequest();
                // 阻塞，实现同步效果
                while (!www.isDone)
                {
                    System.Threading.Thread.Sleep(1 / 30);
                }

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var dataStream = new MemoryStream();
                    dataStream.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
                    assetInfo.assetBundle = AssetBundle.LoadFromStream(dataStream);
                }

                www.Dispose();
                www = null;
            }

            assetInfo.loadCompleteTime = Time.time;
            state = s == ALState.None ? ALState.Complete : s;
            isComplete = true;

            onComplete?.Invoke(this);
        }
        public void LoadDepend()
        {
            var depends = assetInfo.abConfig.depends;
            for (int i = 0; i < depends.Count; i++)
            {
                var depLoader = assetManager.CreateLoader(depends[i], true);
                if (depLoader.state != ALState.Error && depLoader.state != ALState.Complete)
                {
                    depLoader.onComplete += OnDependLoadComplete;
                    currentLoadingDepends.Add(depLoader);
                    // depLoader.assetInfo.referenceCount ++;
                    depLoader.AddReferenceCount();
                }
                else if (depLoader != null)
                {
                    assetInfo.depends.Add(depLoader);
                    // depLoader.assetInfo.referenceCount ++;
                    depLoader.AddReferenceCount();
                }
                else
                {
                    Debug.LogError($"加载依赖时获取到空的loader{depends[i]}");
                }
            }

            for (int i = 0; i < currentLoadingDepends.Count; i++)
            {
                var item = currentLoadingDepends[i];

                if (item.state != ALState.Error && item.state != ALState.Complete)
                {
                    item.LoaderSyn();
                }

            }

            // 若为空则直接加载自身
            if (currentLoadingDepends.Count == 0 && state != ALState.Complete && state != ALState.Error)
            {
                LoadABSyn();
            }
        }

        public void OnDependLoadComplete(AssetLoader assetLoader)
        {
            assetInfo.depends.Add(assetLoader);
            currentLoadingDepends.Remove(assetLoader);

            if (currentLoadingDepends.Count <= 0)
            {
                if (!isAsyn)
                {
                    LoadABSyn();
                }
                else
                {
                    //这里执行异步方法
                }
            }
        }

        public override T GetAsset<T>(string assetName)
        {
            // assetName = assetName[assetName.LastIndexOf('/')..];
            return (T)assetInfo.assetBundle.LoadAsset(assetName);
        }

        public override void UnLoadAsset()
        {
            var depends = assetInfo.depends;
            for (int i = 0; i < depends.Count; i++)
            {
                depends[i].ReduceReferenceCount();
                depends.RemoveAt(i);
            }
            AssetBundle.UnloadAllAssetBundles(assetInfo.assetBundle);
        }
    }
}
