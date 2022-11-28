using UnityEngine;

namespace AKIRA.AI.StateMachine {
    /// <summary>
    /// 状态机
    /// </summary>
    public class AiStateMachine : IUpdate {
        // 持有者
        private HumanAgentBase agent;
        // 注册的状态
        private IAiState[] states;
        // 当前状态
        public AiState currentState { get; private set; }

        public AiStateMachine(HumanAgentBase agent) {
            this.agent = agent;
            states = new IAiState[System.Enum.GetValues(typeof(AiState)).Length];
        }

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="state"></param>
        public void RegistState(IAiState state) {
            var id = (int)state.GetStateID();
            if (states[id] != null)
                $"{this} 状态覆盖{state.GetStateID()}".Colorful(Color.yellow).Log();
            states[(int)state.GetStateID()] = state;
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state"></param>
        public void RemoveState(IAiState state) {
            states[(int)state.GetStateID()] = null;
        }

        /// <summary>
        /// 获得当前状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private IAiState GetState(AiState state) {
            return states[(int)state];
        }

        /// <summary>
        /// 变换状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(AiState newState) {
            GetState(currentState)?.Exit(agent);
            currentState = newState;
            GetState(currentState)?.Enter(agent);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void GameUpdate() {
            GetState(currentState)?.GameUpdate(agent);
        }
    }
}