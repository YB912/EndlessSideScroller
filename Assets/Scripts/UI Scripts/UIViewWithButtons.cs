
using DG.Tweening;

namespace UI
{
    public interface IUIViewWithButtons : IUIView
    {
        public void EnableButtonsInteractability();
        public void DisableButtonsInteractability();
    }

    public interface IGameOverMenuUIView : IUIViewWithButtons
    {
        public void UpdateTotalScore(int totalScore);
    }

    public abstract class UIViewWithButtons : UIView, IUIViewWithButtons
    {
        public override Tween FadePanelIn()
        {
            return base.FadePanelIn()
                .OnComplete(() =>
                {
                    AddButtonListeners();
                    EnableButtonsInteractability();
                });
        }

        public override Tween FadePanelOut()
        {
            RemoveButtonListeners();
            DisableButtonsInteractability();
            return base.FadePanelOut();
        }

        public abstract void EnableButtonsInteractability();
        public abstract void DisableButtonsInteractability();

        protected abstract void AddButtonListeners();
        protected abstract void RemoveButtonListeners();
    }
}
