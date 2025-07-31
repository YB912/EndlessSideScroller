
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.Grappling;
using System.Linq;
using Player.StateMachines;
using UnityEngine;
using DesignPatterns.StatePattern;

namespace Player
{
    /// <summary>
    /// Root player controller that coordinates player parts, grappling system, and swinging state machine.
    /// </summary>
    [RequireComponent(typeof(PlayerSwingingForceController))]
    public class PlayerController : MonoBehaviour, IInitializeable
    {
        [SerializeField] PlayerBodyParts _bodyParts;
        [SerializeField] MainMenuGrapplingOverrideSettings _mainMenuGrapplingOverrideSettings;
 
        GrapplingManager _grapplingManager;
        IStateMachine _lifeCycleStatemachine;
        IStateMachine _swingingStatemachine;
        PlayerSwingingForceController _swingingForceController;
        GameplayEventBus _gameplayEventBus;

        public PlayerBodyParts bodyParts => _bodyParts;
        public MainMenuGrapplingOverrideSettings mainMenuGrapplingOverrideSettings => _mainMenuGrapplingOverrideSettings;
        public GrapplingManager grapplingManager => _grapplingManager;
        public PlayerSwingingForceController swingingForceController => _swingingForceController;

        public void Initialize()
        {
            FetchDependencies();

            // Initialize all child components except self
            var initializeables = GetComponentsInChildren<IInitializeable>();
            foreach (var initializeable in initializeables.Where(i => i != (IInitializeable)this))
            {
                initializeable.Initialize();
            }

            _swingingStatemachine = new PlayerSwingingStatemachine(this);
            _lifeCycleStatemachine = new PlayerLifeCycleStatemachine();

            _lifeCycleStatemachine.Resume();
            _swingingStatemachine.Resume();

            // Notify that player has been initialized
            ServiceLocator.instance.Get<LoadingEventBus>().Publish<PlayerInitializedEvent>();
        }

        void FetchDependencies()
        {
            _grapplingManager = GetComponentInChildren<GrapplingManager>();
            _swingingForceController = GetComponent<PlayerSwingingForceController>();
        }

        void OnGameRestarted()
        {
            _lifeCycleStatemachine.Resume();
            _swingingStatemachine.Resume();
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }

        /// <summary>
        /// Structurally defines the key rigid body parts of the player used in physics and animation.
        /// </summary>
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
}
