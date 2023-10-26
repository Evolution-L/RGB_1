
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    internal enum ALState
    {
        None,
        Loading,
        Error,
        Complete,
    }
    internal abstract class AssetLoader
    {
        public string Mark => assetInfo.assetPath;
        public AssetInfo assetInfo;
        public AssetManager.LoadAssetCompleteHandler onComplete;
        public AssetManager assetManager;
        public ALState state = ALState.None;
        public bool isComplete = false;
        public bool isAsyn = false;
        public bool IsUnused
        {
            get { return isComplete && assetInfo.referenceCount <= 0 && UpdateReference() == 0 && Time.time - assetInfo.loadCompleteTime > assetInfo.timeToLive; }
        }
        /// <summary>
        /// 检查引用情况
        /// </summary>
        /// <returns></returns>
        private int UpdateReference()
        {
            for (int i = 0; i < assetInfo.weakReferenceList.Count; i++)
            {
                Object o = (Object)assetInfo.weakReferenceList[i].Target;
                if (!o)
                {
                    assetInfo.weakReferenceList.RemoveAt(i);
                    i--;
                }
            }
            return assetInfo.weakReferenceList.Count;
        }
        public virtual void LoaderSyn()
        {
            state = ALState.Loading;
        }
        public virtual void LoaderAsyn()
        {
            state = ALState.Loading;
        }
        public abstract T GetAsset<T>(string assetName) where T : Object;
        public AssetLoader(AssetManager assetManager)
        {
            this.assetManager = assetManager;
        }
        // 引用相关方法是给AB模式使用的
        public virtual void AddWeakReference(Object owner)
        {
            if (owner == null)
            {
                return;
            }
            foreach (var item in assetInfo.weakReferenceList)
            {
                if (owner.Equals(item.Target))
                    return;
            }
            WeakReference wr = new(owner);
            assetInfo.weakReferenceList.Add(wr);
        }
        public virtual void RemoveWeakReference(Object owner)
        {
            for (int i = 0; i < assetInfo.weakReferenceList.Count; i++)
            {
                if (owner.Equals(assetInfo.weakReferenceList[i].Target))
                {
                    assetInfo.weakReferenceList.RemoveAt(i);
                    break;
                }
            }
        }
        // public virtual void CheckWeakReference()
        // {
        //     for (int i = 0; i < assetInfo.weakReferenceList.Count; i++)
        //     {
        //         if (assetInfo.weakReferenceList[i].Target == null)
        //         {
        //             assetInfo.weakReferenceList.RemoveAt(i);
        //         }
        //     }
        // }
        public abstract void UnLoadAsset();

        public virtual void AddReferenceCount()
        {
            assetInfo.referenceCount++;
        }
        public virtual void ReduceReferenceCount()
        {
            assetInfo.referenceCount--;
        }
    }
}
