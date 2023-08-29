using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class PlayerDataProxy : DataProxy
{
    private int curHp;
    private int curMp;
    private int maxHp;
    private int maxMp;

    public int CurHp { get => curHp; }
    public int CurMp { get => curMp; }
    public int MaxHp { get => maxHp; }
    public int MaxMp { get => maxMp; }

    
    public PlayerDataProxy()
    {
        curHp = 0;
        curMp = 0;
        maxHp = 100;
        maxMp = 100;
    }

    public void Init(string json) 
    {
        JsonData jsonData = JsonMapper.ToObject(json);
        curHp = (int)jsonData["curHp"];
        curMp = (int)jsonData["curMp"];
        maxHp = (int)jsonData["maxHp"];
        maxMp = (int)jsonData["maxMp"];
    }

    public string GetDataJson()
    {
        Dictionary<string, int> keyValuePairs = new()
        {
            { "curHp", curHp },
            { "curMp", curMp },
            { "maxHp", maxHp },
            { "maxMp", maxMp },
        };

        return JsonMapper.ToJson(keyValuePairs);
    }

}
