
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.EventBusPattern
{
    public class InputEventBusBootstrapper : MonoBehaviour, ISceneBootstrappable
    {
        public IEnumerator BootstrapCoroutine()
        {
            ServiceLocator.instance.Register(new InputEventBus());
            yield break;
        }
    }
}
