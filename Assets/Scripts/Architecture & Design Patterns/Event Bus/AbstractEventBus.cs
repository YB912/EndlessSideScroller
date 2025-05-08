
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DesignPatterns.ServiceLocatorPattern
{
    public abstract class AbstractEventBus : MonoBehaviour
    {
        protected Dictionary<Type, UnityEvent> _parameterlessEvents = new();

        public void Subscribe<T>(UnityAction handler)
        {
            var eventType = typeof(T);
            if (typeof(UnityEvent).IsAssignableFrom(eventType) == false)
            {
                throw new ArgumentException($"AbstractEventBus.Subscribe: The given type {eventType.FullName} is not a UnityEvent.");
            }

            if (_parameterlessEvents.TryGetValue(eventType, out UnityEvent unityEvent))
            {
                unityEvent.AddListener(handler);
            }
            else
            {
                throw new ArgumentException($"AbstractEventBus.Subscribe: Event of type {eventType.FullName} has not been registered.");
            }
        }

        public void Unsubscribe<T>(UnityAction handler)
        {
            var eventType = typeof(T);
            if (_parameterlessEvents.TryGetValue(eventType, out UnityEvent unityEvent))
            {
                unityEvent.RemoveListener(handler);
            }
            else
            {
                throw new ArgumentException($"AbstractEventBus.Unsubscribe: Event of type {eventType.FullName} has not been registered.");
            }
        }

        public void Publish<T>()
        {
            var eventType = typeof(T);
            if (_parameterlessEvents.TryGetValue(eventType, out UnityEvent unityEvent))
            {
                unityEvent.Invoke();
            }
            else
            {
                throw new ArgumentException($"AbstractEventBus.Publish: Event of type {eventType.FullName} has not been registered.");
            }
        }
    }
}
