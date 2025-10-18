using DesignPatterns.ServiceLocatorPattern;
using InputManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayInput
{
    public interface IInputUIView : IBlinkingFadingUIViewWithButtons
    {
        public AimingZoneSettings aimingZoneSettings { get; }
    }

    public class InputUIView : BlinkingFadingUIViewWithButtons, IInputUIView, IInitializeable
    {
        [SerializeField] AimingZoneSettings _aimingZoneSettings;
        [SerializeField] Button _aimingZoneButton;
        [SerializeField] Button _backgroundButton;

        IInputUIPresenter _presenter;

        PointerDownOrUpListener _aimingZoneListener;
        PointerDownOrUpListener _backgroundListener;

        public AimingZoneSettings aimingZoneSettings => _aimingZoneSettings;

        public override void Initialize()
        {
            base.Initialize();
            SetupZone();
            _presenter = IInputUIPresenter.Create(this);
            ServiceLocator.instance.Register(_presenter as ITouchPositionProvider);
        }

        protected override void AddButtonListeners()
        {
            _aimingZoneListener = _aimingZoneButton.gameObject.AddComponent<PointerDownOrUpListener>();
            _backgroundListener = _backgroundButton.gameObject.AddComponent<PointerDownOrUpListener>();

            _aimingZoneListener.onPressed += OnAimingZoneButtonPressed;
            _aimingZoneListener.onReleased += OnAimingZoneButtonReleased;
            _backgroundListener.onPressed += OnBackgroundButtonPressed;
        }

        protected override void RemoveButtonListeners()
        {
            if (_aimingZoneListener != null)
                _aimingZoneListener.onPressed -= OnAimingZoneButtonPressed;

            if (_backgroundListener != null)
                _backgroundListener.onPressed -= OnBackgroundButtonPressed;
        }

        void OnAimingZoneButtonPressed()
        {
            _presenter?.StartAimingTouch();
        }

        void OnAimingZoneButtonReleased()
        {
            _presenter?.EndAimingTouch();
        }

        void OnBackgroundButtonPressed()
        {
            _presenter?.BackgroundTouch();
        }

        void SetupZone()
        {
            var aimingZoneButtonRectTransform = _aimingZoneButton.GetComponent<RectTransform>();

            aimingZoneButtonRectTransform.anchorMin = new Vector2(_aimingZoneSettings.aimingZoneAnchorMinNormalized, 0);
            aimingZoneButtonRectTransform.anchorMax = new Vector2(_aimingZoneSettings.aimingZoneAnchorMaxNormalized, 1);
        }
    }
}
