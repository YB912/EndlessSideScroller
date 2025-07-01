using UnityEngine;
using System;
using System.Collections.Generic;
using DesignPatterns.Singleton;

namespace DesignPatterns.ServiceLocatorPattern
{
    /// <summary>
    /// A globally accessible service locator based on the Singleton pattern.
    /// Allows registration and retrieval of services by type.
    /// </summary>
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        // Stores registered services by their type
        static readonly Dictionary<Type, object> _services = new();

        /// <summary>
        /// Attempts to retrieve a registered service of type T.
        /// </summary>
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

        /// <summary>
        /// Retrieves a registered service of type T or throws if not found.
        /// </summary>
        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return service as T;
            }
            throw new ArgumentException($"Service of type {type.FullName} has not been registered.");
        }

        /// <summary>
        /// Registers a service instance using its type as the key.
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            RegisterInternal(type, service);
        }

        /// <summary>
        /// Registers a service instance using a specific type.
        /// </summary>
        public void Register(Type type, object service)
        {
            if (type.IsInstanceOfType(service) == false)
            {
                throw new ArgumentException($"Service {service} is not of type {type.FullName}");
            }

            RegisterInternal(type, service);
        }

        /// <summary>
        /// Internal helper for safely registering services with type checks.
        /// </summary>
        void RegisterInternal(Type type, object service)
        {
            if (_services.TryAdd(type, service) == false)
            {
                Debug.LogError($"Service of type {type.FullName} has already been registered.");
            }
        }
    }
}
