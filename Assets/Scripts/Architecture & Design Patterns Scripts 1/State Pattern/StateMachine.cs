
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
        void Reset();
    }

    /// <summary>
    /// Base abstract class implementing a generic state machine pattern.
    /// Manages state transitions, pausing, and update forwarding.
    /// </summary>
    public abstract class Statemachine : IStateMachine
    {
        protected Dictionary<Type, IState> _states = new();
        protected IState _currentState;

        protected bool _isPaused;
        protected Action _postponedOnEnterAction;
        public bool isPaused => _isPaused;

        protected Statemachine()
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
            _postponedOnEnterAction?.Invoke();
            _postponedOnEnterAction = null;
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
            if (_isPaused)
            {
                _postponedOnEnterAction = _currentState.OnEnter;
                return;
            }
            _currentState?.OnEnter();
        }

        public virtual void Reset()
        {
            Pause();
        }

        /// <summary>
        /// Must be overridden to initialize the state dictionary. Recommended to define this but call TrySetupStates() instead.
        /// </summary>
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
