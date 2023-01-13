using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AKIRA.UIFramework {
    public class SampleTransitionPanelProp : UIComponent {
        [UIControl("TransitionImg")]
        protected Image TransitionImg;
        [UIControl("TransitionImg")]
        protected RectTransform TransitionRect;
    }
}