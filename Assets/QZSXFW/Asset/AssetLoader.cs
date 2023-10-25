
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    internal enum ALState
    {
        None,
        Loading,
        Error,
        Complete,
    }
    internal abstract class AssetLoader
    {
        public string Mark => assetInfo.assetPath;
        public AssetInfo assetInfo;
        public AssetManager.LoadAssetCompleteHandler onComplete;
        public AssetManager assetManager;
        public ALState state = ALState.None;
        public bool isComplete = false;
        public bool isAsyn = false;
        public bool IsUnused
        {
            get { return isComplete && assetInfo.referenceCount <= 0 && UpdateReference() == 0 && Time.time - assetInfo.loadCompleteTime > assetInfo.timeToLive; }
        }
        /// <summary>
        /// 检查引用情况
        /// </summary>
        /// <returns></returns>
        private int UpdateReference()
        {
            for (int i = 0; i < assetInfo.weakReferenceList.Count; i++)
            {
                Object o = (Object)assetInfo.weakReferenceList[i].Target;
                if (!o)
                {
                    assetInfo.weakReferenceList.RemoveAt(i);
                    i--;
                }
            }
            return assetInfo.weakReferenceList.Count;
        }
        public virtual void LoaderSyn()
        {
            state = ALState.Loading;
        }
        public virtual void LoaderAsyn()
        {
            state = ALState.Loading;
        }
        public abstract T GetAsset<T>(string assetName) where T : Object;
        public AssetLoader(AssetManager assetManager)
        {
            this.assetManager = assetManager;
        }
        // 引用相关方法是给AB模式使用的
        public virtual void AddWeakReference(Object owner)
        {
            if (owner == null)
            {
                return;
            }
            foreach (var item in assetInfo.weakReferenceList)
            {
                if (owner.Equals(item.Target))
                    return;
            }
            WeakReference wr = new(owner);
            assetInfo.weakReferenceList.Add(wr);
        }
        public virtual void RemoveWeakReference(Object owner)
        {
            for (int i = 0; i < assetInfo.weakReferenceList.Count; i++)
            {
                if (owner.Equals(assetInfo.weakReferenceList[i].Target))
                {
                    assetInfo.weakReferenceList.RemoveAt(i);
                    break;
                }
            }
        }
        public void Destroy()
        {
            AssetBundle.UnloadAllAssetBundles(assetInfo.assetBundle);
        }
    }

    internal class EditorAssetLoader : AssetLoader
    {
        string path = @"Assets/Resource/";
        public EditorAssetLoader(string assetPath, AssetManager assetManager) : base(assetManager)
        {
            assetInfo = new AssetInfo
            {
                assetPath = assetPath
            };
        }
        public override void LoaderAsyn()
        {
            base.LoaderAsyn();
        }
        public override void LoaderSyn()
        {
            base.LoaderSyn();
            if (assetInfo.asset == null)
            {
                // 仅适用于编辑器环境
                assetInfo.asset = AssetDatabase.LoadMainAssetAtPath($"{path}{assetInfo.assetPath}");
            }
            state = ALState.Complete;
            isComplete = true;
            onComplete?.Invoke(this);
        }
        public override T GetAsset<T>(string assetName)
        {
            return (T)assetInfo.asset;
        }
    }
    internal class ABAssetLoader : AssetLoader
    {
        /// <summary>
        /// 储存当前资源单元的依赖项
        /// </summary>
        public HashSet<AssetLoader> currentLoadingDepends = new HashSet<AssetLoader>();
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
                    //D:\Project\Unity\RGB_1\Assets\StreamingAssets\data\prefab\camera.packet.one
                    //D:/Project/Unity/RGB_1/Assets/StreamingAssets/data/prefab/camera.packet.one

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
                    currentLoadingDepends.Add(depLoader);
                }
                else if (depLoader != null)
                {
                    assetInfo.depends.Add(depLoader);
                }
                else
                {
                    Debug.LogError($"加载依赖时获取到空的loader{depends[i]}");
                }
            }

            foreach (var item in currentLoadingDepends)
            {
                if (item.state != ALState.Error && item.state != ALState.Complete)
                {
                    item.LoaderSyn();
                }
            }

            // 若为空则直接加载自身
            if (currentLoadingDepends.Count == 0)
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
    }
}
