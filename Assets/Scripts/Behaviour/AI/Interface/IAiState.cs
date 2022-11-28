namespace AKIRA.AI.StateMachine {
    /// <summary>
    /// AI接口
    /// </summary>
    public interface IAiState : IUpdate<HumanAgentBase> {
        /// <summary>
        /// 对应的Enum
        /// </summary>
        /// <returns></returns>
        AiState GetStateID();

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="agent"></param>
        void Enter(HumanAgentBase agent);
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="agent"></param>
        void Exit(HumanAgentBase agent);
    }
}