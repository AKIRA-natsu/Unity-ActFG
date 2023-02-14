using DG.Tweening;
using UnityEngine;

namespace AKIRA.UIFramework {
    /// <summary>
    /// 升级按钮
    /// </summary>
    public class UpgradeButtonComponent : UpgradeButtonComponentProp {
        // 上一次金额
        private int lastCostValue;
        // 动画
        private Tween lerpTween;

        public override void Awake(object obj) {
            base.Awake(obj);
            this.UpGradeButton.onClick.AddListener(() => {
                var upgrade = UpgradeManager.Instance.GetUpgrade(BindComponent.type);
                upgrade.Upgrade();
                UpdateText(upgrade);
            });
            SetButtonData(UpgradeManager.Instance.GetUpgrade(BindComponent.type));
            MoneyManager.Instance.RegistOnCurrencyChangeAction(Clickable);
        }

        /// <summary>
        /// 设置按钮数据
        /// </summary>
        /// <param name="upgradeBase"></param>
        private void SetButtonData(UpgradeBase upgradeBase) {
            this.Name.text = upgradeBase.ToString();
            // this.UpgradeBg.sprite = BindComponent.ButtonImage;
            this.TopBg.sprite = BindComponent.ButtonImage;
            this.TopBg.SetNativeSize();
            UpdateText(upgradeBase);
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="upgradeBase"></param>
        private void UpdateText(UpgradeBase upgradeBase) {
            this.Level.text = $"Level {upgradeBase.level + 1}";
            if (upgradeBase.ReachMaxLevel) {
                this.UpGradeButton.interactable = false;
                this.Cost.text = "MAX";
            } else {
                lerpTween?.Kill(true);
                lerpTween = DOTween.To(() => lastCostValue, lerpValue => {
                    lastCostValue = lerpValue;
                    this.Cost.text = lerpValue.ToString();
                }, upgradeBase.cost, 0.3f);
            }
        }

        /// <summary>
        /// 按钮检查点击
        /// </summary>
        /// <param name="value"></param>
        private void Clickable(int value) {
            var upgradeBase = UpgradeManager.Instance.GetUpgrade(BindComponent.type);
            this.UpGradeButton.interactable = MoneyManager.Instance.Currency >= upgradeBase.cost && !upgradeBase.ReachMaxLevel;
        }
    }
}