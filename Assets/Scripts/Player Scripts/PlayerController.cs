
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;
using Mechanics.Grappling;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializeable
{
    [SerializeField] PlayerBodyParts _bodyParts;

    GrapplingManager _grapplingManager;

    PlayerSwingingStateMachine _swingingStatemachine;

    public PlayerBodyParts bodyParts => _bodyParts;
    public GrapplingManager grapplingManager => _grapplingManager;

    public void Initialize()
    {
        FetchDependencies();
        var initializeables = GetComponentsInChildren<IInitializeable>();
        foreach (var initializeable in initializeables.Where(i => i.gameObject != gameObject))
        {
            initializeable.Initialize();
        }
        _swingingStatemachine.Resume();
        ServiceLocator.instance.Get<LoadingEventBus>().Publish<PlayerInitializedEvent>();
    }

    void FetchDependencies()
    {
        _grapplingManager = GetComponentInChildren<GrapplingManager>();
        _swingingStatemachine = new PlayerSwingingStateMachine(this);
    }

    [System.Serializable]
    public class PlayerBodyParts
    {
        [SerializeField] GameObject _head;
        public GameObject head => _head;
    }
}
