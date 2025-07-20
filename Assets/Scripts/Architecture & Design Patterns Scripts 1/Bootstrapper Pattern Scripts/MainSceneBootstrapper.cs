
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

public class MainSceneBootstrapper : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(BootstrapModulesAsync());
    }

    IEnumerator BootstrapModulesAsync()
    {
        foreach (Transform module in transform)
        {
            if (module.gameObject.activeInHierarchy == false) { continue; }
            var bootstrappableComponents = module.GetComponents<IBootstrapper>();
            if (bootstrappableComponents.Length == 0)
            {
                Debug.LogError($"MainSceneBootstrapper.BootstrapModulesAsync: There are no ISceneBootstrappable components on {module.name}");
            }
            foreach (var bootstrappableComponent in bootstrappableComponents)
            {
                yield return bootstrappableComponent.BootstrapCoroutine();
            }
        }
        ServiceLocator.instance.Get<LoadingEventBus>().Publish<MainSceneBootstrappedEvent>();
    }
}

public interface IBootstrapper
{
    public IEnumerator BootstrapCoroutine();
}
