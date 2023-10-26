using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    internal enum ABType
    {
        None,
        Monofile,
        Packet,
    }
    internal class AssetInfo
    {
        /// <summary>
        /// editor模式下为 资源路径
        /// AB模式下为 ab包路径
        /// </summary>
        public string assetPath;
        public ABConfig abConfig;
        public AssetBundle assetBundle;
        public ABType aBType = ABType.None;
        public Object asset;
        /// <summary>
        /// 引用为 0 时的生存时间, 逾期卸载, 单位秒
        /// </summary>
        public float timeToLive = 5;
        /// <summary>
        /// 加载完成的时间
        /// </summary>
        public float loadCompleteTime;
        /// <summary>
        /// 被一个对象引用时这里储存对象的弱引用
        /// </summary>
        public List<WeakReference> weakReferenceList = new();
        /// <summary>
        /// 被其他AB依赖时, 这里加一
        /// </summary>
        public int referenceCount = 0;
        /// <summary>
        /// 储存当前资源单元的依赖项
        /// </summary>
        public List<AssetLoader> depends = new();
    }
}
