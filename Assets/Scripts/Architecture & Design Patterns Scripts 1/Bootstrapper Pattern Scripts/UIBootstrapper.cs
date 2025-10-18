
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class UIBootstrapper : MonoBehaviour, IBootstrapper
{
    [SerializeField] GameObject _mainMenuPrefab;
    [SerializeField] GameObject _upgradeShopMenuPrefab;
    [SerializeField] GameObject _gameOverMenuPrefab;
    [SerializeField] GameObject _inputUIPrefab;

    public IEnumerator BootstrapCoroutine()
    {
        BootstrapUnityUIEventSystem();
        BootstrapMainMenu();
        BootstrapUpgradeShopMenu();
        BootstrapGameOverMenu();
        BootstrapInputUI();
        yield break;
    }

    void BootstrapUnityUIEventSystem()
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

    void BootstrapUpgradeShopMenu()
    {
        var upgradeMenu = Instantiate(_upgradeShopMenuPrefab);
        upgradeMenu.name = "UpgradeShopMenu";
        upgradeMenu.GetComponent<IInitializeable>().Initialize();
    }

    void BootstrapGameOverMenu()
    {
        var gameOverMenu = Instantiate(_gameOverMenuPrefab);
        gameOverMenu.name = "GameOverMenu";
        gameOverMenu.GetComponent<IInitializeable>().Initialize();
    }

    void BootstrapInputUI()
    {
        var inputUI = Instantiate(_inputUIPrefab);
        inputUI.name = "AimingZoneUI";
        inputUI.GetComponent<IInitializeable>().Initialize();
    }
}
