using UnityEngine;

namespace AKIRA.AI {
    /// <summary>
    /// 动画
    /// </summary>
    public interface IAnima {
        /// <summary>
        /// 切换动画
        /// </summary>
        /// <param name="state">状态 Aniamion Name</param>
        /// <param name="data">数据 float/bool</param>
        void SwitchAnima(AIState state, Object data = null);
    }
}