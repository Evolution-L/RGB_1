using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    public static GameObject LoadGameObject(string fileName)
    {
        string path = "prefab";
        GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
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
        GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
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
        GameObject go = Resources.Load<GameObject>(path + "/" + fileName);
        if (go)
        {
            return GameObject.Instantiate(go);
        }
        else
            return null;
    }
}
