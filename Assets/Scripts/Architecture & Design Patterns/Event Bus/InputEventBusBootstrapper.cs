
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.EventBusPattern
{
    public class InputEventBusBootstrapper : MonoBehaviour, ISceneBootstrappable
    {
        public IEnumerator BootstrapCoroutine()
        {
            var sceneServiceLocator = ServiceLocator.ForSceneOf(this);
            sceneServiceLocator.Register(new InputEventBus());
            yield break;
        }
    }
}
