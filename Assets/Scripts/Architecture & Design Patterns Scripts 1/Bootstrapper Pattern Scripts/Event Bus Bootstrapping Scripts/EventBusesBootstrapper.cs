using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Collections;
using UnityEngine;

public class EventBusesBootstrapper : MonoBehaviour, IBootstrapper
{
    public IEnumerator BootstrapCoroutine()
    {
        ServiceLocator.instance.Register(new GrapplingEventBus());
        ServiceLocator.instance.Register(new InputEventBus());
        ServiceLocator.instance.Register(new LoadingEventBus());
        ServiceLocator.instance.Register(new GameplayEventBus());
        yield break;
    }
}
