
using DG.Tweening;

namespace UI
{
    public interface IUIViewWithButtons : IUIView
    {
        public void EnableButtonsInteractability();
        public void DisableButtonsInteractability();
    }
    public abstract class UIViewWithButtons : UIView, IUIViewWithButtons
    {
        public override Tween SlidePanelIn()
        {
            AddButtonListeners();
            return base.SlidePanelIn();
        }

        public override Tween SlidePanelOut()
        {
            RemoveButtonListeners();
            return base.SlidePanelOut();
        }

        public abstract void EnableButtonsInteractability();
        public abstract void DisableButtonsInteractability();

        protected abstract void AddButtonListeners();
        protected abstract void RemoveButtonListeners();
    }
}
