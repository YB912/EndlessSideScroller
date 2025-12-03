
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.Grappling;
using System.Linq;
using Player.StateMachines;
using UnityEngine;
using DesignPatterns.StatePattern;
using Player.Health;

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
        [SerializeField] PlayerStopDeathSettings _stopDeathSettings;
 
        GrapplingManager _grapplingManager;
        IStateMachine _lifeCycleStatemachine;
        IStateMachine _swingingStatemachine;
        PlayerSwingingForceController _swingingForceController;
        PlayerHealthManager _healthManager;
        GameCycleEventBus _gamecycleEventBus;

        public PlayerBodyParts bodyParts => _bodyParts;
        public MainMenuGrapplingOverrideSettings mainMenuGrapplingOverrideSettings => _mainMenuGrapplingOverrideSettings;
        public PlayerStopDeathSettings stopDeathSettings => _stopDeathSettings;
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

            InitializeStatemachines();

            // Notify that player has been initialized
            ServiceLocator.instance.Get<LoadingEventBus>().Publish<PlayerInitializedLoadingEvent>();
            _gamecycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(OnExitedGameCyclePlayState);
        }

        void FetchDependencies()
        {
            _grapplingManager = GetComponentInChildren<GrapplingManager>();
            _swingingForceController = GetComponent<PlayerSwingingForceController>();
            _healthManager = GetComponent<PlayerHealthManager>();
            _gamecycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
        }

        void Update()
        {
            _swingingStatemachine.Update();
            _lifeCycleStatemachine.Update();
        }

        void InitializeStatemachines()
        {
            _lifeCycleStatemachine = new PlayerLifeCycleStatemachine();
            _swingingStatemachine = new PlayerSwingingStatemachine(this);

            _lifeCycleStatemachine.Resume();
            _swingingStatemachine.Resume();
        }

        void OnExitedGameCyclePlayState()
        {
            ResetRagdoll();
            _lifeCycleStatemachine.Resume();
            _swingingStatemachine.Resume();
        }

        void ResetRagdoll()
        {
            ResetRagdollPosition();
            ResetRagdollVelocity();
            _healthManager.ResetHealth();
            _gamecycleEventBus.Publish<RagdollResetGameCycleEvent>();
        }

        void ResetRagdollPosition()
        {
            foreach (var part in _bodyParts.allParts)
            {
                part.transform.localPosition = Vector3.zero;
            }
        }

        void ResetRagdollVelocity()
        {
            foreach (var part in _bodyParts.allParts)
            {
                var rigidbody = part.GetComponent<Rigidbody2D>();
                rigidbody.linearVelocity = Vector3.zero;
                rigidbody.angularVelocity = 0;
            }
        }
    }
}
