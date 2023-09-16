using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 *	
 *  Base View
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public abstract class BaseView:MonoBehaviour
    {
        // public GameObject gameObject;
        // public Transform transform;

        public abstract void Initialize(BaseContext context);

        public virtual void OnEnter(BaseContext context)
        {

        }

        public virtual void OnExit(BaseContext context)
        {

        }

        public virtual void OnPause(BaseContext context)
        {

        }

        public virtual void OnResume(BaseContext context)
        {

        }

        protected T GetComponent<T>(string path)
        {
        //     if (typeof(GameObject).IsAssignableFrom(typeof(T)))
        //     {
        //         return transform.Find(path).gameObject as T;
        //     }
            if (transform.Find(path).gameObject.TryGetComponent(out T component))
            {
                return component;
            }
            else
            {
                Debug.LogError("获取组件失败");
                return default;
            }
        }

        protected GameObject GetGameObject(string path)
        {
            Transform ts = transform.Find(path);
            if (ts)
            {
                return ts.gameObject;
            }
            else
            {
                Debug.LogError("获取GameObject失败");
                return null;
            }
        }

        public void DestroySelf()
        {
            OnDestroy();
            GameObject.Destroy(gameObject);
        }

        protected virtual void OnDestroy(){

        }
    }
}
