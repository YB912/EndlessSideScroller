
using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuGrapplingOverrideSettings" , menuName = "Main Menu Grappling Override Settings")]
public class MainMenuGrapplingOverrideSettings : ScriptableObject
{
    [SerializeField] float _hangingRopeAimDelay;
    [SerializeField] Vector3 _hangingRopeAimOffsetFromHead;
    [SerializeField] Vector3 _forwardRopeAimOffsetFromHead;
    [SerializeField] float _forwardRopeReleaseDelay;

    public float hangingRopeAimDelay => _hangingRopeAimDelay;
    public Vector3 hangingRopeAimOffsetFromHead => _hangingRopeAimOffsetFromHead;
    public Vector3 forwardRopeAimOffsetFromHead => _forwardRopeAimOffsetFromHead;
    public float forwardRopeReleaseDelay => _forwardRopeReleaseDelay;
}
