
using System;

namespace DesignPatterns.StatePattern
{
    public class PlayerSwingingStateMachine : StateMachine
    {
        PlayerController _player;

        internal PlayerSwingingStateMachine(PlayerController player) : base()
        {
            _player = player;
        }

        protected override void SetupStates()
        {
            var idleState = new PlayerSwingingIdleState(this, _player);
            _states.Add(typeof(PlayerSwingingIdleState), idleState);

            TransitionTo(typeof(PlayerSwingingIdleState));
        }
    }
}


