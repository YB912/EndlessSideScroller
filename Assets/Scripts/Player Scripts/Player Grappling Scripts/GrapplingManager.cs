
using DesignPatterns.ServiceLocatorPattern;
using Mechanics.CourseGeneration;
using Player;
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
            ServiceLocator.instance.Register(this);
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
    public class CommonGrapplingDependencies
    {
        [SerializeField] Transform _effectorTransform;

        public Transform effectorTransform => _effectorTransform;
    }

    /// <summary>
    /// Configuration for grappling aim system including IK and movement parameters.
    /// </summary>
    [System.Serializable]
    public class GrapplingAimDependencies
    {
        [SerializeField] IKManager2D _IKManager;
        [SerializeField] Transform _IKTargetTransform;
        [SerializeField] float _aimMovementDuration;
        [SerializeField] float _minimumAimDistance;
        [SerializeField] GameObject _aimVisualizerPrefab;

        public IKManager2D IKManager => _IKManager;
        public Transform IKTargetTransform => _IKTargetTransform;
        public float aimMovementDuration => _aimMovementDuration;
        public float minimumAimDistance => _minimumAimDistance;
        public GameObject aimVisualizerPrefab => _aimVisualizerPrefab;
    }

    /// <summary>
    /// Configuration for rope physics and rendering including segments and constraints.
    /// </summary>
    [System.Serializable]
    public class GrapplingRopeDependencies
    {
        [SerializeField] GameObject _ropeSegmentPrefab;
        [SerializeField] GameObject _ropeSegmentHolderPrefab;
        [SerializeField] int _segmentCountLimit;

        PlayerController _player;

        public GameObject ropeSegmentPrefab => _ropeSegmentPrefab;
        public GameObject ropeSegmentHolderPrefab => _ropeSegmentHolderPrefab;
        public int segmentCountLimit => _segmentCountLimit;
        public PlayerController player
        {
            get
            {
                if (_player == null)
                {
                    InitializePlayerField();
                }
                return _player;
            }
        }

        void InitializePlayerField()
        {
            _player = ServiceLocator.instance.Get<PlayerController>();
        }
    }

    /// <summary>
    /// Animation settings for rope creation and movement effects.
    /// </summary>
    [System.Serializable]
    public class RopeAnimationDependencies
    {
        [SerializeField] AnimationCurve _animationCurve;
        [SerializeField] float _totalAnimationDuration;

        public AnimationCurve animationCurve => _animationCurve;
        public float totalAnimationDuration => _totalAnimationDuration;
    }
}