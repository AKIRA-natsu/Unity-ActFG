using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AKIRA.UIFramework {
    public class UpgradeButtonComponentProp : UIComponentProp {
        [UIControl("")]
        protected Button UpGradeButton;
        [UIControl("")]
        protected Image UpgradeBg;
        [UIControl("TopBg")]
        protected Image TopBg;
        [UIControl("Name")]
        protected TextMeshProUGUI Name;
        [UIControl("Level")]
        protected TextMeshProUGUI Level;
        [UIControl("Cost/Cost")]
        protected TextMeshProUGUI Cost;

        [UIControl("")]
        protected UpgradeBindComponent BindComponent;
    }
}