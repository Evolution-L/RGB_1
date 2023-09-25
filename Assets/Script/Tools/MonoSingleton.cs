// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.VersionControl;
// using UnityEngine;

// public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
// {
//     private static T instance;
//     public static T Instance
//     {
//         get
//         {
//             if (instance == null)
//             {
//                 GameObject go = GameObject.Find(typeof(T).Name);
//                 if (go == null)
//                 {
//                     go = new GameObject(typeof(T).Name);
//                 }
//                 try
//                 {
//                     instance = go.GetComponent<T>();
//                     if (instance == null)
//                         instance = go.AddComponent<T>();
//                 }
//                 catch
//                 {
//                     Debug.Log(typeof(T).Name);
//                 }
                
//                 DontDestroyOnLoad(go);
//                 instance.Init();
//             }
//             return instance;
//         }
//     }

//     public virtual void Init(){

//     }

//     /// <summary>
//     /// ���ٵ���
//     /// </summary>
//     public virtual void Discard()
//     {
//         Destroy(gameObject);
//         instance = null;
//     }
// }
