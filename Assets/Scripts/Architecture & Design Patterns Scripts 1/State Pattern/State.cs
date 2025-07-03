
namespace DesignPatterns.StatePattern
{
    /// <summary>
    /// Represents a state in a state machine.
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void Update();
        void OnExit();
    }

    /// <summary>
    /// Base implementation of <see cref="IState"/> providing transition event subscription handling.
    /// </summary>
    public abstract class State : IState
    {
        protected IStateMachine _statemachine;

        protected State(IStateMachine statemachine)
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
