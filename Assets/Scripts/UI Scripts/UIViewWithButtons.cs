
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
        public override Tween SlidePanelIn()
        {
            return base.SlidePanelIn()
                .OnComplete(() =>
                {
                    AddButtonListeners();
                    EnableButtonsInteractability();
                });
        }

        public override Tween SlidePanelOut()
        {
            RemoveButtonListeners();
            DisableButtonsInteractability();
            return base.SlidePanelOut();
        }

        public abstract void EnableButtonsInteractability();
        public abstract void DisableButtonsInteractability();

        protected abstract void AddButtonListeners();
        protected abstract void RemoveButtonListeners();
    }
}
