using UnityEngine;

namespace ActFG.Action {
    /// <summary>
    /// 摄像机射线
    /// </summary>
    public class CameraAction : MonoBehaviour {
        public Camera mainCamera;

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
    }
}