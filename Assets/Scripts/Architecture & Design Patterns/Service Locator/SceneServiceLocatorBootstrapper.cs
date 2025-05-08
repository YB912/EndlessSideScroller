
using UnityEngine;

namespace DesignPatterns.ServiceLocatorPattern
{
    [AddComponentMenu("Service Locator/Scene Service Locator")]
    public class SceneServiceLocatorBootstrapper : AbstractServiceLocatorBootStrapper
    {
        protected override void Bootstrap()
        {
            locator.ConfigureForScene();
        }
    }
}
