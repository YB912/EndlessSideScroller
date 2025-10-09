
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using Player;
using System.Data.Common;
using UnityEngine;

namespace Mechanics.Scoring
{
    public class ScoreManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] ScoringSettings _scoringSettings;

        Transform _playerAbdomenTransform;
        GameplayEventBus _gameplayEventBus;
        LoadingEventBus _loadingEventBus;

        int _totalScore;
        int _scoreFromDistance;

        float _originX;

        public int totalScore => _totalScore;
        public int scoreFromDistance => _scoreFromDistance;

        public void Initialize()
        {
            FetchDependencies();
            _gameplayEventBus.Subscribe<LaunchSequenceCompletedGameplayEvent>(OnLaunchSequenceCompleted);
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
            _loadingEventBus.Subscribe<PlayerInitializedLoadingEvent>(OnPlayerInitialized);
            ServiceLocator.instance.Register<ScoreManager>(this);
        }

        void FetchDependencies()
        {
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _loadingEventBus = ServiceLocator.instance.Get<LoadingEventBus>();
        }

        void UpdateTotalScore()
        {
            UpdateScoreFromDistance();
            _totalScore = _scoreFromDistance;
        }

        void UpdateScoreFromDistance()
        {
            _scoreFromDistance = Mathf.RoundToInt((_playerAbdomenTransform.position.x - _originX) * _scoringSettings.distanceScoreMultiplier);
        }

        void OnLaunchSequenceCompleted()
        {
            _originX = _playerAbdomenTransform.position.x;
        }

        void OnPlayerDied()
        {
            UpdateTotalScore();
        }

        void OnPlayerInitialized()
        {
            _playerAbdomenTransform = ServiceLocator.instance.Get<PlayerController>().bodyParts.abdomen.transform;
        }
    }
}
