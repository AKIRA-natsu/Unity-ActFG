using UnityEngine;

namespace AKIRA.AI.Animation {
    /// <summary>
    /// AI 动画
    /// </summary>
    public abstract class AIAnimationBase {
        /// <summary>
        /// 动画控制器
        /// </summary>
        protected Animator animator { get; private set; }

        public AIAnimationBase(Animator animator) => this.animator = animator;
    }
}