
using DesignPatterns.EventBusPattern;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeController : IDisposable
{
    CameraShakeSettings _settings;
    CinemachineBasicMultiChannelPerlin _cinemachinePerlin;
    GameCycleEventBus _gameCycleEventBus;
    GameplayEventBus _gameplayEventBus;

    MonoBehaviour _coroutineRunner;
    Coroutine _shakeCoroutine;

    public CameraShakeController(CameraShakeSettings settings,
        CinemachineBasicMultiChannelPerlin cinemachinePerlin, 
        GameCycleEventBus gameCycleEventBus,
        GameplayEventBus gameplayEventBus,
        MonoBehaviour coroutineRunner)
    {
        _settings = settings;
        _cinemachinePerlin = cinemachinePerlin;
        _gameplayEventBus = gameplayEventBus;
        _gameCycleEventBus = gameCycleEventBus;
        _coroutineRunner = coroutineRunner;

        _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
        _gameCycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(OnExitedPlayState);
    }

    void OnEnteredPlayState()
    {
        _gameplayEventBus.Subscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
        _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
    }

    void OnExitedPlayState()
    {
        _gameplayEventBus.Unsubscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
        _gameplayEventBus.Unsubscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
    }

    void OnPlayerReceivedDamage()
    {
        if (_shakeCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_shakeCoroutine);
        }
        _shakeCoroutine = _coroutineRunner.
            StartCoroutine(PerformShake(_settings.DamageShakeDuration, _settings.DamageShakeMaxAmplitude, _settings.DamageShakeFrequency));
    }

    void OnPlayerDied()
    {
        if (_shakeCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_shakeCoroutine);
        }
        _shakeCoroutine = _coroutineRunner.
            StartCoroutine(PerformShake(_settings.DeathShakeDuration, _settings.DeathShakeMaxAmplitude, _settings.DeathShakeFrequency));
    }

    IEnumerator PerformShake(float duration, float maxAmplitude, float frequency)
    {
        _cinemachinePerlin.AmplitudeGain = maxAmplitude;
        _cinemachinePerlin.FrequencyGain = frequency;
        var timer = duration;
        while (timer > 0)
        {
            var lerpTime = 1 - (timer / duration);
            var currentAmplitude = Mathf.Lerp(maxAmplitude, 0, lerpTime);
            _cinemachinePerlin.AmplitudeGain = currentAmplitude;
            timer -= Time.deltaTime;
            yield return null;
        }
        _cinemachinePerlin.AmplitudeGain = 0;
        _cinemachinePerlin.FrequencyGain = 0;
    }

    public void Dispose()
    {
        _gameCycleEventBus.Unsubscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
        _gameCycleEventBus.Unsubscribe<ExitedPlayStateGameCycleEvent>(OnExitedPlayState);
        _gameplayEventBus.Unsubscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
        _gameplayEventBus.Unsubscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
    }
}
