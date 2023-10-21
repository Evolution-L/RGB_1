using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using PlasticGui.WorkspaceWindow.QueryViews.Changesets;
using Unity.VisualScripting;
using dnlib.DotNet.Emit;
/// <summary>
/// 该工具还有很多问题,  当前已知:
/// 1. 不够灵活
/// 2. 没有解决冗余问题
/// </summary>
public class GenAB : EditorWindow
{
    private static GenAB genABWindow;
    private int version = 0;
    private string assetPath = "";

    private FolderProcessor folderProcessor = new FolderProcessor();

    private static BuildTarget BuildTarget_;
    private static string Variant_ = "";

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
        // var manifest = BuildPipeline.BuildAssetBundles(assetPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget_);
    }

    private void SetABName()
    {
        string rootPath = @"Assets/Resource/";
        string[] files = Directory.GetDirectories(rootPath);
        string[] files2 = Directory.GetFiles(rootPath);
        // 设置AB包名
        for (int i = 0; i < files.Length; i++)
        {
            if (folderProcessor.SetPath(files[i]))
            {
                folderProcessor.SetABName();
            }
            else
            {
                Debug.Log($"异常目录: {files[i]}");
            }
        }

        // List<string> allFiles = GetAllFiles(rootPath);
        // foreach (var item in allFiles)
        // {
        //     AssetImporter assetImporter = AssetImporter.GetAtPath(item);
        //     assetImporter.assetBundleName = "test";
        //     assetImporter.assetBundleVariant = null;
        // }
    }

    private List<string> GetAllFiles(string path)
    {
        List<string> allFiles = new();
        string[] files1 = Directory.GetFiles(path);
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


abstract class IGenABNameStrategy
{
    public abstract void SetABName(string filePath);

    protected string ClipPath(string path)
    {
        var bb = path.Replace("Assets/Resource", "");
        return $"data{bb}";
    }
    protected void SetABName(string path, string abName, string variant = "one")
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        assetImporter.assetBundleName = abName;
        assetImporter.assetBundleVariant = variant;
    }
}

class FolderProcessor
{
    string optionPath;
    IGenABNameStrategy strategy;
    public bool SetPath(string path)
    {
        optionPath = path.Replace("\\", "/");
        var folderName = optionPath.Substring(optionPath.LastIndexOf("/") + 1);
        // SetDirectoryName(Variant_, optionPath);
        switch (folderName)
        {
            case "prefab":
                strategy = new GenPrefabABNameStrategy();
                return true;
            case "texture":
                strategy = new GenTextureABNameStrategy();
                return true;
            case "anim_asset":
                strategy = new GenAnimAssetABNameStrategy();
                return true;
            default:
                Debug.Log("!!!! 为注册的文件夹 !!!!");
                return false;
        }


    }

    public void SetABName()
    {
        strategy.SetABName(optionPath);
    }
}

class GenPrefabABNameStrategy : IGenABNameStrategy
{
    public override void SetABName(string filePath)
    {

        string rootPath = filePath;
        // string[] fileEntries = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray();
        string[] folders = Directory.GetDirectories(filePath);
        string[] strings = new string[2];

        foreach (var item in folders)
        {
            var name = Path.GetFileName(item);
            var type = name.Substring(name.IndexOf(".") + 1);
            switch (type)
            {
                case "packet":
                    // 包
                    foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    {
                        string abName = Path.GetDirectoryName(file).Replace("\\", "/");
                        abName = ClipPath(abName);
                        SetABName(file, abName);
                    }
                    break;
                default:
                    // file 为单个文件
                    foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    {
                        string abName = file.Replace("\\", "/");
                        abName = ClipPath(abName).Replace(abName[abName.LastIndexOf('.') ..], "");
                        SetABName(file, abName);
                    }
                    break;
            }
        }
    }
}

class GenTextureABNameStrategy : IGenABNameStrategy
{
    string uiPath = @"Assets/Resource/prefab/ui/";
    public override void SetABName(string filePath)
    {
        string rootPath = filePath;
        // string[] fileEntries = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray();
        string[] folders = Directory.GetDirectories(filePath);
        string[] strings = new string[2];

        string[] allUI = Directory.GetFiles(uiPath, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray();
        Dictionary<string, string> allUIDic = new Dictionary<string, string>();
        for (int i = 0; i < allUI.Length; i++)
        {
            var name = Path.GetFileNameWithoutExtension(allUI[i]).ToLower();
            var item = allUI[i][..allUI[i].LastIndexOf('.')];
            allUIDic.Add(name, ClipPath(item));
        }

        foreach (var item in folders)
        {
            var name = Path.GetFileName(item);
            var type = name[(name.IndexOf(".") + 1)..];
            switch (type)
            {
                case "monofile":
                    // file 为单个文件
                    foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    {
                        string abName = file.Replace("\\", "/");
                        abName = ClipPath(abName);
                        abName.Replace(abName[abName.LastIndexOf('.') ..], "");
                        SetABName(file, abName);
                    }
                    break;
                case "packet":
                    // 包
                    foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    {
                        string abName = Path.GetDirectoryName(file).Replace("\\", "/"); ;
                        abName = ClipPath(abName);
                        SetABName(file, abName);
                    }
                    break;
                case "ui":
                    // 与 ui 预制体打到一起
                    string folderName = Path.GetFileNameWithoutExtension(item);
                    if (allUIDic.ContainsKey(folderName))
                    {
                        string abName = allUIDic[folderName];
                        foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                        {
                            SetABName(file, abName);
                        }
                    }
                    else
                    {
                        Debug.LogError($"texture下文件夹{name}命名错误, 未检查到对应的ui预制体, 检查命名或将'.ui'后缀更改为'.packet' or '.monofile'\n {item}");
                    }
                    break;
                default:
                    Debug.LogError($"texture下文件夹{name}命名错误, 未指明标记, 给文件夹加上对应后缀 '.ui' or '.packet' or '.monofile'\n '.ui' : 与对应UI预制体打到一起. \n '.packet' : 文件夹为单位的包 \n '.monofile' : 文件夹下 文件为单位的包 \n{item}");
                    break;
            }
        }
    }

    private string GetDirMark(string filePath)
    {
        string dirName = Path.GetDirectoryName(filePath);
        return dirName.Substring(dirName.IndexOf('.') + 1);
    }
}
/// <summary>
/// 此文件夹下资源禁止代码加载, 默认打到对应prefab中, 若被多引用则单独打成AB包
/// 此文件夹下的资源若单独成包必须 打成以文件夹为单位的包
/// 被多引用的资源文件夹需要标记 '.packet'
/// </summary>
class GenAnimAssetABNameStrategy : IGenABNameStrategy
{
    public override void SetABName(string filePath)
    {

        string rootPath = filePath;
        // string[] fileEntries = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray();
        string[] folders = Directory.GetDirectories(filePath);
        string[] strings = new string[2];

        foreach (var item in folders)
        {
            var name = Path.GetFileName(item);
            var type = name.Substring(name.IndexOf(".") + 1);
            switch (type)
            {
                case "packet":
                    // 包
                    foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    {
                        string abName = Path.GetDirectoryName(file).Replace("\\", "/");
                        abName = ClipPath(abName);
                        SetABName(file, abName);
                    }
                    break;
                default:
                    // // file 为单个文件
                    // foreach (var file in Directory.GetFiles(item, "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith(".meta")).ToArray())
                    // {
                    //     string abName = file.Replace("\\", "/");
                    //     abName = ClipPath(abName).Replace(abName[abName.LastIndexOf('.') ..], "");
                    //     SetABName(file, abName);
                    // }
                    break;
            }
        }
    }
}