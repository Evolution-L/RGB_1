using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagDataProxy : DataProxy
{
    public List<CellInfo> cellInfos;

    private int maxIndex;

    public BagDataProxy()
    {
        cellInfos = new();
        maxIndex = 10;
        for (int i = 0; i < maxIndex; i++)
        {
            CellInfo cellInfo = new();
            cellInfo.index = i;

            cellInfos.Add(cellInfo);
        }
    }

    public string GetDataJson()
    {
        return LitJson.JsonMapper.ToJson(cellInfos);
    }

    public void Init(string json)
    {
        cellInfos.Clear();
        cellInfos = LitJson.JsonMapper.ToObject<List<CellInfo>>(json);
    }

    public void ChangeCell(CellInfo cellInfo)
    {
        foreach (var item in cellInfos)
        {
            if (cellInfo.index == item.index)
            {
                item.index = cellInfo.index;
                item.configId = cellInfo.configId;
                item.iconId = cellInfo.iconId;
                item.stackNum = cellInfo.stackNum;
                item.curlevel = cellInfo.curlevel;
            }               
        }
    }

    public CellInfo GetCellInfoByIndex(int index)
    {
        if (index <= maxIndex)
        {
            return cellInfos[index];
        }
        Debug.Log("尝试通过ID获取未拥有或不存在的物品");
        return null;
    }    
    public CellInfo GetCellInfoById(string id)
    {
        foreach (var item in cellInfos)
        {
            if (item.configId == id)
            {
                return item;
            }
        }
        Debug.Log("尝试通过ID获取未拥有或不存在的物品");
        return null;
    }
}
public class CellInfo
{
    public int index;
    public string configId;
    public int iconId;
    public int stackNum;
    public int curlevel;
}
