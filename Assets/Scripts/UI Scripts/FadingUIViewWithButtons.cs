
using DG.Tweening;

namespace UI
{
    public interface IFadingUIViewWithButtons : IFadingUIView
    {
        public void EnableButtons();
        public void DisableButtons();
    }

    public abstract class FadingUIViewWithButtons : FadingUIView, IFadingUIViewWithButtons
    {
        public override Tween FadePanelIn()
        {
            return base.FadePanelIn()
                .OnComplete(() =>
                {
                    AddButtonListeners();
                    EnableButtons();
                });
        }

        public override Tween FadePanelOut()
        {
            RemoveButtonListeners();
            DisableButtons();
            return base.FadePanelOut();
        }

        public void EnableButtons()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void DisableButtons()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        protected abstract void AddButtonListeners();
        protected abstract void RemoveButtonListeners();
    }
}
