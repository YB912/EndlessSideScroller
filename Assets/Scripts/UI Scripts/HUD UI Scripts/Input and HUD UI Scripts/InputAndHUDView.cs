
using DesignPatterns.ServiceLocatorPattern;
using DG.Tweening;
using InputManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayInputAndHUD
{
    public interface IInputAndHUDView : IBlinkingFadingUIViewWithButtons
    {
        public AimingZoneSettings aimingZoneSettings { get; }

        public void DisplayStopDeathCountdownNumber(int number);
        public void UpdateHealthTint(float healthNormalized);
    }

    public class InputAndHUDView : BlinkingFadingUIViewWithButtons, IInputAndHUDView, IInitializeable
    {
        [Header("Aiming Zone")]
        [SerializeField] AimingZoneSettings _aimingZoneSettings;
        [SerializeField] Button _aimingZoneButton;
        [SerializeField] Button _backgroundButton;

        [Header("Stop Death Countdown")]
        [SerializeField] TMP_Text _stopDeathCountdownText;
        [SerializeField] float _countdownDisplayAnimationDuration;

        [Header("Health Tint")]
        [SerializeField] Image _healthTintImage;
        [SerializeField] float _healthTintTweenDuration;

        IInputAndHUDUIPresenter _presenter;

        PointerDownOrUpListener _aimingZoneListener;
        PointerDownOrUpListener _backgroundListener;

        public AimingZoneSettings aimingZoneSettings => _aimingZoneSettings;

        public override void Initialize()
        {
            base.Initialize();
            _canvas.worldCamera = Camera.main;
            SetupZone();
            _presenter = IInputAndHUDUIPresenter.Create(this);
            ServiceLocator.instance.Register(_presenter as ITouchPositionProvider);
            DisableCountdownText();
        }

        public void DisplayStopDeathCountdownNumber(int number)
        {
            _stopDeathCountdownText.text = number.ToString();
            _stopDeathCountdownText.enabled = true;
            _stopDeathCountdownText.alpha = 0f;
            _stopDeathCountdownText.rectTransform.localScale = Vector3.zero;
            _stopDeathCountdownText.DOFade(1, _countdownDisplayAnimationDuration).SetEase(Ease.InOutQuad);
            _stopDeathCountdownText.rectTransform.DOScale(Vector3.one, _countdownDisplayAnimationDuration).SetEase(Ease.InOutQuad)
                .OnComplete(() => {
                    _stopDeathCountdownText.DOFade(0, _countdownDisplayAnimationDuration).SetEase(Ease.InOutQuad);
                    _stopDeathCountdownText.rectTransform.DOScale(Vector3.zero, _countdownDisplayAnimationDuration).SetEase(Ease.InOutQuad)
                    .OnComplete(DisableCountdownText);
                });
        }

        public void UpdateHealthTint(float healthNormalized)
        {
            var endValue = (1 - healthNormalized);
            _healthTintImage.DOFade(endValue, _healthTintTweenDuration);
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

        void DisableCountdownText()
        {
            _stopDeathCountdownText.alpha = 0;
            _stopDeathCountdownText.rectTransform.localScale = Vector3.zero;
            _stopDeathCountdownText.enabled = false;
        }
    }
}
