using UnityEngine;

/// <summary>
/// 摄像机表现基类
/// </summary>
public abstract class CameraBehaviour : MonoBehaviour, IUpdate {
    // 更新模式
    public UpdateMode mode = UpdateMode.Update;

    private void OnEnable() {
        UpdateManager.Instance.Regist(this, mode);
    }

    private void OnDisable() {
        if (!UpdateManager.IsApplicationOut)
            UpdateManager.Instance.Remove(this, mode);
    }

    public abstract void GameUpdate();
}