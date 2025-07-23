
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeShop
{
    public class UpgradeShopMenuUIView : UIViewWithButtons, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _upgradeShopTitleText;
        [SerializeField] TextMeshProUGUI _upgradeShopComingSoonText;
        [SerializeField] Button _upgradeShopDeployButton;

        IUpgradeShipMenuUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IUpgradeShipMenuUIPresenter.Create(this);
        }

        public override void EnableButtonsInteractability()
        {
            _upgradeShopDeployButton.interactable = true;
        }

        public override void DisableButtonsInteractability()
        {
            _upgradeShopDeployButton.interactable = false;
        }

        protected override void AddButtonListeners()
        {
            _upgradeShopDeployButton.onClick.AddListener(_presenter.DeployButtonClicked);
        }

        protected override void RemoveButtonListeners()
        {
            _upgradeShopDeployButton.onClick.RemoveAllListeners();
        }
    }
}
