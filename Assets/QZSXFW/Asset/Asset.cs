using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    public static class Asset
    {
        public static GameObject GetPrefab(string path)
        {
            GameObject go = null;
            Singleton<AssetManager>.Instance.LoadAssetSync<GameObject>(path, 
                (o) => {
                    if (o != null)
                    {
                        go = GameObject.Instantiate(o) as GameObject;
                        return go;
                    }
                    else
                    {
                        Debug.LogError($"资源加载失败:{path}");
                        return null;    
                    }
                }
            );
            return go;
        }
    }
}
