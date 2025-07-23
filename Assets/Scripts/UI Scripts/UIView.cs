
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

        protected RectTransform _rootPanelRectTransform;
        protected Canvas _parentCanvas;
        protected Rect _parentCanvasRect;

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
            _parentCanvas = GetComponentInChildren<Canvas>();
            if (_parentCanvas != null)
                _parentCanvasRect = _parentCanvas.GetComponent<RectTransform>().rect;
            else
                _parentCanvasRect = default;
        }

        public virtual Tween SlidePanelIn()
        {
            _currentTween?.Kill();
            ActivateRootPanel();
            SetPositionOffScreen();
            var tweenDestination = GetAnchoredPosition(_slidingWindowSettings.onScreenNormalizedPosition);
            _currentTween = _rootPanelRectTransform.DOAnchorPos(tweenDestination, _slidingWindowSettings.slidingDuration).
                SetEase(_slidingWindowSettings.slidingEase).
                OnComplete(() => SetPositionOnScreen());
            return _currentTween;
        }

        public virtual Tween SlidePanelOut()
        {
            _currentTween?.Kill();
            var tweenDestination = GetAnchoredPosition(_slidingWindowSettings.offScreenNormalizedPosition);
            _currentTween = _rootPanelRectTransform.DOAnchorPos(tweenDestination, _slidingWindowSettings.slidingDuration)
                .SetEase(_slidingWindowSettings.slidingEase)
                .OnComplete(() =>
                {
                    DeactivateRootPanel();
                    SetPositionOnScreen();
                });
            return _currentTween;
        }

        public virtual void SetPositionOnScreen()
        {
            _rootPanelRectTransform.anchoredPosition = GetAnchoredPosition(_slidingWindowSettings.onScreenNormalizedPosition);
        }

        public virtual void SetPositionOffScreen()
        {
            _rootPanelRectTransform.anchoredPosition = GetAnchoredPosition(_slidingWindowSettings.offScreenNormalizedPosition);
        }

        protected void ActivateRootPanel()
        {
            _rootPanel.SetActive(true);
        }

        protected void DeactivateRootPanel()
        {
            _rootPanel.SetActive(false);
        }

        Vector2 GetAnchoredPosition(Vector2 normalized)
        {
            float x = (normalized.x - 0.5f) * _parentCanvasRect.width;
            float y = (normalized.y - 0.5f) * _parentCanvasRect.height;

            return new Vector2(x, y);
        }
    }
}
