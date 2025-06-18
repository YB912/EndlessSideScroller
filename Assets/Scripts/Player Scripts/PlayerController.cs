
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;
using Mechanics.Grappling;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerSwingingForceController))]
public class PlayerController : MonoBehaviour, IInitializeable
{
    [SerializeField] PlayerBodyParts _bodyParts;

    GrapplingManager _grapplingManager;

    PlayerSwingingStateMachine _swingingStatemachine;

    PlayerSwingingForceController _swingingForceController;

    public PlayerBodyParts bodyParts => _bodyParts;
    public GrapplingManager grapplingManager => _grapplingManager;
    public PlayerSwingingForceController swingingForceController => _swingingForceController;

    public void Initialize()
    {
        FetchDependencies();
        var initializeables = GetComponentsInChildren<IInitializeable>();
        IInitializeable self = this;
        foreach (var initializeable in initializeables.Where(i => i != self))
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
        _swingingForceController = GetComponent<PlayerSwingingForceController>();
    }

    [System.Serializable]
    public class PlayerBodyParts
    {
        [SerializeField] GameObject _head;
        [SerializeField] GameObject _abdomen;
        [SerializeField] GameObject _backUpperLeg;
        [SerializeField] GameObject _frontUpperLeg;
        [SerializeField] GameObject _backForearm;
        public GameObject head => _head;
        public GameObject abdomen => _abdomen;
        public GameObject backUpperLeg => _backUpperLeg;
        public GameObject frontUpperLeg => _frontUpperLeg;
        public GameObject backForearm => _backForearm;
    }
}
