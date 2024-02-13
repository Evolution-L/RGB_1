using System.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    public class AssetManager
    {
        internal delegate Object LoadAssetHandler(Object asset);
        internal delegate void LoadAssetCompleteHandler(AssetLoader assetLoader);
        // 加载中
        private HashSet<AssetLoader> _loadingLoader = new HashSet<AssetLoader>();
        // 记载完成的
        private Dictionary<string, AssetLoader> _loaderCache = new Dictionary<string, AssetLoader>();
        // 所有的loader
        private Dictionary<string, AssetLoader> _allLoaderCache = new Dictionary<string, AssetLoader>();

        Dictionary<string, ABConfig> aBConfigs = new();


        public AssetManager()
        {
            var json = File.ReadAllText(Application.streamingAssetsPath + "/data/" + "files.json");
            aBConfigs = LitJson.JsonMapper.ToObject<Dictionary<string, ABConfig>>(json);
        }



        internal void LoadAssetSync<T>(string path, LoadAssetHandler handler = null) where T : Object
        {
            // string filePath = AssetPathProcessor.editorModeAssetPath + path;

            AssetLoader assetLoader = CreateLoader(path);
            if (assetLoader.isComplete)
            {
                T a = assetLoader.GetAsset<T>(path[(path.LastIndexOf('/') + 1)..]);
                Object b = handler?.Invoke(a);
                assetLoader.AddWeakReference(b);
                // handler?.Invoke(assetLoader.GetAsset<T>(path));
            }
            else
            {
                if (handler != null)
                {
                    assetLoader.onComplete += (assetLoader) =>
                    {
                        T a = assetLoader.GetAsset<T>(path[(path.LastIndexOf('/') + 1)..]);
                        Object b = handler?.Invoke(a);
                        assetLoader.AddWeakReference(b);
                    };
                    assetLoader.onComplete += OnCompleteLoad;
                }
                assetLoader.LoaderSyn();
            }
        }

        internal AssetLoader CreateLoader(string path, bool isAB = false)
        {
            AssetLoader assetLoader = null;
#if UNITY_EDITOR && !AB_MODE
            if (_loaderCache.ContainsKey(path))
            {
                assetLoader = _loaderCache[path];
                return assetLoader;
            }
            else
                assetLoader = new EditorAssetLoader(path, this);
                // return new EditorAssetLoader(path, this);
#else       
            if (!isAB)
            {
                var p = path.Split('/');
                for (int i = p.Length - 1; i >= 0; i--)
                {
                    if (p[i].Contains("monofile"))
                    {
                        path = path[..path.LastIndexOf('.')];
                        break;
                    }
                    else if (p[i].Contains("packet"))
                    {
                        path = path[..path.LastIndexOf('/')];
                        break;
                    }
                }
                path = $"{AssetPathProcessor.DirBundleName}/{path}{AssetPathProcessor.assetBundleVariant}";
            }
            path = path.ToLower();
            
            if (_allLoaderCache.ContainsKey(path))
            {
                assetLoader = _allLoaderCache[path];
                return assetLoader;
            }
            else
            {
                ABConfig ab = aBConfigs[path];
                var loder = new ABAssetLoader(path,ab ,this);
                _loadingLoader.Add(loder);
                _allLoaderCache.Add(path, loder);
                return loder;
            }
                
#endif

            return assetLoader;
        }

        internal void OnCompleteLoad(AssetLoader assetLoader)
        {
            _loadingLoader.Remove(assetLoader);
            _loaderCache.Add(assetLoader.assetInfo.assetPath, assetLoader);
        }


        public void CheckUnUseAB()
        {
            var t = _loaderCache.Keys.ToArray();
            foreach (var item in t)
            {
                if (_loaderCache[item].IsUnused)
                {
                    _loaderCache[item].UnLoadAsset();
                }
            }
        }
    }
}
