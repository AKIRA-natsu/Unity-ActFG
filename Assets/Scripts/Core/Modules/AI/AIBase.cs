using UnityEngine;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 基类
    /// </summary>
    [SelectionBase]
    public abstract class AIBase : MonoBehaviour, IUpdateCallback, ILinkAnima {
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
                return ianima;
            }
        }

        /// <summary>
        /// 更新模式
        /// </summary>
        [SerializeField]
        protected UpdateMode mode = UpdateMode.Update;

        /// <summary>
        /// 更新组
        /// </summary>
        public const string Group = "AI";

        // abstract functions
        public abstract void GameUpdate();
        public abstract void OnUpdateStop();
        public abstract void OnUpdateResume();

        /// <summary>
        /// 开始更新
        /// </summary>
        #if UNITY_EDITOR
        [ContextMenu("Begin Update")]
        #endif
        public void BeginUpdate() {
            this.Regist(Group, mode);
        }

        /// <summary>
        /// 结束更新
        /// </summary>
        #if UNITY_EDITOR
        [ContextMenu("Stop Update")]
        #endif
        public void StopUpdate() {
            this.Remove(Group, mode);
        }
    }
}