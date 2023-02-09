using UnityEngine;

/// <summary>
/// 自身动画基类
/// </summary>
public abstract class SelfAnim : MonoBehaviour, IUpdate {
    /// <summary>
    /// 正负朝向
    /// </summary>
    public enum PunchWard {
        // 正向
        Forward,
        // 倒向
        Backward,
    }

    [CNName("自动更新")]
    [SerializeField]
    protected bool auto = false;
    // 自身动画组
    private const string AnimationGroup = "SelfAnimation";

    protected virtual void OnEnable() {
        if (auto)
            this.Regist();
    }

    protected virtual void OnDisable() {
        if (auto)
            this.Remove();
    }

    public abstract void GameUpdate();
}