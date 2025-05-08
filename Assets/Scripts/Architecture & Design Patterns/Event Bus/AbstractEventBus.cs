
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
                throw new ArgumentException($"EventBus.cs: The given type {eventType.FullName} is not a UnityEvent.");
            }

            if (_parameterlessEvents.TryGetValue(eventType, out UnityEvent unityEvent))
            {
                unityEvent.AddListener(handler);
            }
            else
            {
                Debug.LogError($"EventBus.cs: Event of type {eventType.FullName} has not been registered.", this);
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
                Debug.LogError($"EventBus.cs: Event of type {eventType.FullName} has not been registered.", this);
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
                Debug.LogError($"EventBus.cs: Event of type {eventType.FullName} has not been registered.", this);
            }
        }
    }
}
