using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

public class GenAB : EditorWindow
{
    private static GenAB genABWindow;
    private int version = 0;
    private string assetPath = "";

    private static BuildTarget BuildTarget_;

    [MenuItem("AssetBundle相关/GenAB")]
    private static void Init()
    {
        if (!genABWindow)
        {
            genABWindow = GetWindow<GenAB>();
            genABWindow.Show();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("GenAB", EditorStyles.boldLabel);
        version = EditorPrefs.GetInt("version", 1);
        version = EditorGUILayout.IntField("version", version);
        assetPath = EditorPrefs.GetString("assetPath", Application.dataPath.Replace("/Assets", "/output/"));
        assetPath = EditorGUILayout.TextField("assetPath", assetPath);

        if (GUILayout.Button("<1>生成所有资源"))
        {
            if (genABWindow == null)
            {
                Debug.LogError("请重新打开该窗口");
                EditorUtility.DisplayDialog("提示", "请重新打开该窗口!", "ok");
                return;
            }

            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }

            BuildAssets();
        }
    }

    private void BuildAssets()
    {
#if UNITY_ANDROID
        BuildTarget_ = BuildTarget.Android;
        Debug.LogError("target android");
#elif UNITY_IPHONE
        BuildTarget_ = BuildTarget.iOS;
        Debug.LogError("target ios");
#elif UNITY_STANDALONE_OSX
        BuildTarget_ = BuildTarget.StandaloneOSX;
        Debug.LogError("target StandaloneOSX");
#elif UNITY_STANDALONE_WIN
        BuildTarget_ = BuildTarget.StandaloneWindows64;
        Debug.LogError("target windows");
#endif
        SetABName();
    }

    private void SetABName()
    {
        string rootPath = @"Assets/ResourcesHot/";
        string[] files = Directory.GetDirectories(rootPath);
        string[] files2 = Directory.GetFiles(rootPath);
 
        //  foreach (var item in files2)
        //  {
        //     if (!item.Contains(".meta"))
        //     {
        //         Debug.Log(item);
        //     }
        //  }

        List<string> allFiles = GetAllFiles(rootPath);
        foreach (var item in allFiles)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(item);
            assetImporter.assetBundleName = "test";
            assetImporter.assetBundleVariant = null;
        }

        var manifest = BuildPipeline.BuildAssetBundles(assetPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget_);
    }

    private List<string> GetAllFiles(string path)
    {
        List<string> allFiles = new();
        string[] files1 =  Directory.GetFiles(path);
        foreach (var item in files1)
        {
            if (!item.Contains(".meta"))
            {
                allFiles.Add(item);
            }
        }

        string[] childPath = Directory.GetDirectories(path);
        foreach (var item in childPath)
        {
            var temp = GetAllFiles(item);
            allFiles.AddRange(temp);
        }

        return allFiles;
    }
}

