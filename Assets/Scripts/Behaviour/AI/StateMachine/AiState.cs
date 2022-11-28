namespace AKIRA.AI.StateMachine {
    /// <summary>
    /// AI状态
    /// </summary>
    public enum AiState {
        /// <summary>
        /// 待机
        /// </summary>
        Idle,
        /// <summary>
        /// 巡逻
        /// </summary>
        Patrol,
        /// <summary>
        /// 追随
        /// </summary>
        Chase,
        /// <summary>
        /// 攻击
        /// </summary>
        Attack,
        /// <summary>
        /// 死亡
        /// </summary>
        Dead,
    }
}