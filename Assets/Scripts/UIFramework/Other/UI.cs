using UnityEngine;
using UnityEngine.UI;

namespace AKIRA.UIFramework {
    public static class UI {
        // 根节点
        public static GameObject Root { get; private set; }
        // Canvas
        public static Canvas Canvas { get; private set; }
        // UI Transform
        public static RectTransform Rect { get; private set; }

        private static Camera uiCamera;
        /// <summary>
        /// UI摄像机
        /// </summary>
        /// <value></value>
        public static Camera UICamera {
            get {
                if (uiCamera == null)
                    uiCamera = Root.transform.Find($"UICamera").GetComponent<Camera>();
                return uiCamera;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="root">根节点</param>
        public static void Initialize(GameObject root) {
            Root = root;
            Canvas = Root.GetComponent<Canvas>();
            Rect = Root.GetComponent<RectTransform>();
        }
    }
}