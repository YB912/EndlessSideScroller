
using UnityEngine;
using System.Collections.Generic;
using DesignPatterns.EventBusPattern;
using DesignPatterns.ObjectPool;
using DesignPatterns.ServiceLocatorPattern;

namespace Mechanics.Grappling
{
    public class RopeSegmentHolderController : MonoBehaviour
    {
        List<GameObject> _segmentsToDespawn = new();

        GameplayEventBus _gameplayEventBus;
        GameCycleEventBus _gameCycleEventBus;

        ObjectPoolManager _objectPool;

        public void Initilaize()
        {
            FetchDependencies();
            _gameplayEventBus.Subscribe<PlayerEnteredATilemapGameplayEvent>(OnPlayerEnteredATilemap);
            _gameplayEventBus.Subscribe<PlayerPassedATilemapRevolvingTriggerGameplayEvent>(OnPlayerPassedATilemapRevolvingTrigger);
            _gameCycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(ClearAllSegments);
        }

        void FetchDependencies()
        {
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        }

        void OnPlayerEnteredATilemap()
        {
            foreach (Transform segment in transform)
            {
                if (segment.gameObject.activeSelf == false) continue;
                _segmentsToDespawn.Add(segment.gameObject);
            }
        }

        void OnPlayerPassedATilemapRevolvingTrigger()
        {
            foreach (GameObject segment in _segmentsToDespawn)
            {
                _objectPool.ReturnToPool(segment);
            }
            _segmentsToDespawn.Clear();
        }

        void ClearAllSegments()
        {
            foreach (Transform segment in transform)
            {
                if (segment.gameObject.activeSelf)
                    _objectPool.ReturnToPool(segment.gameObject);
            }
            _segmentsToDespawn.Clear();
        }
    }
}
