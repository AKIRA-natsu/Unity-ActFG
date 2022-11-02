using UnityEngine;

/// <summary>
/// 摄像机范围点击表现
/// </summary>
public class CameraClickBehaviour : CameraBehaviour {
    /// <summary>
    /// 点击半径
    /// </summary>
    [SerializeField]
    private float radius;

    public override void GameUpdate() {
        if (Input.GetMouseButtonDown(0)) {
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
    }
}