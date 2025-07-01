
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

        /// <summary>
        /// Called when entering this state. Subscribes to transition events.
        /// </summary>
        public virtual void OnEnter()
        {
            SubscribeToTransitionEvents();
        }

        /// <summary>
        /// Called when exiting this state. Unsubscribes from transition events.
        /// </summary>
        public virtual void OnExit()
        {
            UnsubscribeFromTransitionEvents();
        }

        /// <summary>
        /// Called every update cycle while this state is active.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Subclasses implement to subscribe to relevant transition events.
        /// </summary>
        protected abstract void SubscribeToTransitionEvents();

        /// <summary>
        /// Subclasses implement to unsubscribe from transition events.
        /// </summary>
        protected abstract void UnsubscribeFromTransitionEvents();
    }
}
