using UnityEngine;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 状态
    /// </summary>
    public enum AIState {
        /// <summary>
        /// 待机
        /// </summary>
        Idle,
        /// <summary>
        /// 等待
        /// </summary>
        Wait,
        /// <summary>
        /// 移动
        /// </summary>
        Speed,
        /// <summary>
        /// 跳跃
        /// </summary>
        Jump,
        /// <summary>
        /// 攀爬
        /// </summary>
        Climb,
        /// <summary>
        /// 准备攻击
        /// </summary>
        ReadyAttack,
        /// <summary>
        /// 攻击
        /// </summary>
        Attack,
        /// <summary>
        /// 死亡
        /// </summary>
        Die,
    }
}