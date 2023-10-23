using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public static class AssetConfig
    {
        public static Dictionary<string, string> folderABType = new Dictionary<string, string>
        {
            {@"/anim_asset/growable", "monofile"},
            {@"/anim_asset/role", "monofile"},

            {@"/prefab/camera", "packet"},
            {@"/prefab/growable", "monofile"},
            {@"/prefab/role", "monofile"},
            {@"/prefab/ui", "monofile"},

            {@"/texture/common", "monofile"},
            {@"/texture/commonpacket", "packet"},
            {@"/texture/item_icon", "packet"},
            {@"/texture/main", "ui"},
        };

    }




