using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class HotUpdate : MonoBehaviour
{
    private static bool gameInit = false;
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();
    private static List<Assembly> assemblies = new();

    private static List<string> assemblyFiles { get; } = new List<string>()
    {
        "Config.dll.bytes",
        "CustomEvent.dll.bytes",
        "Tools.dll.bytes",
        "DataManager.dll.bytes",
        "Role.dll.bytes",
        "Factory.dll.bytes",
        "CustomUI.dll.bytes",
        "Logic.dll.bytes",
    };

    void Start()
    {
        if (!gameInit)
        {
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(DownLoadAssets(this.StartGame));
        }
    }

    private void StartGame()
    {
        // 开始游戏前先加载dll  暂时没理解为AOT补充元数据和泛型相关的问题, 所以暂时忽略对其的处理
        foreach (var item in assemblyFiles)
        {
            var assPath = Path.Combine(AppConst.hotSavePath, item);
            Assembly ass = Assembly.LoadFrom(assPath);
            Debug.Log($"Load: {assPath}");
            assemblies.Add(ass);
        }

        Debug.Log("热更处理完成, 加载预制体启动游戏逻辑");
        gameInit = true;
        GameObject.Instantiate(Resources.Load("GameManager") as GameObject);


    }

    //  IEnumerator DownLoadAssets(Action onDownloadComplete)
    IEnumerator DownLoadAssets(Action onDownloadComplete)
    {
        var assets = new List<string>
        {

        }.Concat(assemblyFiles);
        // 遍历获取需要下载的资源
        foreach (var asset in assets)
        {
            string dllPath = GetWebRequestPath(asset);
            Debug.Log($"start download asset:{dllPath}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
#else
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
#endif
            else
            {
                byte[] assetData = www.downloadHandler.data;
                Debug.Log($"dll:{asset}  size:{assetData.Length}");
                s_assetDatas[asset] = assetData;

                var savePath = Path.Combine(AppConst.hotSavePath, asset);

                File.WriteAllBytes(savePath, assetData);
            }
        }

        onDownloadComplete();
    }

    private string GetWebRequestPath(string asset)
    {
        var path = $"{AppConst.resourcesPath}/{asset}";
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }
        return path;
    }
}
