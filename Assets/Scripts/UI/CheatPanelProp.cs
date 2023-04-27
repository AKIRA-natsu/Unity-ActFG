using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AKIRA.UIFramework {
    public class CheatPanelProp : UIComponent {
        [UIControl("Mask")]
        protected RectTransform View;
        [UIControl("Mask")]
        protected Mask Mask;
        [UIControl("Mask/ScrollView/Viewport/Content")]
        protected ScrollListComponent Content;
        [UIControl("Mask/ScrollView/Scrollbar Vertical")]
        protected Scrollbar Scrollbar;
        [UIControl("CloseBtn/HideText")]
        protected TextMeshProUGUI HideText;
        [UIControl("CloseBtn")]
        protected Button CloseBtn;
    }
}