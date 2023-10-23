using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    public class AssetManager
    {
        internal delegate void LoadAssetCompleteHandler(AssetInfo info);

        private HashSet<AssetLoader> _loadingLoader = new HashSet<AssetLoader>();
        private Dictionary<string, AssetLoader> _loaderCache = new Dictionary<string, AssetLoader>();



        internal void LoadAssetSync(string path, LoadAssetCompleteHandler handler = null)
        {
            // string filePath = AssetPathProcessor.editorModeAssetPath + path;

            AssetLoader assetLoader = CreateLoader(path);
            if (assetLoader.isComplete)
            {
                if (handler != null)
                {
                    handler(assetLoader.assetInfo);
                }
            }
            else
            {
                if (handler != null)
                {
                    assetLoader.onComplete += handler;
                }
                assetLoader.SynLoader();
            }
        }

        private AssetLoader CreateLoader(string path)
        {
            AssetLoader assetLoader = null;
            if (_loaderCache.ContainsKey(path))
            {
                assetLoader = _loaderCache[path];
            }
            else
            {
#if UNITY_EDITOR && !AB_MODE
                return new EditorAssetLoader(path);
#else
        // return new MobileAssetBundleLoader();
#endif
            }

            return assetLoader;
        }
        internal void OnCompleteLoad(AssetLoader assetLoader)
        {
            _loadingLoader.Remove(assetLoader);
        }
    }
}
