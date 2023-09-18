using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    private AssetBundle assetBundle;
    public AssetManager()
    {
        assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/test");
    }

    public static GameObject LoadGameObject(string fileName)
    {
        string path = "prefab";
        // GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
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
        // GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
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
        // GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
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
