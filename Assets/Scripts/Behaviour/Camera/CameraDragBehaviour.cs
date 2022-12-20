using UnityEngine;
using UnityEngine.InputSystem;

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
        if (!Mouse.current.leftButton.isPressed && dragObject != null) {
            dragObject.OnDragUp();
            dragObject = null;
        }

        if (Mouse.current.leftButton.isPressed && dragObject == null) {
            Ray ray = CameraExtend.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, System.Single.MaxValue)) {
                if (hit.transform.TryGetComponent<IDrag>(out dragObject))
                    dragObject.OnDragDown();
            }
        }

        if (dragObject != null)
            dragObject.OnDrag();
    }
}