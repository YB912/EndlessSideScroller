
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
            var bootstrappableComponents = module.GetComponents<ISceneBootstrappable>();
            if (bootstrappableComponents.Length == 0)
            {
                Debug.LogError($"MainSceneBootstrapper.BootstrapModulesAsync: There are no ISceneBootstrappable components on {module.name}");
            }
            foreach (var bootstrappableComponent in bootstrappableComponents)
            {
                yield return bootstrappableComponent.BootstrapCoroutine();
            }
        }
    }
}

public interface ISceneBootstrappable
{
    public IEnumerator BootstrapCoroutine();
}
