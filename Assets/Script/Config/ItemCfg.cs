
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
        
public class ItemCfg : IEnumerable<ItemCfgItem>
{
    private static ItemCfg instance;
    public static ItemCfg Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
    }
    
    public ItemCfg()
    {
        cfgs = ConfigLoader.GetDate<string, ItemCfgItem>("item");
    }
    
    //List<ItemCfgItem> cfgs;
    public Dictionary<string, ItemCfgItem> cfgs = new();
    
    public IEnumerator<ItemCfgItem> GetEnumerator()
    {
        foreach (var item in cfgs.Values)
        {
            yield return item;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public struct ItemCfgItem
{
    public readonly string id;
	public readonly int type;
	public readonly string name;
	public readonly string iconId;
	public readonly bool canStack;
	public readonly bool canDiscard;
	public readonly bool canUpLevel;
	public readonly bool canTrade;
	public readonly int tradeValue;
}