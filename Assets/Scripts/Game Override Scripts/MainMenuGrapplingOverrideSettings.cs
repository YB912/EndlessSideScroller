
using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuGrapplingOverrideSettings" , menuName = "Main Menu Grappling Override Settings")]
public class MainMenuGrapplingOverrideSettings : ScriptableObject
{
    [SerializeField] float _hangingRopeAimHeightNormalized;
    [SerializeField] Vector3 _hangingRopeAimOffsetFromHead;
    [SerializeField] Vector3 _forwardRopeAimOffsetFromHead;
    [SerializeField] float _forwardRopeReleaseDelay;

    public float hangingRopeAimHeightNormalized => _hangingRopeAimHeightNormalized;
    public Vector3 hangingRopeAimOffsetFromHead => _hangingRopeAimOffsetFromHead;
    public Vector3 forwardRopeAimOffsetFromHead => _forwardRopeAimOffsetFromHead;
    public float forwardRopeReleaseDelay => _forwardRopeReleaseDelay;
}
