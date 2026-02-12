
using UnityEngine;
using DG.Tweening;

namespace UI
{
    public interface IUIView
    {
        public void Initialize();
    }

    public abstract class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected GameObject _rootPanel;

        protected CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            FetchDependencies();
            Hide();
        }

        public abstract void Initialize();

        protected virtual void FetchDependencies()
        {
            _canvasGroup = _rootPanel.GetComponent<CanvasGroup>();
        }

        protected void ActivateRootPanel()
        {
            _rootPanel.SetActive(true);
        }

        protected void DeactivateRootPanel()
        {
            _rootPanel.SetActive(false);
        }

        void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
