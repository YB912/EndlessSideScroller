
using UnityEngine;
using DG.Tweening;

namespace UI
{
    public interface IUIView
    {
        public void Initialize();
        public Tween SlidePanelIn();
        public Tween SlidePanelOut();
        public void SetPositionOnScreen();
        public void SetPositionOffScreen();
    }

    public abstract class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected GameObject _rootPanel;
        [SerializeField] SlidingWindowSettings _slidingWindowSettings;

        Vector2 _onScreenAnchorMin;
        Vector2 _onScreenAnchorMax;

        Vector2 _offScreenAnchorMin;
        Vector2 _offScreenAnchorMax;

        protected RectTransform _rootPanelRectTransform;

        Tween _currentTween;

        protected virtual void Awake()
        {
            FetchDependencies();
            SetPositionOffScreen();
        }

        public abstract void Initialize();

        protected virtual void FetchDependencies()
        {
            _rootPanelRectTransform = _rootPanel.GetComponent<RectTransform>();

            _onScreenAnchorMin = _rootPanelRectTransform.anchorMin;
            _onScreenAnchorMax = _rootPanelRectTransform.anchorMax;
            CalculateOffscreenAnchors();
        }

        public virtual Tween SlidePanelIn()
        {
            ActivateRootPanel();
            return SlidePanel(_onScreenAnchorMin, _onScreenAnchorMax);
        }

        public virtual Tween SlidePanelOut()
        {
            return SlidePanel(_offScreenAnchorMin, _offScreenAnchorMax)
                .OnComplete(DeactivateRootPanel);
        }

        public virtual void SetPositionOnScreen()
        {
            _rootPanelRectTransform.anchorMin = _onScreenAnchorMin;
            _rootPanelRectTransform.anchorMax = _onScreenAnchorMax;
            SetPanelRectOffsets();
        }

        public virtual void SetPositionOffScreen()
        {
            _rootPanelRectTransform.anchorMin = _offScreenAnchorMin;
            _rootPanelRectTransform.anchorMax = _offScreenAnchorMax;
            SetPanelRectOffsets();
        }

        protected void ActivateRootPanel()
        {
            _rootPanel.SetActive(true);
        }

        protected void DeactivateRootPanel()
        {
            _rootPanel.SetActive(false);
        }

        private void CalculateOffscreenAnchors()
        {
            _offScreenAnchorMin = new Vector2(_onScreenAnchorMin.x + _slidingWindowSettings.offScreenAnchorOffsetMin.x,
                _onScreenAnchorMin.y + _slidingWindowSettings.offScreenAnchorOffsetMin.y);
            _offScreenAnchorMax = new Vector2(_onScreenAnchorMax.x + _slidingWindowSettings.offScreenAnchorOffsetMax.x,
                _onScreenAnchorMax.y + _slidingWindowSettings.offScreenAnchorOffsetMax.y);
        }

        void SetPanelRectOffsets()
        {
            _rootPanelRectTransform.offsetMin = Vector2.zero;
            _rootPanelRectTransform.offsetMax = Vector2.zero;
        }

        Tween SlidePanel(Vector2 targetAnchorMin, Vector2 targetAnchorMax)
        {
            _currentTween?.Kill();

            var anchorMinTween = _rootPanelRectTransform.DOAnchorMin(targetAnchorMin, _slidingWindowSettings.slidingDuration);
            var anchorMaxTween = _rootPanelRectTransform.DOAnchorMax(targetAnchorMax, _slidingWindowSettings.slidingDuration);

            _currentTween = DOTween.Sequence().Join(anchorMinTween).Join(anchorMaxTween)
                .SetEase(_slidingWindowSettings.slidingEase);
            _currentTween.Play();

            return _currentTween;
        }
    }
}
