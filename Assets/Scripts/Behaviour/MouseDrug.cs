using UnityEngine;
using ActFG.Util.Tools;

namespace ActFG.Behaviour {
    /// <summary>
    /// 拖拽 xy轴
    /// </summary>
    public class MouseDrug : MonoBehaviour {
        private Vector3 ScreenPoint;
        private Vector3 Offset;

        private void Update() {
            OnMouseDown();
            OnMouseDrag();
        }

        private void OnMouseDown() {
            ScreenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
            Offset = this.transform.position - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z)
            );
        }

        private void OnMouseDrag() {
            var currentPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
            if (currentPos.x <= 0 || currentPos.x >= Screen.width
                || currentPos.y <= 0 || currentPos.y >= Screen.height)
            {
                Debug.Log("超出屏幕边界");
                return;
            }
            this.transform.position = Camera.main.ScreenToWorldPoint(currentPos) + Offset;
            Debug.Log("移动位置到".StringColor(Color.red) + this.transform.position);
        }
    }
}