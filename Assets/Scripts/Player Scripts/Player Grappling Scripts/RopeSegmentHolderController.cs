
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

        ObjectPoolManager _objectPool;

        public void Initilaize()
        {
            FetchDependencies();
            _gameplayEventBus.Subscribe<PlayerEnteredATilemapGameplayEvent>(OnPlayerEnteredATilemap);
            _gameplayEventBus.Subscribe<PlayerPassedATilemapRevolvingTriggerGameplayEvent>(OnPlayerPassedATilemapRevolvingTrigger);
        }

        void FetchDependencies()
        {
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
            _objectPool = ServiceLocator.instance.Get<ObjectPoolManager>();
        }

        void OnPlayerEnteredATilemap()
        {
            foreach (Transform segment in transform)
            {
                if (segment.gameObject.activeInHierarchy == false) continue;
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
    }
}
