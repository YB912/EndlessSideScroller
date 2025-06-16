
using System;

namespace DesignPatterns.StatePattern
{
    public interface IState
    {
        public void OnEnter();
        public void Update();
        public void OnExit();
    }

    public abstract class State : IState
    {
        protected IStateMachine _statemachine;
        protected PlayerController _player;

        public State(IStateMachine statemachine, PlayerController player)
        {
            _statemachine = statemachine;
            _player = player;
            SubscribeToTransitionEvents();
        }

        public virtual void OnEnter()
        {
            SubscribeToTransitionEvents();
        }

        public virtual void OnExit()
        {
            UnsubscribeFromTransitionEvents();
        }

        public virtual void Update() { }

        protected abstract void SubscribeToTransitionEvents();

        protected abstract void UnsubscribeFromTransitionEvents();
    }
}
