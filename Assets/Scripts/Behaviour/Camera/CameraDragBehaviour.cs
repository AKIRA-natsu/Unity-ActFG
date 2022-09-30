using UnityEngine;

/// <summary>
/// 摄像机拖拽表现
/// </summary>
public class CameraDragBehaviour : CameraBehaviour {
    private IDrag dragObject;
    /// <summary>
    /// 当前拖拽物体
    /// </summary>
    public IDrag CurrentDrag => dragObject;

    public override void GameUpdate() {
        if (Input.GetMouseButtonUp(0) && dragObject != null) {
            dragObject.OnDragUp();
            dragObject = null;
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = CameraExtend.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, System.Single.MaxValue)) {
                if (hit.transform.TryGetComponent<IDrag>(out dragObject))
                    dragObject.OnDragDown();
            }
        }

        if (dragObject != null)
            dragObject.OnDrag();
    }
}