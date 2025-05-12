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
            GetOrCreateEvent<T>().Invoke();
        }

        public void Subscribe<T>(UnityAction handler)
        {
            GetOrCreateEvent<T>().AddListener(handler);
        }

        public void Unsubscribe<T>(UnityAction handler, object caller)
        {
            GetOrCreateEvent<T>().RemoveListener(handler);
        }

        private UnityEvent GetOrCreateEvent<T>()
        {
            var eventType = typeof(T);
            if (_events.TryGetValue(eventType, out var unityEvent) == false)
            {
                _events.Add(eventType, RegisterEvent(eventType));
            }
            return unityEvent;
        }

        private UnityEvent RegisterEvent(Type eventType)
        {
            if (_events.ContainsKey(eventType) == false)
            {
                var unityEvent = new UnityEvent();
                _events.Add(eventType, unityEvent);
                return unityEvent;
            }
            else
            {
                throw new ArgumentException($"EventBus.RegisterEvent: Event of type {eventType} has already been registered.");
            }
        }
    }

    public class InputEventBus : EventBus { }
}