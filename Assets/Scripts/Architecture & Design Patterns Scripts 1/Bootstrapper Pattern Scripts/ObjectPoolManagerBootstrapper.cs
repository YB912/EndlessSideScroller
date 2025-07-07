
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.ObjectPool
{
    [RequireComponent(typeof(ObjectPoolManager))]
    [DisallowMultipleComponent]
    public class ObjectPoolManagerBootstrapper : MonoBehaviour, IBootstrapper
    {
        public IEnumerator BootstrapCoroutine()
        {
            var objectPoolManager = GetComponent<ObjectPoolManager>();
            objectPoolManager.Bootstrap();
            ServiceLocator.instance.Register(objectPoolManager);
            yield break;
        }
    }
}
