
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

        public bool IsPlayerAlive()
        {
            return currentHealthObservable.value > 0f;
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
            _gameplayEventBus.Publish<PlayerReceivedDamageGameplayEvent>();
            if (currentHealthObservable.value <= 0f)
            {
                _gameplayEventBus.Publish<PlayerReachedZeroHealth>();
            }
        }

        public void RegisterRegeneration(float regenerationAmount)
        {
            if (regenerationAmount < 0)
            {
                Debug.LogWarning($"Tried to register a negative regeneration to PlayerHealthManager: {regenerationAmount} is invalid");
            }
            if (currentHealthObservable.value == _healthSettings.playerMaxHealth) return;
            currentHealthObservable.value = Mathf.Min(_healthSettings.playerMaxHealth, currentHealthObservable.value + regenerationAmount);
            currentHealthNormalizedObservable.value = currentHealthObservable.value / _healthSettings.playerMaxHealth;
        }

        public void ResetHealth()
        {
            currentHealthObservable.value = _healthSettings.playerMaxHealth;
            currentHealthNormalizedObservable.value = 1;
        }
    }
}
