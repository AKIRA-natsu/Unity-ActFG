using UnityEngine;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class AIBase : MonoBehaviour, IPool, IUpdate, ILinkAnima {
        // parameters
        private IAnima ianima;
        /// <summary>
        /// 动画
        /// </summary>
        /// <value></value>
        public IAnima Animation {
            get {
                if (ianima == null)
                    ianima = this.GetComponentInChildren<IAnima>();
                if (ianima == null)
                    throw new System.NullReferenceException($"{this.name}: AI下没有动画脚本");
                return ianima;
            }
        }

        /// <summary>
        /// 更新模式
        /// </summary>
        [SerializeField]
        protected UpdateMode mode = UpdateMode.Update;
        
        /// <summary>
        /// 状态
        /// </summary>
        [ReadOnly]
        [SerializeField]
        protected AIState state = AIState.Idle;

        /// <summary>
        /// 更新组
        /// </summary>
        public const string Group = "AI";

        // abstract functions
        public abstract void Wake();
        public abstract void Recycle();
        public abstract void GameUpdate();
    }
}