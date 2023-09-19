using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetManager
{
    private AssetBundle assetBundle;
    public AssetManager()
    {
        AssetBundle existingBundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault(bundle => bundle.name == "test");
        if (existingBundle == null)
        {
            // 只有当 AssetBundle 'test' 未加载时才加载它
            assetBundle = AssetBundle.LoadFromFile("path/to/test");
        }
        else
        {
            // 已经加载了 AssetBundle 'test'，您可以使用 existingBundle 来访问它
            assetBundle = existingBundle;
        }
        // assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/test");
    }

    public static GameObject LoadGameObject(string fileName)
    {
        string path = "prefab";
        // GameObject go = AssetManager.LoadGameObject(path + "/" + fileName);
        GameObject go = Singleton<AssetManager>.Instance.assetBundle.LoadAsset<GameObject>(fileName);
        if (go)
        {
            return GameObject.Instantiate(go);
        }
        else
            return null;
    }
    public static GameObject LoadUIPrefab(string fileName)
    {
        string path = "ui_prefab";
        // GameObject go = AssetManager.LoadGameObject(path + "/" + fileName);
        GameObject go = Singleton<AssetManager>.Instance.assetBundle.LoadAsset<GameObject>(fileName);
        if (go)
        {
            return GameObject.Instantiate(go);
        }
        else
            return null;
    }
    public static GameObject LoadCamera(string fileName)
    {
        string path = "camera";
        // GameObject go = AssetManager.LoadGameObject(path + "/" + fileName);
        GameObject go = Singleton<AssetManager>.Instance.assetBundle.LoadAsset<GameObject>(fileName);
        if (go)
        {
            return GameObject.Instantiate(go);
        }
        else
            return null;
    }
    public static Sprite LoadItemSprite(string fileName)
    {
        string path = "texture";
        // Sprite sp = Resources.Load<Sprite>(path + "/item/" + fileName + ".png");
        Sprite sp = Singleton<AssetManager>.Instance.assetBundle.LoadAsset<Sprite>(fileName);
        if (sp)
        {
            return sp;
        }
        else
            return null;
    }
}
