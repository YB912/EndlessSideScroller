
using Mechanics.GameManagement;
using UnityEngine;

public class GameManager : MonoBehaviour, IInitializeable
{
    GameCycleStateMachine _cycleStateMachine;

    public void Initialize()
    {
        _cycleStateMachine = new GameCycleStateMachine();
    }
}
