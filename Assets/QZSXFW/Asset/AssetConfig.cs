using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QZSXFrameWork
{
    public static class AssetConfig
    {
        

#if AB_MODE
    public static bool AB_MODE = true;
#elif UNITY_EDITOR
    public static bool AB_MODE = false;
#else
    public static bool AB_MODE = true;
#endif

    }
}
