
using UnityEngine;
using InputManagement;

namespace DesignPatterns.ServiceLocatorPattern
{
    [AddComponentMenu("Service Locator/Global Service Locator Bootstrapper")]
    public class GlobalServiceLocatorBootStrapper : AbstractServiceLocatorBootStrapper
    {
        [SerializeField] bool _dontDestroyOnLoad = true;

        protected override void Bootstrap()
        {
            locator.ConfigureAsGlobal(_dontDestroyOnLoad);
            RegisterGlobalServices();
        }

        private void RegisterGlobalServices()
        {
            locator.Register(new InputManager());
            locator.Register(new TouchInputManager());
        }
    }
}
