using UnityEngine;

namespace AKIRA.AI {
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class AIBase : MonoBehaviour, IPool, IUpdate, ILinkAnima {
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

        public abstract void Wake();
        public abstract void Recycle();
        public abstract void GameUpdate();
    }
}