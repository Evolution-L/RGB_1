using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public interface IEventEntity
    {
        public void ClearAllListeners();
    }
    public class EventEntity<T> : IEventEntity where T : IEventArgs
    {
        Action<T> action;
        public void Dispatch(T eventArges)
        {
            action?.Invoke(eventArges);
        }

        public void Register(Action<T> action)
        {
            this.action += action;
        }

        public void UnRegister(Action<T> action)
        {
            this.action -= action;
        }

        public void ClearAllListeners()
        {
            this.action = null;
        }
    }

    public static class EventManager
    {
        private static readonly Dictionary<Type, IEventEntity> eventEntitys = new();

        public static EventEntity<T> GetEventManager<T>() where T : IEventArgs
        {
            var eventType = typeof(T);
            if (!eventEntitys.ContainsKey(eventType))
            {
                eventEntitys[eventType] = new EventEntity<T>();
            }

            return (EventEntity<T>)eventEntitys[eventType];
        }

        public static void Dispatch<T>(T eventArges) where T : IEventArgs
        {
            // var eventType = typeof(T);
            var eventType = eventArges.GetType();
            if (eventEntitys.TryGetValue(eventType, out IEventEntity ieventEntity))
            {
                EventEntity<T> eventEntity = (EventEntity<T>)ieventEntity;
                eventEntity.Dispatch(eventArges);
            }
        }

        public static void Register<T>(Action<T> action) where T : IEventArgs
        {
            var eventType = typeof(T);
            if (eventEntitys.TryGetValue(eventType, out IEventEntity ieventEntity))
            {
                EventEntity<T> eventEntity = (EventEntity<T>)ieventEntity;
                eventEntity.Register(action);
            }
            else
            {
                EventEntity<T> eventEntity = new();
                eventEntity.Register(action);
                eventEntitys[eventType] = eventEntity;
            }

        }

        public static void UnRegister<T>(Action<T> action) where T : IEventArgs
        {
            var eventType = typeof(T);
            if (eventEntitys.TryGetValue(eventType, out IEventEntity ieventEntity))
            {
                EventEntity<T> eventEntity = (EventEntity<T>)ieventEntity;
                eventEntity.UnRegister(action);
            }
        }

        public static void ClearAllListeners()
        {
            foreach (var ieventEntity in eventEntitys.Values)
            {
                if (ieventEntity is IEventEntity eventEntity)
                {
                    eventEntity.ClearAllListeners();
                }
            }
            eventEntitys.Clear();
        }

        public static void A<T>(T a)
        {

        }

        public static void B()
        {
            A<int>(1);
            A<string>("q");
            A<bool>(false);
            A<float>(1);
            A<long>(1);
        }
    }
}
