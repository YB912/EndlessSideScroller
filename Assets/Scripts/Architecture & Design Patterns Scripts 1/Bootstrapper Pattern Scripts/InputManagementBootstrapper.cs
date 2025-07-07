using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using System.Collections;
using UnityEngine;

public class InputManagementBootstrapper : MonoBehaviour, IBootstrapper
{
    public IEnumerator BootstrapCoroutine()
    {
        yield return StartCoroutine(InitializeInputManager());
        yield return StartCoroutine(InitializeTouchInputManager());

        yield break;
    }

    private IEnumerator InitializeInputManager()
    {
        var inputManager = IInputManagementFactory.CreateInputManager();
        ServiceLocator.instance.Register(inputManager);
        yield break;
    }

    private IEnumerator InitializeTouchInputManager()
    {
        var touchInputManager = IInputManagementFactory.CreateTouchInputManager();
        ServiceLocator.instance.Register(touchInputManager);
        yield break;
    }
}
