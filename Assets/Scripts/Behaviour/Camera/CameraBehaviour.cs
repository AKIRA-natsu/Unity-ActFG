using UnityEngine;

/// <summary>
/// 摄像机表现基类
/// </summary>
public abstract class CameraBehaviour : MonoBehaviour, IUpdate {
    // 更新模式
    public UpdateMode mode = UpdateMode.Update;

    private void OnEnable() {
        this.Regist(mode);
    }

    private void OnDisable() {
        this.Remove(mode);
    }

    public abstract void GameUpdate();
}