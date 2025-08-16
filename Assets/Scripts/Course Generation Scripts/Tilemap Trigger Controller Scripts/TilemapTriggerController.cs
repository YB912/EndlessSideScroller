
using DesignPatterns.EventBusPattern;
using UnityEngine;

namespace Mechanics.CourseGeneration
{
    public abstract class TilemapTriggerController : MonoBehaviour
    {
        protected GameplayEventBus _gamePlayEventBus;
        protected TilemapParameters _tilemapParameters;

        protected BoxCollider2D _trigger;

        protected abstract string nameInHierarchy { get; }
        const string PLAYER_ABDOMEN_TAG = "PlayerAbdomen";

        public virtual void Initialize(GameplayEventBus gameplayEventBus, TilemapParameters tilemapParameters)
        {
            _gamePlayEventBus = gameplayEventBus;
            _tilemapParameters = tilemapParameters;
            _trigger = GetComponent<BoxCollider2D>();
            SetName();
            SetupTrigger();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PLAYER_ABDOMEN_TAG))
            {
                OnPlayerEnteredTrigger();
            }
        }

        private void SetName()
        {
            name = nameInHierarchy;
        }
        protected abstract void OnPlayerEnteredTrigger();

        protected abstract void SetupTrigger();
    }
}
