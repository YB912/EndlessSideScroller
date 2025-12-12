
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

namespace Player.Health
{
    public class BodypartDamageReceiver : MonoBehaviour
    {
        [SerializeField] BodypartDamageSettings _bodypartDamageSettings;

        Rigidbody2D _rigidBody;

        PlayerHealthManager _healthManager;
        GlobalDamageSettings _globalDamageSettings;

        GameplayEventBus _gameplayEventBus;
        GameCycleEventBus _gameCycleEventBus;

        const string WALL_LAYER_NAME = "Wall";

        bool _shouldReceiveDamage;

        public void Initialize(GlobalDamageSettings globalDamageSettings)
        {
            var serviceLocator = ServiceLocator.instance;
            _rigidBody = GetComponent<Rigidbody2D>();
            _healthManager = serviceLocator.Get<PlayerHealthManager>();
            _globalDamageSettings = globalDamageSettings;
            _gameplayEventBus = serviceLocator.Get<GameplayEventBus>();    
            _gameCycleEventBus = serviceLocator.Get<GameCycleEventBus>();

            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_shouldReceiveDamage == false) return;
            if (collision.gameObject.layer != LayerMask.NameToLayer(WALL_LAYER_NAME)) return;

            var force = _rigidBody.mass * collision.relativeVelocity.magnitude / Time.fixedDeltaTime;
            force = Mathf.Min(force, _globalDamageSettings.maxImpactForce);

            if (force <= _globalDamageSettings.impactForceThreshold) return;
            var velocityDirection = _rigidBody.linearVelocity.normalized;
            var normal = collision.GetContact(0).normal;
            var perpendicularFactor = Vector2.Dot(-normal, velocityDirection);
            perpendicularFactor = Mathf.Clamp01(perpendicularFactor);
            var normalizedForce = Mathf.InverseLerp(_globalDamageSettings.impactForceThreshold, _globalDamageSettings.maxImpactForce, force);
            var forceDamageCoeficient = _globalDamageSettings.impactForceToDamageCurve.Evaluate(normalizedForce);
            var damage = _globalDamageSettings.maxImpactDamage * _bodypartDamageSettings.impactSensitivity * forceDamageCoeficient * perpendicularFactor;
            _healthManager.RegisterBodypartDamage(damage);
        }

        void OnPlayerDied()
        {
            _shouldReceiveDamage = false;
        }

        void OnEnteredPlayState()
        {
            _shouldReceiveDamage = true;
        }
    }
}
