using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrowableFactory
{
    public IGrowable Create(string id);
}

public class GrowableFactory : IGrowableFactory
{
    public IGrowable Create(string id)
    {
        if (GrowableCfg.Instance.cfgs.TryGetValue(id, out GrowableCfgItem item))
        {
            GameObject go = AssetManager.LoadGameObject(item.res);
            IGrowable growable = go.AddComponent<Growable>() as IGrowable;

            return growable;
        }

        return null;
    }
}
