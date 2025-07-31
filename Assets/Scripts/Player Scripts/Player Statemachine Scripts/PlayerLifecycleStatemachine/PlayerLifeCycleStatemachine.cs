
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DesignPatterns.StatePattern;

namespace Player.StateMachines
{
    public class PlayerLifeCycleStatemachine : Statemachine
    {
        GameplayEventBus _gameplayEventBus;

        public PlayerLifeCycleStatemachine() : base()
        {
            FetchDependencies();
            SetupStates();
            TransitionTo(typeof(PlayerLifeCycleAliveState));
        }

        public override void Reset()
        {
            base.Reset();
            TransitionTo(typeof(PlayerLifeCycleAliveState));
        }

        protected override void SetupStates()
        {
            var aliveState = new PlayerLifeCycleAliveState(this, _gameplayEventBus);
            var deadState = new PlayerLifeCycleDeadState(this, _gameplayEventBus);

            _states.TryAdd(typeof(PlayerLifeCycleAliveState), aliveState);
            _states.TryAdd(typeof(PlayerLifeCycleDeadState), deadState);
        }

        private void FetchDependencies()
        {
            _gameplayEventBus = ServiceLocator.instance.Get<GameplayEventBus>();
        }
    }
}
