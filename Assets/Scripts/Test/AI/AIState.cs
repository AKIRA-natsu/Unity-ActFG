/// <summary>
/// AI 状态
/// </summary>
public enum AIState {
    /// <summary>
    /// 待机
    /// </summary>
    Idle,
    /// <summary>
    /// 移动
    /// </summary>
    Walk,
    /// <summary>
    /// 跑步
    /// </summary>
    Run,
    /// <summary>
    /// 跳跃
    /// </summary>
    Jump,
    /// <summary>
    /// 攀爬
    /// </summary>
    Climb,
    /// <summary>
    /// 坐下
    /// </summary>
    Sit,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 死亡
    /// </summary>
    Dead,
}