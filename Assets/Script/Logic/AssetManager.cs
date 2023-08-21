using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    public static GameObject LoadGameObject(string fileName)
    {
        string path = "prefab";
        return GameObject.Instantiate(Resources.Load<GameObject>(path + "/" + fileName));
    }    
    public static GameObject LoadUIPrefab(string fileName)
    {
        string path = "ui_prefab";
        return GameObject.Instantiate(Resources.Load<GameObject>(path + "/" + fileName));
    }
}
