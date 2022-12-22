using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AKIRA.UIFramework {
    public class BezierPanelProp : UIComponent {
        [UIControl("TempRect")]
        protected RectTransform TempRect;
        [UIControl("BezierControlRect")]
        protected RectTransform BezierControlRect;
    }
}