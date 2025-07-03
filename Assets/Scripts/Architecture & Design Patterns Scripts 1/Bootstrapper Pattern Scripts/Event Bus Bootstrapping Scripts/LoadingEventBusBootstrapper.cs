
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.EventBusPattern
{
    public class LoadingEventBusBootstrapper : MonoBehaviour, ISceneBootstrappable
    {
        public IEnumerator BootstrapCoroutine()
        {
            ServiceLocator.instance.Register(new LoadingEventBus());
            yield break;
        }
    }
}
