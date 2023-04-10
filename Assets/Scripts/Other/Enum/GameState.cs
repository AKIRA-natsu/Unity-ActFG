/// <summary>
/// 游戏状态
/// </summary>
public enum GameState {
    /// <summary>
    /// 无
    /// </summary>
    None = 1000,
    /// <summary>
    /// 准备开始
    /// </summary>
    Ready,
    /// <summary>
    /// 游玩中
    /// </summary>
    Playing,
    /// <summary>
    /// 过渡
    /// </summary>
    Transition,
    /// <summary>
    /// 恢复
    /// </summary>
    Resume,
    /// <summary>
    /// 暂停
    /// </summary>
    Pause,
    /// <summary>
    /// 失败
    /// </summary>
    Fail,
    /// <summary>
    /// 完成
    /// </summary>
    Complete,
    /// <summary>
    /// 退出
    /// </summary>
    Exit,
}