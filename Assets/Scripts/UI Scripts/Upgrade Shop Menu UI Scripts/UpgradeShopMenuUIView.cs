
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeShop
{
    public class UpgradeShopMenuUIView : FadingUIViewWithButtons, IInitializeable
    {
        [SerializeField] TextMeshProUGUI _upgradeShopTitleText;
        [SerializeField] TextMeshProUGUI _upgradeShopComingSoonText;
        [SerializeField] Button _upgradeShopDeployButton;

        IUpgradeShipMenuUIPresenter _presenter;

        public override void Initialize()
        {
            _presenter = IUpgradeShipMenuUIPresenter.Create(this);
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
