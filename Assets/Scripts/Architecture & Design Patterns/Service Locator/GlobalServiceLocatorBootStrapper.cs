
using UnityEngine;

namespace ServiceLocatorPattern
{
    [AddComponentMenu("Service Locator/Global Service Locator")]
    public class GlobalServiceLocatorBootStrapper : AbstractServiceLocatorBootStrapper
    {
        [SerializeField] bool _dontDestroyOnLoad = true;

        protected override void Bootstrap()
        {
            locator.ConfigureAsGlobal(_dontDestroyOnLoad);
        }
    }
}
