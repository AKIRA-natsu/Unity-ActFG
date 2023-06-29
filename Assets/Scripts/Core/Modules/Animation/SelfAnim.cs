using AKIRA.Data;
using UnityEngine;

/// <summary>
/// 正负朝向
/// </summary>
public enum PunchWard {
    // 正向
    Forward,
    // 倒向
    Backward,
}

/// <summary>
/// 自身动画基类
/// </summary>
public abstract class SelfAnim : MonoBehaviour, IUpdate {
    [CNName("自动更新")]
    [SerializeField]
    protected bool auto = false;

    protected virtual void OnEnable() {
        if (auto)
            this.Regist(GameData.Group.SelfAnimation);
    }

    protected virtual void OnDisable() {
        if (auto)
            this.Remove(GameData.Group.SelfAnimation);
    }

    public abstract void GameUpdate();
}