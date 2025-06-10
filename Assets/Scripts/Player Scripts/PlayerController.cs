using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializeable
{
    [SerializeField] PlayerBodyParts _bodyParts;

    public PlayerBodyParts bodyParts => _bodyParts;

    public void Initialize()
    {
        var initializeables = GetComponentsInChildren<IInitializeable>();
        foreach (var initializeable in initializeables.Where(i => i.gameObject != gameObject))
        {
            initializeable.Initialize();
        }
        ServiceLocator.instance.Get<LoadingEventBus>().Publish<PlayerInitializedEvent>();
    }

    [System.Serializable]
    public class PlayerBodyParts
    {
        [SerializeField] GameObject _head;
        public GameObject head => _head;
    }
}
