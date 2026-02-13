
using DesignPatterns.EventBusPattern;
using UnityEngine;
using DesignPatterns.ServiceLocatorPattern;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] SFXList _sfxList;
        [SerializeField] MusicList _musicList;

        SFXManager _sfxManager;
        MusicManager _musicManager;

        GameCycleEventBus _gameCycleEventBus;
        GameplayEventBus _gameplayEventBus;
        GrapplingEventBus _grapplingEventBus;

        public void Initialize()
        {
            _sfxManager = new SFXManager(_sfxList, gameObject);
            _musicManager = new MusicManager(_musicList, gameObject);

            FetchDependencies();
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(OnEnteredPlayState);
            _gameCycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(OnExitedPlayState);
            _grapplingEventBus.Subscribe<GrapplerFiredGrapplingEvent>(OnPlayerFiredGrapplingHook);


            PlayMusicSuddenly(MusicType.BACKGROUND_MUSIC);
        }

        public void PlaySFXDefaultPitch(SoundEffectType type)
        {
            _sfxManager.PlayDefaultPitch(type);
        }

        public void PlaySFXRandomPitch(SoundEffectType type)
        {
            _sfxManager.PlayRandomPitch(type);
        }

        public void PlayMusicSuddenly(MusicType type, bool isLooped = true)
        {
            _musicManager.PlaySuddenly(type, isLooped);
        }
        
        void FetchDependencies()
        {
            var serviceLocator = ServiceLocator.instance;
            _gameCycleEventBus = serviceLocator.Get<GameCycleEventBus>();
            _gameplayEventBus = serviceLocator.Get<GameplayEventBus>();
            _grapplingEventBus = serviceLocator.Get<GrapplingEventBus>();
        }

        void OnEnteredPlayState()
        {
            OnGameStarted();
            _gameplayEventBus.Subscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
            _gameplayEventBus.Subscribe<PlayerHitCeilingLaserGameplayEvent>(OnPlayerHitCeilingLaser);
            _gameplayEventBus.Subscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
        }

        void OnExitedPlayState()
        {
            _gameplayEventBus.Unsubscribe<PlayerDiedGameplayEvent>(OnPlayerDied);
            _gameplayEventBus.Unsubscribe<PlayerHitCeilingLaserGameplayEvent>(OnPlayerHitCeilingLaser);
            _gameplayEventBus.Unsubscribe<PlayerReceivedDamageGameplayEvent>(OnPlayerReceivedDamage);
        }

        void OnGameStarted()
        {
            PlaySFXRandomPitch(SoundEffectType.GAME_START);
        }

        void OnPlayerFiredGrapplingHook()
        {
            PlaySFXRandomPitch(SoundEffectType.HOOK_WHOOSH);
            PlaySFXRandomPitch(SoundEffectType.HOOK_ATTACH);
        }

        void OnPlayerReceivedDamage()
        {
            PlaySFXRandomPitch(SoundEffectType.DAMAGE);
        }

        void OnPlayerHitCeilingLaser()
        {
            PlaySFXRandomPitch(SoundEffectType.LASER_HIT);
        }

        void OnPlayerDied()
        {
            PlaySFXDefaultPitch(SoundEffectType.DEATH);
        }
    }
}
