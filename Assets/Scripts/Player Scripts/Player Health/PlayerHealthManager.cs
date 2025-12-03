
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObserverPattern;
using DesignPatterns.ServiceLocatorPattern;
using UnityEngine;

namespace Player.Health
{
    public class PlayerHealthManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] PlayerHealthSettings _healthSettings;
        [SerializeField] GlobalDamageSettings _globalDamageSettings;

        GameplayEventBus _gameplayEventBus;

        [HideInInspector] public Observable<float> currentHealthObservable;
        [HideInInspector] public Observable<float> currentHealthNormalizedObservable;

        public void Initialize()
        {
            ServiceLocator.instance.Register(this);

            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();

            currentHealthObservable.value = _healthSettings.playerMaxHealth;
            currentHealthNormalizedObservable.value = 1;

            var bodypartDamageReceivers = GetComponentsInChildren<BodypartDamageReceiver>();
            foreach (var receiver in bodypartDamageReceivers)
            {
                receiver.Initialize(_globalDamageSettings);
            }
        }

        public void RegisterBodypartDamage(float damage)
        {
            if (damage < 0)
            {
                Debug.LogWarning($"Tried to register a negative damage to PlayerHealthManager: {damage} is invalid");
            }
            if (currentHealthObservable.value == 0) return;
            currentHealthObservable.value = Mathf.Max(0f, currentHealthObservable.value - damage);
            currentHealthNormalizedObservable.value = currentHealthObservable.value / _healthSettings.playerMaxHealth;
            if (currentHealthObservable.value <= 0f)
            {
                _gameplayEventBus.Publish<PlayerReachedZeroHealth>();
            }
        }

        public void ResetHealth()
        {
            currentHealthObservable.value = _healthSettings.playerMaxHealth;
            currentHealthNormalizedObservable.value = 1;
        }
    }
}
