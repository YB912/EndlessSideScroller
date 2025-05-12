
using System.Collections;
using UnityEngine;

public class MainSceneBootstrapper : MonoBehaviour
{
    [SerializeField] GameObject[] _modulesToBootstrapAtAwake;

    private void Awake()
    {
        StartCoroutine(BootstrapModulesAsync());
    }

    private IEnumerator BootstrapModulesAsync()
    {
        foreach (var module in _modulesToBootstrapAtAwake)
        {
            if (module == null)
            {
                Debug.LogError($"MainSceneBootstrapper.CentralBootstrapCoroutine: Null reference found in {nameof(_modulesToBootstrapAtAwake)}");
                continue;
            }
            var bootstrappableComponents = module.GetComponents<ISceneBootstrappable>();
            if (bootstrappableComponents.Length == 0)
            {
                Debug.LogError($"MainSceneBootstrapper.CentralBootstrapCoroutine: There are no ISceneBootstrappable components on {module.name}");
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
