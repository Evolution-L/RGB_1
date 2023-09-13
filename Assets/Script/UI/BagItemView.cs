using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItemView : MonoBehaviour
{
    public GameObject iconObj;
    public GameObject block;
    public Image icon;
    public ItemCfgItem itemCfg;

    // public void Init(CellInfo cellInfo)
    // {
    //     if (cellInfo.configId != "")
    //     {
    //         block.SetActive(false);
    //         itemCfg = ItemCfg.Instance.cfgs[cellInfo.configId];
    //         icon.sprite = AssetManager.LoadItemSprite(itemCfg.iconId);
    //     }
    // }
}


