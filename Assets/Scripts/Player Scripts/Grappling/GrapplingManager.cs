
using UnityEngine;
using UnityEngine.U2D.IK;

namespace Mechanics.Grappling
{
    [RequireComponent(typeof(GrapplingAimController))]
    [RequireComponent(typeof(GrapplingRopesManager))]
    [RequireComponent(typeof(RopeCreator))]
    public class GrapplingManager : MonoBehaviour, IInitializeable
    {
        [SerializeField] CommonGrapplingDependencies _commonDependencies;
        [SerializeField] GrapplingAimDependencies _aimDependencies;
        [SerializeField] GrapplingRopeDependencies _ropeDependencies;

        GrapplingAimController _aimController;
        GrapplingRopesManager _ropeController;

        public GrapplingAimController aimController => _aimController;
        public GrapplingRopesManager ropeController => _ropeController;

        public void Initialize()
        {
            FetchDependencies();
            _aimController.Initialize(_aimDependencies, _commonDependencies);
            _ropeController.Initialize(_ropeDependencies, _commonDependencies);
        }

        void FetchDependencies()
        {
            _aimController = GetComponent<GrapplingAimController>();
            _ropeController = GetComponent<GrapplingRopesManager>();
        }
    }

    [System.Serializable]
    internal class CommonGrapplingDependencies
    {
        public Transform effectorTransform;
    }

    [System.Serializable]
    internal class GrapplingAimDependencies
    {
        public IKManager2D IKManager;
        public Transform IKTargetTransform;
        public float aimMovementDuration;
        public float minimumAimDistance;
    }

    [System.Serializable]
    internal class GrapplingRopeDependencies
    {
        public GameObject ropeSegmentPrefab;
        public Rigidbody2D forearmRigidbody;
        public int segmentCountLimit;
    }
}
