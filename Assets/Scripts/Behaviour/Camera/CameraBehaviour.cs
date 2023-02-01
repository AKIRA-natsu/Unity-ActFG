using UnityEngine;

/// <summary>
/// 摄像机表现基类
/// </summary>
public abstract class CameraBehaviour : MonoBehaviour, IUpdate {
    // 更新模式
    public UpdateMode mode = UpdateMode.Update;
    // 摄像机更新组
    protected const string CameraGroup = "Camera";

    private void OnEnable() {
        this.Regist(CameraGroup, mode);
    }

    private void OnDisable() {
        this.Remove(CameraGroup, mode);
    }

    public abstract void GameUpdate();
}