
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DesignPatterns.ServiceLocatorPattern
{
    public class ServiceLocator : MonoBehaviour
    {
        static ServiceLocator _global;
        static Dictionary<Scene, ServiceLocator> _sceneServiceLocators = new();

        static List<GameObject> _tempSceneGameObjects;

        readonly ServiceManager _serviceManager;

        const string _globalServiceLocatorName = "Service Locator (Global)";
        const string _sceneServiceLocatorName = "Service Locator (Scene)";

        public static ServiceLocator global
        {
            get
            {
                if (_global != null) return _global;

                var bootstrapper = FindFirstObjectByType<GlobalServiceLocatorBootStrapper>();
                if (bootstrapper != null)
                {
                    bootstrapper.BootStrapOnDemand();
                    return _global;
                }

                var container = new GameObject(_globalServiceLocatorName);
                container.AddComponent<ServiceLocator>();
                container.AddComponent<GlobalServiceLocatorBootStrapper>().BootStrapOnDemand();

                return _global;
            }
        }

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (_global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as gloabal.");
            } else if (_global != null)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Another locator already configured as global.");
            } else
            {
                _global = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (_sceneServiceLocators.ContainsKey(scene))
            {
                Debug.LogWarning("ServiceLocator.ConfigureForScene: Another locator already configured for this scene.");
                return;
            }

            _sceneServiceLocators.Add(scene, this);
        }

        public static ServiceLocator For(MonoBehaviour monoBehaviour)
        {
            var output = monoBehaviour.GetComponentInParent<ServiceLocator>();
            if (output != null)
            {
                return output;
            }

            output = ForSceneOf(monoBehaviour);
            if (output != null)
            {
                return output;
            }

            return global;
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour)
        {
            Scene scene = monoBehaviour.gameObject.scene;

            if (_sceneServiceLocators.TryGetValue(scene, out ServiceLocator serviceLocatorContainer) && serviceLocatorContainer != monoBehaviour)
            {
                return serviceLocatorContainer;
            }

            _tempSceneGameObjects.Clear();

            scene.GetRootGameObjects(_tempSceneGameObjects);

            foreach (var gameObject in _tempSceneGameObjects.Where(gameObject => gameObject.GetComponent<SceneServiceLocatorBootstrapper>() != null))
            {
                if (gameObject.TryGetComponent(out SceneServiceLocatorBootstrapper bootstrapper) && bootstrapper.locator != monoBehaviour)
                {
                    bootstrapper.BootStrapOnDemand();
                    return bootstrapper.locator;
                }
            }

            return global;
        }

        public void Register<T>(T service) where T : class
        {
            _serviceManager.Register(service);
        }

        public void Register(Type type, object service)
        {
            _serviceManager.Register(type, service);
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service))
            {
                return this;
            }

            if (TryGetNextInHierarchy(out ServiceLocator locator))
            {
                locator.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get: Serivce of type {typeof(T).FullName} has not been registered.");
        }

        bool TryGetService<T>(out T service) where T : class
        {
            return _serviceManager.TryGet(out service);
        }

        bool TryGetNextInHierarchy(out ServiceLocator locator)
        {
            if (this == _global)
            {
                locator = null;
                return false;
            }

            locator = transform.parent?.GetComponentInParent<ServiceLocator>();
            if (locator == null)
            {
                locator = ForSceneOf(this);
            }

            return locator != null;
        }

        private void OnDestroy()
        {
            if (_global == this)
            {
                _global = null;
            } else
            {
                _sceneServiceLocators.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetStatics()
        {
            _global = null;
            _sceneServiceLocators = new Dictionary<Scene, ServiceLocator>();
            _tempSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        static void AddGlobal()
        {
            var gameObject = new GameObject(_globalServiceLocatorName, typeof(GlobalServiceLocatorBootStrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add For Scene")]
        static void AddForScene()
        {
            var gameObject = new GameObject(_sceneServiceLocatorName, typeof(SceneServiceLocatorBootstrapper));
        }
#endif
    }
}
