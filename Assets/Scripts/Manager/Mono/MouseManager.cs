using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 手指图标跟随（可能素材用）
    /// </summary>
    public class MouseManager : MonoSingleton<MouseManager> {
        // 是否开启手跟踪
        public bool enable;
        // 手图片
        public RectTransform hand;
        
        private void Start() {
            #if UNITY_EDITOR
                enable = true;
            #endif

            hand.gameObject.SetActive(enable);
        }

        private void Update() {
            if (enable)
                HandFollow();
        }

        /// <summary>
        /// 手指图片跟随
        /// </summary>
        private void HandFollow() {
            if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
             && Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
                hand.localPosition = Input.mousePosition.ScreenToUGUI();
        }
    }
}