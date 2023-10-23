
using UnityEditor;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    public abstract class AssetLoader
    {
        public string Mark => assetInfo.assetPath;
        public AssetInfo assetInfo;
        internal AssetManager.LoadAssetCompleteHandler onComplete;
        public AssetManager assetManager;

        public bool isComplete = false;
        

        public AssetLoader(string assetPath)
        {
            assetInfo = new AssetInfo();
            assetInfo.assetPath = assetPath;
        }

        public abstract void SynLoader();
        public abstract void AsynLoader();
        public abstract T GetAsset<T>(string assetName) where T : Object;
    }

    public class EditorAssetLoader : AssetLoader
    {   
        string path = @"Assets/Resource/";
        public EditorAssetLoader(string assetPath) : base(assetPath)
        {
        }

        public override void AsynLoader()
        {
            throw new System.NotImplementedException();
        }

        public override void SynLoader()
        {
            if (assetInfo.asset == null)
            {
                assetInfo.asset = AssetDatabase.LoadMainAssetAtPath($"{path}{assetInfo.assetPath}");
            }
            isComplete = true;
            if (onComplete != null)
            {
                onComplete(assetInfo);
            }
        }

        public override T GetAsset<T>(string assetName)
        {
            throw new System.NotImplementedException();
        }
    }
}
