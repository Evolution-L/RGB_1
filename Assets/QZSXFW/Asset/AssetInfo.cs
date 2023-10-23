using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QZSXFrameWork.Asset
{
    public class AssetInfo
    {
        public string assetPath;
        public ABConfig abConfig;
        public Object asset;

        public int ReferenceCount{ get; private set ; }

        /// <summary>
        /// 引用为 0 时的生存时间, 逾期卸载, 单位秒
        /// </summary>
        public float timeToLive = 5;

        /// <summary>
        /// 加载完成的时间
        /// </summary>
        public float loadCompleteTime;

        /// <summary>
        /// 储存当前资源单元的依赖项
        /// </summary>
        private HashSet<AssetInfo> depends = new HashSet<AssetInfo>();
    }
}
