
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;
using UnityEngine;

namespace Player.StateMachines
{
    public class PlayerLifeCycleDeadState : State
    {
        GameplayEventBus _gameplayEventBus;
        public PlayerLifeCycleDeadState(IStateMachine statemachine, GameplayEventBus gameplayEventBus) : base(statemachine)
        {
            _gameplayEventBus = gameplayEventBus;
        }

        public override void OnEnter()
        {
            _gameplayEventBus.Publish<PlayerDiedEvent>();
        }

        protected override void SubscribeToTransitionEvents()
        {
            // Game Restart
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            // Game Restart
        }

        void TransitionToAliveState()
        {
            // Game Restart
        }
    }
}
