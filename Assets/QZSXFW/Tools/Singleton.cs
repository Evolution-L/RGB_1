using System;
using UnityEngine;

public static class Singleton<T> where T : class
{
    /*	Instance	*/
    private static T _instance;

    /* Static constructor	*/
    static Singleton()
    {
        return;
    }

    public static void Create(params object[] args)
    {
        _instance = (T)Activator.CreateInstance(typeof(T),  args);
        return;
    }
    public static void Create()
    {
        _instance = (T)Activator.CreateInstance(typeof(T), true);
        return;
    }
    public static void Create(T o)
    {
        _instance = o;
        return;
    }

    /* Serve the single instance to callers	*/
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Create();
            }
            return _instance;
        }
    }

    /*	Destroy	*/
    public static void Destroy()
    {
        _instance = null;
        return;
    }
}
