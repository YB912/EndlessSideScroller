using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace DesignPatterns.EventBusPattern
{
    public abstract class EventBus
    {
        protected Dictionary<Type, UnityEvent> _events;

        public EventBus()
        {
            _events = new Dictionary<Type, UnityEvent>();
        }

        public void Publish<T>()
        {
            GetOrCreateEvent<T>()?.Invoke();
        }

        public void Subscribe<T>(UnityAction handler)
        {
            GetOrCreateEvent<T>().AddListener(handler);
        }

        public void Unsubscribe<T>(UnityAction handler, object caller)
        {
            GetOrCreateEvent<T>().RemoveListener(handler);
        }

        UnityEvent GetOrCreateEvent<T>()
        {
            var eventType = typeof(T);

            if (_events.TryGetValue(eventType, out var unityEvent))
            {
                return unityEvent;
            }

            var newEvent = new UnityEvent();
            _events.Add(eventType, newEvent);
            return newEvent;
        }
    }

    public class InputEventBus : EventBus { }

    public class GrapplingEventBus : EventBus { }

    public class LoadingEventBus : EventBus { }
}