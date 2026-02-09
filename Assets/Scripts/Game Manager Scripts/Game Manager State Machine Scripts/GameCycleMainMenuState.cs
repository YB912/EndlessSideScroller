
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;

namespace Mechanics.GameManagement
{
    public class GameCycleMainMenuState : State
    {
        UIEventBus _UIEventBus;
        GameCycleEventBus _gameCycleEventBus;

        public GameCycleMainMenuState(IStateMachine statemachine, UIEventBus UIEventBus, GameCycleEventBus gameCycleEventBus) : base(statemachine)
        {
            _UIEventBus = UIEventBus;
            _gameCycleEventBus = gameCycleEventBus;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _gameCycleEventBus.Publish<EnteredMainMenuStateGameCycleEvent>();
        }

        protected override void SubscribeToTransitionEvents()
        {
            _UIEventBus.Subscribe<PlayStateButtonClickedUIEvent>(TransitionToPlayState);
        }

        protected override void UnsubscribeFromTransitionEvents()
        {
            _UIEventBus.Unsubscribe<PlayStateButtonClickedUIEvent>(TransitionToPlayState);
        }

        void TransitionToPlayState()
        {
            _statemachine.TransitionTo(typeof(GameCyclePlayState));
        }
    }
}
