
using DesignPatterns.EventBusPattern;

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

        public State(IStateMachine statemachine)
        {
            _statemachine = statemachine;
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
