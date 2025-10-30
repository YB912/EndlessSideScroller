
using DG.Tweening;
using UnityEngine;

namespace Mechanics.Grappling
{
    public class RopeVisualsController : MonoBehaviour
    {
        [SerializeField] LineRenderer _currentLineRenderer;
        [SerializeField] LineRenderer _fadingLineRenderer;

        Vector3 _currentAttachmentEndPosition;
        Vector3 _fadingRopeAttachmentEndPosition;

        Vector3 _currentLineEndPosition;
        Vector3 _fadingRopeLineEndPosition;

        Transform _playerForearmTransform;

        AnimationCurve _animationCurve;
        float _totalAnimationDuration;

        Tween _creationTween;
        Tween _fadingTween;

        bool _updateCurrentLineRenderer;
        bool _updateFadingLineRenderer;

        private void LateUpdate()
        {
            if (_updateCurrentLineRenderer)
            {
                _currentLineRenderer.positionCount = 2;
                _currentLineRenderer.SetPosition(0, _playerForearmTransform.position);
                _currentLineRenderer.SetPosition(1, _currentLineEndPosition);
            }

            if (_updateFadingLineRenderer)
            {
                _fadingLineRenderer.positionCount = 2;
                _fadingLineRenderer.SetPosition(0, _fadingRopeLineEndPosition);
                _fadingLineRenderer.SetPosition(1, _fadingRopeAttachmentEndPosition);
            }
        }

        public void Initialize(RopeAnimationDependencies fadeInDependencies, Transform playerForearmTransform)
        {
            _playerForearmTransform = playerForearmTransform;
            _animationCurve = fadeInDependencies.animationCurve;
            _totalAnimationDuration = fadeInDependencies.totalAnimationDuration;
        }

        public void AnimateRopeCreation(Vector3 attachmentEndPosition)
        {
            _creationTween?.Kill();
            EnableCurrentRenderer();
            _currentAttachmentEndPosition = attachmentEndPosition;
            _currentLineEndPosition = _playerForearmTransform.position;
            _creationTween = DOTween.To(() => _currentLineEndPosition, x => _currentLineEndPosition = x, _currentAttachmentEndPosition, _totalAnimationDuration)
                .SetEase(_animationCurve);
        }

        public void AnimateRopeFade()
        {
            _creationTween?.Kill();
            _fadingTween?.Kill();
            DisableCurrentRenderer();
            EnableSecondRenderer();
            _fadingRopeAttachmentEndPosition = _currentLineEndPosition;
            _fadingRopeLineEndPosition = _playerForearmTransform.position;
            _fadingTween = DOTween.To(() => _fadingRopeLineEndPosition, x => _fadingRopeLineEndPosition = x, _fadingRopeAttachmentEndPosition, _totalAnimationDuration)
                .SetEase(_animationCurve)
                .OnComplete(DisableSecondRenderer);
        }

        void EnableCurrentRenderer()
        {
            _updateCurrentLineRenderer = true;
            _currentLineRenderer.enabled = true;
        }

        void DisableCurrentRenderer()
        {
            _updateCurrentLineRenderer = false;
            _currentLineRenderer.enabled = false;
        }

        void EnableSecondRenderer()
        {
            _updateFadingLineRenderer = true;
            _fadingLineRenderer.enabled = true;
        }

        void DisableSecondRenderer()
        {
            _updateFadingLineRenderer = false;
            _fadingLineRenderer.enabled = false;
        }
    }
}
