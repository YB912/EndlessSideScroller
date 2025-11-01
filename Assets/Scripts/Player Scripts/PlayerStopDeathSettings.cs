
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStopDeathSettings", menuName = "Player Stop Death Settings")]
public class PlayerStopDeathSettings : ScriptableObject
{
    [SerializeField] int _secondsToDie;
    [SerializeField] int _secondsToCountDownVisually;
    [SerializeField] float _minMovementThreshold;
    

    public int secondsToDie => _secondsToDie;
    public int secondsToCountDownVisually => _secondsToCountDownVisually;
    public float minMovementThreshold => _minMovementThreshold;
}
