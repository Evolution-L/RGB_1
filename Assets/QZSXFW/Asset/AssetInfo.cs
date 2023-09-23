using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 资源单元
    /// </summary>
    public class AssetInfo
    {
        /// <summary>
        /// 资源销毁委托
        /// </summary>
        /// <param name="abi"></param>
        public delegate void OnUnloadedHandler(AssetInfo abi);

        /// <summary>
        /// 资源销毁回调
        /// </summary>
        public OnUnloadedHandler onUnloaded;

        /// <summary>
        /// 储存文本, 作用不明????????
        /// </summary>
        public string text;

        /// <summary>
        /// 意义不明???????
        /// </summary>
        public object param;
        /// <summary>
        /// 可能是AB包的二进制数据, 意义不明?????????
        /// </summary>
        public byte[] bytes;


        /// <summary>
        /// 当前资源单元 持有的AB包
        /// </summary>
        public AssetBundle bundle;
        /// <summary>
        /// 当前资源单元持有的AB包的信息
        /// </summary>
        public ConfResItem data;

        /// <summary>
        /// 当前资源单元持有的AB包的名称
        /// </summary>
        public string bundleName;

        /// <summary>
        /// 没有被引用的情况下, 此资源单元的最小生存时间(秒级)
        /// 否则有可能刚加载完成就被释放了
        /// </summary>
        public float minLifeTime = 5;

        /// <summary>
        /// 准备完毕的时间, 常与minLifeTime配合使用
        /// </summary>
        private float _readyTime;

        /// <summary>
        /// 标记当前资源单位是否准备完毕
        /// </summary>
        private bool _isReady;
        /// <summary>
        /// 储存加载到的资源
        /// </summary>
        protected Object _mainObject;
        /// <summary>
        /// 储存加载到的所有资源
        /// </summary>
        protected Dictionary<string, Object> mainObjectsDic = new Dictionary<string, Object>();
        /// <summary>
        /// 数组储存加载到的资源
        /// </summary>
        public virtual Object[] mainObjects { get; set; }

        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount { get; private set; }
        /// <summary>
        /// 储存当前资源单元的依赖项
        /// </summary>
        private HashSet<AssetInfo> depends = new HashSet<AssetInfo>();
        /// <summary>
        /// 当前资源单元被哪些单元依赖
        /// </summary>
        private HashSet<string> beDepends = new HashSet<string>();
        /// <summary>
        /// 当被游戏对象引用时 创建一个对该游戏对象的弱引用
        /// </summary>
        private List<WeakReference> references = new List<WeakReference>();

        public virtual Object mainObject
        {
            get
            {
                if (_mainObject == null && _isReady)
                {
                    //Debug.LogError("llll bundleName = " + bundleName);
                    string[] names = bundle.GetAllAssetNames();
                    _mainObject = bundle.LoadAsset(names[0]);
                }
                return _mainObject;
            }
        }

        public bool isReady
        {
            get { return _isReady; }
            set { _isReady = value; }
        }

        public void AddDependency(AssetInfo target)
        {
            if (depends.Add(target))
            {
                target.Retain();
                target.beDepends.Add(this.bundleName);
            }
        }


        public void ResetLifeTime()
        {
            if (_isReady)
            {
                _readyTime = Time.time;
            }
        }

        /// <summary>
        /// 获取 资源之一
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public virtual Object GetTargetObject(string assetName)
        {
            if (bundle == null)
            {
                Debug.LogError("加载资源错误！" + assetName);
                return null;
            }
            return bundle.LoadAsset(assetName);
        }

        /// <summary>
        /// 引用计数加一
        /// </summary>
        public void Retain()
        // public void AddRef()
        {
            RefCount++;
        }
        /// <summary>
        /// 引用计数减一
        /// </summary>
        public void Release()
        // public void CutDownRef()
        {
            RefCount--;
        }

        /// <summary>
        /// 资源弱引用计数加一
        /// 当有游戏对象引用当前资源单元持有的资源时, 当前资源单元持有该游戏对象的弱引用
        /// </summary>
        /// <param name="owner">用来计算引用计数，如果所有的引用对象被销毁了，那么AB也将会被销毁</param>
        public void Retain(Object owner)
        {
            if (owner == null)
            {
                //Debug.LogError("owner => null :Will destory ");
                return;
            }

            for (int i = 0; i < references.Count; i++)
            {
                // 判断当前是否已经持有该游戏物体的弱引用
                if (owner.Equals(references[i].Target))
                    return;
            }

            WeakReference wr = new WeakReference(owner);
            references.Add(wr);
        }

        /// <summary>
        /// 释放弱引用
        /// </summary>
        /// <param name="owner"></param>
        public void Release(object owner)
        {
            for (int i = 0; i < references.Count; i++)
            {
                if (references[i].Target == owner)
                {
                    references.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="user">增加引用的对象</param>
        /// <returns></returns>
        public virtual GameObject Instantiate()
        {
            return Instantiate(true);
        }

        /// <summary>
        /// 此单元资源被实例化时持有一个实例化对象的弱引用
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public virtual GameObject Instantiate(bool enable = true)
        {
            if (mainObject != null)
            {
                //只有GameObject才可以Instantiate
                if (mainObject is GameObject)
                {
                    GameObject prefab = mainObject as GameObject;
                    prefab.SetActive(enable);
                    Object inst = Object.Instantiate(prefab);
                    inst.name = prefab.name;
                    Retain(inst);
                    return (GameObject)inst;
                }
            }
            return null;
        }

        /// <summary>
        /// 此单元资源被实例化时持有一个实例化对象的弱引用
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public virtual GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null, bool enable = true)
        {
            if (mainObject != null)
            {
                //只有GameObject才可以Instantiate
                if (mainObject is GameObject)
                {
                    GameObject prefab = mainObject as GameObject;
                    prefab.SetActive(enable);
                    Object inst = Object.Instantiate(prefab, position, rotation, parent);
                    inst.name = prefab.name;
                    Retain(inst);
                    return (GameObject)inst;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取此对象
        /// </summary>
        /// <param name="user">增加引用的对象</param>
        /// <returns></returns>
        public Object Require(Object user)
        {
            this.Retain(user);
            return mainObject;
        }

        /// <summary>
        /// 获取文本, 感觉没啥用
        /// </summary>
        public string RequireText()
        {
            return text;
        }

        /// <summary>
        /// 获取对象
        /// 若参数 autoBindGameObject 为true, 则持有 依赖当前资源单元的组件所在GameObject的若引用, 否则 持有该组件的若引用
        /// </summary>
        /// <param name="c">增加引用的Component</param>
        /// <param name="autoBindGameObject">如果为true，则增加引用到它的gameObject对象上</param>
        /// <returns></returns>
        public Object Require(Component c, bool autoBindGameObject)
        {
            if (autoBindGameObject && c && c.gameObject)
                return Require(c.gameObject);
            else
                return Require(c);
        }

        /// <summary>
        /// 获取集合中对象
        /// </summary>
        /// <param name="user">增加引用的对象</param>
        /// <param name="assetName">集合中单个资源名称</param>
        /// <returns></returns>
        public virtual Object Require(Object user, string assetName)
        {
            this.Retain(user);
            return GetTargetObject(assetName);
        }

        /// <summary>
        /// 更新弱引用表, 
        /// </summary>
        /// <returns>返回若引用数量</returns>
        int UpdateReference()
        {
            for (int i = 0; i < references.Count; i++)
            {
                Object o = (Object)references[i].Target;
                if (!o)
                {
                    references.RemoveAt(i);
                    i--;
                }
            }
            return references.Count;
        }

        /// <summary>
        /// 是否没有被依赖且最小生存周期结束了
        /// </summary>
        public bool isUnused
        {
            get { return _isReady && RefCount <= 0 && UpdateReference() == 0 && Time.time - _readyTime > minLifeTime; }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        void UnloadBundle()
        {
            if (bundle != null)
            {
                bundle.Unload(true);
            }
            bundle = null;
        }

        /// <summary>
        /// 销毁逻辑
        /// </summary>
        public virtual void Dispose()
        {
            foreach (Object abi in mainObjectsDic.Values)
            {
                GameObject.Destroy(abi);
            }

            UnloadBundle();
            // 销毁时遍历所有当前资源单元依赖的其他资源单元
            var e = depends.GetEnumerator();
            while (e.MoveNext())
            {
                AssetInfo dep = e.Current;
                dep.beDepends.Remove(this.bundleName);
                dep.Release();
            }
            depends.Clear();
            references.Clear();
            if (onUnloaded != null)
                onUnloaded(this);
        }

        /// <summary>
        /// 从已加载的资源列表中获取资源
        /// </summary>
        /// <param name="user"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public Object RequireAsset(Object user, string assetName)
        {
            this.Retain(user);
            return GetAsset(assetName);
        }

        public Object GetAsset(string assetName)
        {
            if (mainObjectsDic.ContainsKey(assetName))
            {
                return mainObjectsDic[assetName];
            }

            return null;
        }

        /// <summary>
        /// 加载所有包内包含的资源, 并将其储存在字典 mainObjectsDic 中
        /// </summary>
        public void LoadAllAsset()
        {
#if AB_MODE
            Retain();

            mainObjectsDic.Clear();
            Object[] objs = bundle.LoadAllAssets();
            for (int i = 0; i < objs.Length; ++i)
            {
                mainObjectsDic[objs[i].name.ToLower()] = objs[i];
            }
#endif
        }

    }
}