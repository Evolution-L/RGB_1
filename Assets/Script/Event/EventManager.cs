using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public static class EventManager
    // public static class EventManager 
    {
        private static Dictionary<EventDefine, Delegate> m_EventTable = new();

        private static void OnListenerAdding(EventDefine eventType, Delegate callBack)
        {
            if (!EventManager.m_EventTable.ContainsKey(eventType))
            {
                EventManager.m_EventTable.Add(eventType, null);
            }
            Delegate @delegate = EventManager.m_EventTable[eventType];
            if (@delegate != null && @delegate.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, @delegate.GetType(), callBack.GetType()));
            }
        }

        private static void OnListenerRemoving(EventDefine eventType, Delegate callBack)
        {
            if (!EventManager.m_EventTable.ContainsKey(eventType))
            {
                throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
            }
            Delegate @delegate = EventManager.m_EventTable[eventType];
            if (@delegate == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            if (@delegate.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, @delegate.GetType(), callBack.GetType()));
            }
        }

        private static void OnListenerRemoved(EventDefine eventType)
        {
            if (EventManager.m_EventTable[eventType] == null)
            {
                EventManager.m_EventTable.Remove(eventType);
            }
        }

        public static void AddListener(EventDefine eventType, CallBack callBack)
        {
            EventManager.OnListenerAdding(eventType, callBack);
            EventManager.m_EventTable[eventType] = (CallBack)Delegate.Combine((CallBack)EventManager.m_EventTable[eventType], callBack);
        }
        public static void RemoveListener(EventDefine eventType, CallBack callBack)
        {
            EventManager.OnListenerRemoving(eventType, callBack);
            EventManager.m_EventTable[eventType] = (CallBack)Delegate.Remove((CallBack)EventManager.m_EventTable[eventType], callBack);
            EventManager.OnListenerRemoved(eventType);
        }

        public static void Broadcast(EventDefine eventType)
        {
            Delegate @delegate;
            if (!EventManager.m_EventTable.TryGetValue(eventType, out @delegate))
            {
                return;
            }
            CallBack callBack = @delegate as CallBack;
            if (callBack != null)
            {
                callBack();
                return;
            }
            throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
        }

        public static void Broadcast<T>(EventDefine eventType, T args)
        {
            Delegate @delegate;
            if (!EventManager.m_EventTable.TryGetValue(eventType, out @delegate))
            {
                return;
            }
            CallBack<T> callBack = @delegate as CallBack<T>;
            if (callBack != null)
            {
                callBack(args);
                return;
            }
            throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
        }
    }


    public delegate void CallBack();
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T, U>(T arg, U arg1);
    public delegate void CallBack<T, U, X>(T arg, U arg1, X arg2);
    public delegate void CallBack<T, U, X, Y>(T arg, U arg1, X arg2, Y arg3);
}
