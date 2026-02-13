
using DesignPatterns.EventBusPattern;
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using System.Collections;
using UI.GameplayInputAndHUD;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.IK;

namespace Mechanics.Grappling
{
    /// <summary>
    /// Controls the aiming mechanics for grappling,
    /// managing IK targets, tweening aim movement, and event notifications.
    /// </summary>
    public class GrapplingAimController : MonoBehaviour
    {
        IKManager2D _IKManager;
        Transform _IKTargetTransform;
        Transform _effectorTransform;

        float _aimMovementDuration;
        float _minimumAimDistance;

        ITouchPositionProvider _touchPositionProvider;
        GrapplingEventBus _grapplingEventBus;
        GameCycleEventBus _gameCycleEventBus;
        HingeJoint2D _forearmJoint;

        Vector3 _currentTouchPosition;
        Vector3 _currentAimedTilePosition;
        Vector3 _aimMissedPosition = new Vector3(-1, -1, -1);
        Tween _currentTween;
        float _forearmJointLimitRange;

        AimVisualizer _visualizer;

        int _wallLayerInBitMap;

        const string WALL_LAYER_NAME = "Wall";

        public Vector3 currentAimedTilePosition => _currentAimedTilePosition;

        public void Initialize(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            FetchDependencies(aimDependencies, commonDependencies);
            _gameCycleEventBus.Subscribe<EnteredPlayStateGameCycleEvent>(SubscribeToTouchPositionInput);
            _gameCycleEventBus.Subscribe<ExitedPlayStateGameCycleEvent>(UnsubscribeFromTouchPositionInput);
            _forearmJointLimitRange = _forearmJoint.limits.max - _forearmJoint.limits.min;
            _visualizer = Instantiate(aimDependencies.aimVisualizerPrefab).GetComponent<AimVisualizer>().Initialize();
            _wallLayerInBitMap = Utility.LayerNameToBitMap(WALL_LAYER_NAME);
            DisableIK();
        }

        public void StartAiming()
        {
            StartCoroutine(WaitForTouchPositionAndAimCoroutine());
        }

        public void AimTowards(Vector3 position)
        {
            EnableIK();
            SetCurrentAimedTilePosition(position);
            _currentTween = _IKTargetTransform.DOMove(position, _aimMovementDuration).OnComplete(() => OnAimingFinished(false));
        }

        public void AimTowardsWithDelay(Vector3 position, float delay)
        {
            StartCoroutine(AimTowardsWithDelayCoroutine(position, delay));
        }

        void FetchDependencies(GrapplingAimDependencies aimDependencies, CommonGrapplingDependencies commonDependencies)
        {
            _IKManager = aimDependencies.IKManager;
            _IKTargetTransform = aimDependencies.IKTargetTransform;
            _aimMovementDuration = aimDependencies.aimMovementDuration;
            _minimumAimDistance = aimDependencies.minimumAimDistance;
            _effectorTransform = commonDependencies.effectorTransform;
            _touchPositionProvider = ServiceLocator.instance.Get<ITouchPositionProvider>();
            _grapplingEventBus = ServiceLocator.instance.Get<GrapplingEventBus>();
            _gameCycleEventBus = ServiceLocator.instance.Get<GameCycleEventBus>();
            _forearmJoint = transform.GetChild(0).GetComponent<HingeJoint2D>();
        }

        void SubscribeToTouchPositionInput()
        {
            _touchPositionProvider.currentTouchPositionInWorldObservable.AddListener(OnTouchPositionChanged);
        }

        void UnsubscribeFromTouchPositionInput()
        {
            _touchPositionProvider.currentTouchPositionInWorldObservable.RemoveListener(OnTouchPositionChanged);
        }

        IEnumerator WaitForTouchPositionAndAimCoroutine()
        {
            yield return null; // Wait one frame to ensure input updates
            Aim();
        }

        IEnumerator AimTowardsWithDelayCoroutine(Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);
            AimTowards(position);
        }

        void OnTouchPositionChanged(Vector3 newPosition)
        {
            _currentTouchPosition = new Vector3(newPosition.x, newPosition.y, 0);
            SetCurrentAimedTilePosition(_currentTouchPosition);
        }

        void SetCurrentAimedTilePosition(Vector3 raycastOrigin)
        {
            var raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.up, Mathf.Infinity, _wallLayerInBitMap);
            if (raycastHit)
            {
                var tilemap = raycastHit.collider.GetComponent<Tilemap>();
                _currentAimedTilePosition = tilemap.GetCellCenterWorld(tilemap.WorldToCell(raycastHit.point));
            }
            else
            {
                _currentAimedTilePosition = _aimMissedPosition;
            }
        }

        void Aim()
        {
            if (IsAimPositionFarEnough() && AimingMissed() == false)
            {
                EnableIK();
                _currentTween = _IKTargetTransform.DOMove(_currentAimedTilePosition, _aimMovementDuration).OnComplete(() => OnAimingFinished());
            }
        }

        void OnAimingFinished(bool visualize = true)
        {
            _grapplingEventBus.Publish<GrapplerAimedGrapplingEvent>();
            if (visualize) _visualizer.VisualizeAim(_currentTouchPosition, _currentAimedTilePosition);
            EndAiming();
        }

        void EndAiming()
        {
            DisableIK();
        }

        void EnableIK()
        {
            CancelCurrentTween();
            _IKTargetTransform.position = _effectorTransform.position;
            _IKManager.enabled = true;
        }

        void DisableIK()
        {
            CancelCurrentTween();
            _IKManager.enabled = false;
            SyncBoneAngleLimitsWithIK();
        }

        void SyncBoneAngleLimitsWithIK()
        {
            float currentAngle = _forearmJoint.jointAngle;

            JointAngleLimits2D newLimits = new JointAngleLimits2D
            {
                min = currentAngle - _forearmJointLimitRange,
                max = currentAngle
            };

            _forearmJoint.limits = newLimits;
        }

        void CancelCurrentTween()
        {
            _currentTween?.Kill();
            _currentTween = null;
        }

        bool IsAimPositionFarEnough()
        {
            return Vector3.Distance(transform.position, _currentAimedTilePosition) > _minimumAimDistance;
        }

        bool AimingMissed()
        {
            return _currentAimedTilePosition == _aimMissedPosition;
        }
    }
}
