
using System;
using System.Collections.Generic;

namespace DesignPatterns.StatePattern
{
    /// <summary>
    /// Interface defining the core functionality of a state machine.
    /// </summary>
    public interface IStateMachine
    {
        bool isPaused { get; }
        void Pause();
        void Resume();
        void Update();
        void TransitionTo(Type type);
    }

    /// <summary>
    /// Base abstract class implementing a generic state machine pattern.
    /// Manages state transitions, pausing, and update forwarding.
    /// </summary>
    public abstract class StateMachine : IStateMachine
    {
        protected Dictionary<Type, IState> _states = new();
        protected IState _currentState;

        protected bool _isPaused;
        public bool isPaused => _isPaused;

        protected StateMachine()
        {
            Pause();
        }

        public virtual void Pause()
        {
            _isPaused = true;
        }

        public virtual void Resume()
        {
            _isPaused = false;
        }

        public virtual void Update()
        {
            if (_isPaused) return;
            _currentState?.Update();
        }

        public virtual void TransitionTo(Type type)
        {
            var state = GetStateOfType(type);
            _currentState?.OnExit();
            _currentState = state;
            _currentState?.OnEnter();
        }

        // Must be overridden to initialize the state dictionary.
        protected abstract void SetupStates();

        IState GetStateOfType(Type type)
        {
            if (_states.TryGetValue(type, out IState output))
            {
                return output;
            }
            throw new ArgumentException($"State of type {type.FullName} wasn't found.");
        }
    }
}
