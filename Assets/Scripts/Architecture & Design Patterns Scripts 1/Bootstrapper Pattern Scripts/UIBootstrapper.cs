
using System.Collections;
using UnityEngine;

public class UIBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _mainMenuPrefab;
    public IEnumerator BootstrapCoroutine()
    {
        var mainMenu = Instantiate(_mainMenuPrefab);
        mainMenu.GetComponent<IInitializeable>().Initialize();
        yield break;
    }
}
