
using Mechanics.GameManagement;
using UnityEngine;

public class GameManager : MonoBehaviour, IInitializeable
{
    GameCycleStatemachine _cycleStateMachine;

    public void Initialize()
    {
        _cycleStateMachine = new GameCycleStatemachine();
    }
}
