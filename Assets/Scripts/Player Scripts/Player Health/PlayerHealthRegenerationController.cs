
using DesignPatterns.EventBusPattern;
using Player.Health;
using System.Collections;
using UnityEngine;
using DesignPatterns.ServiceLocatorPattern;

public class PlayerHealthRegenerationController : MonoBehaviour, IInitializeable
{
    [SerializeField] PlayerHealthSettings _playerHealthSettings;

    PlayerHealthManager _playerHealthManager;

    GameplayEventBus _gameplayEventBus;
    GameCycleEventBus _gameCycleEventBus;

    Coroutine _regenerationCoroutine;

    public void Initialize()
    {
        var serviceLocator = ServiceLocator.instance;
        _gameplayEventBus = serviceLocator.Get<GameplayEventBus>();
        _gameCycleEventBus = serviceLocator.Get<GameCycleEventBus>();
        _playerHealthManager = serviceLocator.Get<PlayerHealthManager>();

        _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
        _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
    }

    void OnPlayerDied()
    {
        if (_regenerationCoroutine != null) StopCoroutine(_regenerationCoroutine);
        _gameplayEventBus.Unsubscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
    }

    void OnEnteredPlayState()
    {
        _gameplayEventBus.Subscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
    }

    void OnPlayerReceivedDamage()
    {
        if (_regenerationCoroutine != null) StopCoroutine(_regenerationCoroutine);
        _regenerationCoroutine = StartCoroutine(RegenerateHealthAfterDelayCoroutine());
    }

    IEnumerator RegenerateHealthAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(_playerHealthSettings.timeBeforeRegenerationStarts);
        while (_playerHealthManager.currentHealthNormalizedObservable.value < 1)
        {
            _playerHealthManager.RegisterRegeneration(
                _playerHealthSettings.normalizedRegenerationPerSecond
                * _playerHealthSettings.playerMaxHealth
                * Time.deltaTime);
            yield return null;
        }
    }
}
