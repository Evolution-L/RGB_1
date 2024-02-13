using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    public static class Asset
    {
    #if UNITY_EDITOR && !AB_MODE
        // static string path = Application.dataPath + "/Resource/";
        static string path = "Assets/Resource/";
    #else
        static string path = Application.persistentDataPath + "/HotUpdate/";
    #endif

        public static GameObject GetPrefab(string targetPath)
        {
            int index = targetPath.LastIndexOf('.');
            string dic_name = targetPath.Substring(index + 1) + "/";
    #if UNITY_EDITOR && !AB_MODE
            string tp = path + dic_name + targetPath;
            return AssetDatabase.LoadMainAssetAtPath(tp) as GameObject;
    #else
            string tp = path + targetPath;
            // return AssetDatabase.LoadAssetAtPath<GameObject>(tp);
    #endif
        }
    }
}
