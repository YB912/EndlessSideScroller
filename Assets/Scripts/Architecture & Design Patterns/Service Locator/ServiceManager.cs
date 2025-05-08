
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.ServiceLocatorPattern
{
    public class ServiceManager
    {
        readonly Dictionary<Type, object> _services = new();

        public bool TryGet<T>(out T outputService) where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                outputService = service as T;
                return true;
            }
            outputService = null;
            return false;
        }

        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return service as T;
            }
            throw new ArgumentException($"Service of type {type.FullName} has not been registered.");
        }

        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            RegisterInternal(type, service);
        }

        public void Register(Type type, object service)
        {
            if (type.IsInstanceOfType(service) == false)
            {
                throw new ArgumentException($"Service {service} is not of type {type.FullName}");
            }

            RegisterInternal(type, service);
        }

        void RegisterInternal(Type type, object service)
        {
            if (_services.TryAdd(type, service) == false)
            {
                Debug.LogError($"Service of type {type.FullName} has already been registered.");
            }
        }
    }
}
