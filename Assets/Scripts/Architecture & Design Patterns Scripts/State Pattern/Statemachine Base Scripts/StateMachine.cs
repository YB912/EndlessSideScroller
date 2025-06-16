
using System;
using System.Collections.Generic;

namespace DesignPatterns.StatePattern
{
    public interface IStateMachine
    {
        public bool isPaused { get; }
        public void Pause();
        public void Resume();
        public void Update();
        public void TransitionTo(Type type);
    }

    public abstract class StateMachine : IStateMachine
    {
        protected Dictionary<Type, IState> _states = new();
        protected IState _currentState;

        protected bool _isPaused;

        public bool isPaused => _isPaused;

        public StateMachine()
        {
            Pause();
            SetupStates();
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