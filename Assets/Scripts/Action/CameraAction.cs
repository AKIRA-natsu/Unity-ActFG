using UnityEngine;
using UnityEngine.EventSystems;

namespace ActFG.Action {
    /// <summary>
    /// 摄像机射线
    /// </summary>
    public class CameraAction : MonoBehaviour {
        // 主摄像机
        public Camera mainCamera;
        // 主Canvas
        public RectTransform canvasRectTransform;

        private void CameraRay() {
            if (mainCamera == null)
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isCollider = Physics.Raycast(ray, out hit);
            if (isCollider) {
                // Debug.Log(hit.collider);
            }
        }

        /// <summary>
        /// 射线 判断UI
        /// </summary>
        private void UIRay() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                //  点击UI
            }
        }

        /// <summary>
        /// 世界坐标转UGUI坐标
        /// </summary>
        private Vector2 WorldToUGUI(Vector3 position) {
            Vector2 ScreenPoint = mainCamera.WorldToScreenPoint(position);
            Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
            ScreenPoint -= ScreenSize/2;//将屏幕坐标变换为以屏幕中心为原点
            Vector2 anchorPos = ScreenPoint / ScreenSize * canvasRectTransform.sizeDelta;//缩放得到UGUI坐标
            return anchorPos;
        }
    }
}