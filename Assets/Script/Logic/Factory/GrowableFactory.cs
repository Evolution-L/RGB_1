using UnityEngine;
// using Asset = QZSXFrameWork.Asset.Asset;

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
            // GameObject go = Asset.GetInsObj($"growable/{item.res}.prefab");
            // IGrowable growable = go.AddComponent<Growable>() as IGrowable;

            // return growable;
        }

        return null;
    }
}
