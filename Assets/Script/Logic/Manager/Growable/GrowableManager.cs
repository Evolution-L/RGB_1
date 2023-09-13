// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;



// public class GrowableManager : MonoSingleton<GrowableManager>
// {
//     Dictionary<string, IGrowable> allGlowable = new Dictionary<string, IGrowable>();
//     public void CreateGrowable(string id)
//     {
//         if (GrowableCfg.Instance.cfgs.TryGetValue(id, out GrowableCfgItem item))
//         {
//             GameObject go = AssetManager.LoadGameObject(item.res);
//             Growable growable = go.AddComponent<Growable>();
//             growable.Init(allGlowable.Count + item.id, item);
//             AddGrowable(growable);
//         }
//     }
//     public void AddGrowable(IGrowable growable)
//     {
//         if (allGlowable.ContainsKey(growable.Rid))
//         {
//             Debug.Log("���д�ֲ��");
//             return;
//         }
//         else
//             allGlowable.Add(growable.Rid, growable);
//     }
//     public void RemoveGrowable(IGrowable growable)
//     {
//         if (allGlowable.ContainsKey(growable.Rid))
//         {
//             Debug.Log("δ�д�ֲ��");
//             return;
//         }
//         else
//             allGlowable.Add(growable.Rid, growable);
//     }

//     private void Update()
//     {

//     }

//     public void UpdateGrowableState()
//     {
//         foreach (var item in allGlowable.Values)
//         {
//             item.AddAge();
//         }
//     }

// }
