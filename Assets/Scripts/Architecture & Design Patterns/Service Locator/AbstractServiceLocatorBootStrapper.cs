
using UnityEngine;

namespace DesignPatterns.ServiceLocatorPattern
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class AbstractServiceLocatorBootStrapper : MonoBehaviour
    {
        ServiceLocator _locator;
        bool _hasBeenBootstrapped;

        internal ServiceLocator locator => _locator ?? (_locator = GetComponent<ServiceLocator>());

        void Awake()
        {
            BootStrapOnDemand();
        }

        public void BootStrapOnDemand()
        {
            if (_hasBeenBootstrapped == false)
            {
                _hasBeenBootstrapped = true;
                Bootstrap();
            }
        }

        protected abstract void Bootstrap();
    }
}
