using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 记录AB包的信息
    /// </summary>
    public class ABConfig
    {
        public string file;
        public string md5;
        public int size;
        public int type;
        public bool mark;
        public List<string> depends;
        public AssetBundleType assetType;
    }
}
