using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 摄像机范围点击表现
/// </summary>
public class CameraClickBehaviour : CameraBehaviour {
    /// <summary>
    /// 点击半径
    /// </summary>
    [SerializeField]
    private float radius;
    /// <summary>
    /// 是否只点击一次
    /// </summary>
    [SerializeField]
    private bool callOnce = true;

    /// <summary>
    /// 是否已经按下
    /// </summary>
    private bool called = false;

    public override void GameUpdate() {
        if (Mouse.current.leftButton.isPressed) {
            if (callOnce && called)
                return;
            if (callOnce && !called)
                called = true;
            Ray ray = CameraExtend.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, System.Single.MaxValue)) {
                if (hit.transform.TryGetComponent<IClick>(out IClick clickObject)) {
                    var colliders = Physics.OverlapSphere(hit.transform.position, radius);
                    foreach (var collider in colliders) {
                        if (!collider.transform.TryGetComponent<IClick>(out IClick click))
                            continue;
                        click.OnClick();
                    }
                }
            }
        }

        if (callOnce && !Mouse.current.leftButton.isPressed)
            called = false;
    }
}