using UnityEngine;

/// <summary>
/// 摄像机移动脚本
/// </summary>
public class CameraFollowBehaviour : CameraBehaviour {
    // 跟随目标
    public Transform target;
    // 跟随速度
    public float lerpSpeed;
    // 差值
    private Vector3 offset;

    // 是否跟随
    public bool follow = false;

    private void Start() {
        offset = this.transform.position - target.position;
    }

    public override void GameUpdate() {
        if (!follow)
            return;
        
        this.transform.position =
            Vector3.Lerp(this.transform.position, target.position + offset, Time.deltaTime * lerpSpeed);
    }
}