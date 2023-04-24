using UnityEngine.AI;

namespace AKIRA.Behaviour.AI {
    public interface IState : IUpdate<FSMMachine> {
        /// <summary>
        /// 状态
        /// </summary>
        /// <value></value>
        AIState State { get; }

        /// <summary>
        /// 进入状态
        /// </summary>
        /// <returns></returns>
        AIState Enter();
        /// <summary>
        /// 更新状态
        /// </summary>
        void Exit();
    }
}