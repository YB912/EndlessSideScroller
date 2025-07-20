
using DesignPatterns.EventBusPattern;
using DesignPatterns.StatePattern;
using Mechanics.GameManagement;

public class GameCycleUpgradeShopState : State
{
    UIEventBus _UIEventBus;
    GameCycleEventBus _gameCycleEventBus;

    public GameCycleUpgradeShopState(IStateMachine statemachine, UIEventBus UIEventBus, GameCycleEventBus gameCycleEventBus) : base(statemachine)
    {
        _UIEventBus = UIEventBus;
        _gameCycleEventBus = gameCycleEventBus;
    }

    protected override void SubscribeToTransitionEvents()
    {
        _UIEventBus.Subscribe<PlayStateButtonClickedUIEvent>(TransitionToPlayState);
        _UIEventBus.Subscribe<MainMenuStateButtonClickedUIEvent>(TransitionToMainMenuState);
    }

    protected override void UnsubscribeFromTransitionEvents()
    {
        _UIEventBus.Unsubscribe<PlayStateButtonClickedUIEvent>(TransitionToPlayState);
        _UIEventBus.Unsubscribe<MainMenuStateButtonClickedUIEvent>(TransitionToMainMenuState);
    }

    void TransitionToPlayState()
    {
        _statemachine.TransitionTo(typeof(GameCyclePlayState));
    }

    void TransitionToMainMenuState()
    {
        _statemachine.TransitionTo(typeof(GameCycleMainMenuState));
    }
}
