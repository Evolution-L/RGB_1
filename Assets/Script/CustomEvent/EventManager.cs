using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public interface IEventEntity
    {
        public void ClearAllListeners();
        public void Dispatch(IEventArgs eventArgs);
    }
    public class EventEntity<T> : IEventEntity where T : IEventArgs
    {
        Action<T> action;
        public void Dispatch(IEventArgs eventArges)
        {
            action?.Invoke((T)eventArges);
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

        public static void Dispatch<T>(T eventArges) where T : IEventArgs
        {
            var eventType = typeof(T);
            // var eventType = eventArges.GetType();
            if (eventEntitys.TryGetValue(eventType, out IEventEntity ieventEntity))
            {
                ieventEntity.Dispatch(eventArges);
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
    }
}
