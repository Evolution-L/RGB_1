using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QZSXFrameWork.Asset
{
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
#if UNITY_EDITOR && !AB_MODE
                // 仅适用于编辑器环境
                assetInfo.asset = AssetDatabase.LoadMainAssetAtPath($"{path}{assetInfo.assetPath}");
#endif
            }
            state = ALState.Complete;
            isComplete = true;
            onComplete?.Invoke(this);
        }
        public override T GetAsset<T>(string assetName)
        {
            return (T)assetInfo.asset;
        }

        public override void UnLoadAsset()
        {
            
        }
    }
}
