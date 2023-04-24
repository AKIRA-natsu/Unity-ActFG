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

    /// <summary>
    /// 更新组
    /// </summary>
    public const string Group = "SelfAnimation";

    protected virtual void OnEnable() {
        if (auto)
            this.Regist(Group);
    }

    protected virtual void OnDisable() {
        if (auto)
            this.Remove(Group);
    }

    public abstract void GameUpdate();
}