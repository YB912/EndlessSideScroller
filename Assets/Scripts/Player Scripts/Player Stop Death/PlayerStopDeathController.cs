
using DesignPatterns.ServiceLocatorPattern;
using Player.StateMachines;
using UnityEngine;
using Player;
using DesignPatterns.ObserverPattern;

public class PlayerStopDeathController
{
    PlayerLifeCycleAliveState _aliveState;

    PlayerStopDeathSettings _settings;

    Transform _playerAbdomenTransform;

    Vector3 _previousAbdomenPosition;

    float _oneSecondTimer;
    int _secondsToDie;

    bool _active;

    public Observable<int> countdownNumberToDisplay;

    public PlayerStopDeathController(PlayerLifeCycleAliveState aliveState)
    {
        _aliveState = aliveState;
        var serviceLocator = ServiceLocator.instance;
        var player = serviceLocator.Get<PlayerController>();
        _settings = player.stopDeathSettings;
        _playerAbdomenTransform = player.bodyParts.abdomen.transform;
        countdownNumberToDisplay = new Observable<int>(0);
        serviceLocator.Register<PlayerStopDeathController>(this);
        Activate();
    }

    public void Activate()
    {
        _secondsToDie = _settings.secondsToDie;
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
    }

    public void Update()
    {
        if (_active == false) return;

        _oneSecondTimer += Time.deltaTime;
        if (_oneSecondTimer > 1)
        {
            _oneSecondTimer = 0;
            if (Vector3.Distance(_playerAbdomenTransform.position, _previousAbdomenPosition) < _settings.minMovementThreshold)
            {
                _secondsToDie--;
                if (_secondsToDie <= _settings.secondsToCountDownVisually)
                {
                    countdownNumberToDisplay.value = _secondsToDie;
                }
            }
            else
            {
                _secondsToDie = _settings.secondsToDie;
            }
            _previousAbdomenPosition = _playerAbdomenTransform.position;
        }
        if (_secondsToDie <= 0)
        {
            _secondsToDie = _settings.secondsToDie;
            _aliveState.PlayerStopped();
            Deactivate();
        }
    }
}
