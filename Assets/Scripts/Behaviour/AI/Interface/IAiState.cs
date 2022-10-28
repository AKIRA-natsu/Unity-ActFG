/// <summary>
/// AI接口
/// </summary>
public interface IAiState : IUpdate<AiAgent> {
    /// <summary>
    /// 对应的Enum
    /// </summary>
    /// <returns></returns>
    AiState GetStateID();

    /// <summary>
    /// 进入
    /// </summary>
    /// <param name="agent"></param>
    void Enter(AiAgent agent);
    /// <summary>
    /// 退出
    /// </summary>
    /// <param name="agent"></param>
    void Exit(AiAgent agent);
}