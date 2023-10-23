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
            Singleton<AssetManager>.Instance.LoadAssetSync(path, 
                (o) => {
                    if (o != null)
                    {
                        go = GameObject.Instantiate(o.asset) as GameObject;
                        
                    }
                }
            );

            return go;
        }
    }
}
