using UnityEngine;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 状态
    /// </summary>
    public enum AIState {
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
        /// 攀爬结束 
        /// </summary>
        ClimbToTop,
    }
}