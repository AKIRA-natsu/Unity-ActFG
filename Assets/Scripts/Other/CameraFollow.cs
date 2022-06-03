using UnityEngine;

public class CameraFollow : MonoBehaviour, IUpdate {
    // 更新模式
    public UpdateMode mode = UpdateMode.Update;
    // 跟随目标
    public Transform target;
    // 跟随速度
    public float lerpSpeed;
    // 差值
    private Vector3 offset;

    // 是否跟随
    public bool follow = false;

    private void Start() {
        UpdateManager.Instance.Regist(this, mode);

        offset = this.transform.position - target.position;
    }

    public void GameUpdate() {
        if (!follow)
            return;
        
        this.transform.position =
            Vector3.Lerp(this.transform.position, target.position + offset, Time.deltaTime * lerpSpeed);
    }
}