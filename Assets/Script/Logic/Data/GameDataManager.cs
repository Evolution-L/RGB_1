using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GameDataManager
{
    public BagDataProxy bagData;
    public PlayerDataProxy playerData;
    public TimerDataProxy timerData;
    public string fileName = "";
    public string directoryPath = "";
    public string filePath = "";
    
    public readonly string[] saveFileNames;

    public GameDataManager()
    {
        directoryPath = Path.Combine(Application.persistentDataPath, "save");

        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath); 
        FileInfo[] files = directoryInfo.GetFiles();

        saveFileNames = new string[3];
        for (int i = 0; i< 3; i ++)
        {
            if  (i < files.Length)
            {
                saveFileNames[i] = files[i].Name; 
            }
            else
            {
                saveFileNames[i] = "";
            }
        }
    }

    public void LoadGameArchive(string saveFileName)
    {
        bagData = new();
        playerData = new();
        timerData = new();
        this.fileName = saveFileName;
        directoryPath = Path.Combine(Application.persistentDataPath, "save");
        filePath = Path.Combine(directoryPath, saveFileName + ".json");
        string readData = "";
        if (File.Exists(filePath))
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                readData = sr.ReadToEnd();
                sr.Close();
            }
            Dictionary<string, string> datas;
            datas = JsonMapper.ToObject<Dictionary<string, string>>(readData);
            if (datas.TryGetValue("BagDataProxy", out string dataJson))
            {
                bagData.Init(dataJson);
            }
            if (datas.TryGetValue("PlayerDataProxy", out string pdp))
            {
                playerData.Init(pdp);
            }            
            if (datas.TryGetValue("TimerDataProxy", out string tm))
            {
                timerData.Init(tm);
            }
        }
        else
        {
            playerData = new();
            if (!File.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(JsonMapper.ToJson(GetProxyDic()));
                sw.Flush();
                sw.Close();
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        if (!File.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        using (StreamWriter sw = File.CreateText(filePath))
        {
            sw.WriteLine(JsonMapper.ToJson(GetProxyDic()));
            sw.Flush();
            sw.Close();
        }
    }

    public Dictionary<string, string> GetProxyDic()
    {
        return new Dictionary<string, string>()
        {
            { "BagDataProxy" , bagData.GetDataJson()},
            { "PlayerDataProxy" , playerData.GetDataJson()},
            { "TimerDataProxy" , timerData.GetDataJson()},
        };
    }

}



