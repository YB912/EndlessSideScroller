
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class UIBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _mainMenuPrefab;
    public IEnumerator BootstrapCoroutine()
    {
        BootstrapEventSystem();
        BootstrapMainMenu();
        yield break;
    }

    void BootstrapEventSystem()
    {
        var eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<InputSystemUIInputModule>();
    }

    void BootstrapMainMenu()
    {
        var mainMenu = Instantiate(_mainMenuPrefab);
        mainMenu.name = "MainMenu";
        mainMenu.GetComponent<IInitializeable>().Initialize();
    }
}
