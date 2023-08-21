using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class ConfigLoader
{
    static readonly string fileUrl = Application.streamingAssetsPath + @"\Config\";
    static readonly string expandedName = ".json";
    public void Loader()
    {
        //GrowableCfg.Instance.cfgs = JsonMapper.ToObject<Dictionary<string, GrowableCfgItem>>(GetDate("growable.json"));
    }

    public static Dictionary<T, U> GetDate<T, U>(string fileName)
    {
        string readData = "";

        //��ȡ�ļ�
        using (StreamReader sr = File.OpenText(fileUrl + fileName + expandedName))
        {
            //���ݱ���
            readData = sr.ReadToEnd();
            sr.Close();
        }

        return JsonMapper.ToObject<Dictionary<T, U>>(readData);
    }
}
