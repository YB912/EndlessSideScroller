
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.EventBusPattern
{
    public class GrapplingEventBusBootstrapper : MonoBehaviour, ISceneBootstrappable
    {
        public IEnumerator BootstrapCoroutine()
        {
            ServiceLocator.instance.Register(new GrapplingEventBus());
            yield break;
        }
    }
}
