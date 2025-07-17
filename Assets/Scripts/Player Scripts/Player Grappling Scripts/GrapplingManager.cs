using UnityEngine;
using UnityEngine.U2D.IK;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Main controller for the grappling system. Manages aiming, rope creation, and coordinates
    /// between different grappling components.
    /// </summary>
    [RequireComponent(typeof(GrapplingAimController))]
    [RequireComponent(typeof(GrapplingRopeController))]
    [RequireComponent(typeof(RopeCreator))]
    [RequireComponent(typeof(RopeCreationAnimationController))]
    public class GrapplingManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] CommonGrapplingDependencies _commonDependencies;
        [SerializeField] GrapplingAimDependencies _aimDependencies;
        [SerializeField] GrapplingRopeDependencies _ropeDependencies;
        [SerializeField] RopeAnimationDependencies _ropeAnimationDependencies;

        GrapplingAimController _aimController;
        GrapplingRopeController _ropeController;

        public GrapplingAimController aimController => _aimController;
        public GrapplingRopeController ropeController => _ropeController;

        public void Initialize()
        {
            FetchDependencies();
            _aimController.Initialize(_aimDependencies, _commonDependencies);
            _ropeController.Initialize(_ropeDependencies, _ropeAnimationDependencies, _commonDependencies);
        }

        void FetchDependencies()
        {
            _aimController = GetComponent<GrapplingAimController>();
            _ropeController = GetComponent<GrapplingRopeController>();
        }
    }

    /// <summary>
    /// Shared dependencies used across multiple grappling components.
    /// </summary>
    [System.Serializable]
    internal class CommonGrapplingDependencies
    {
        public Transform effectorTransform;
    }

    /// <summary>
    /// Configuration for grappling aim system including IK and movement parameters.
    /// </summary>
    [System.Serializable]
    internal class GrapplingAimDependencies
    {
        public IKManager2D IKManager;
        public Transform IKTargetTransform;
        public float aimMovementDuration;
        public float minimumAimDistance;
    }

    /// <summary>
    /// Configuration for rope physics and rendering including segments and constraints.
    /// </summary>
    [System.Serializable]
    internal class GrapplingRopeDependencies
    {
        public GameObject ropeSegmentPrefab;
        public Rigidbody2D forearmRigidbody;
        public Rigidbody2D abdomenRigidbody;
        public int segmentCountLimit;
    }

    /// <summary>
    /// Animation settings for rope creation and movement effects.
    /// </summary>
    [System.Serializable]
    internal class RopeAnimationDependencies
    {
        public AnimationCurve animationCurve;
        public float totalAnimationDuration;
    }
}