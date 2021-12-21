namespace ActFG.UIFramework {
    /// <summary>
    /// UI 基类
    /// </summary>
    public abstract class UIBase {

        public UIBase() {}

        /// <summary>
        /// 唤醒
        /// </summary>
        public virtual void Awake() {}
        /// <summary>
        /// 进入
        /// </summary>
        public virtual void OnEnter() {}
        /// <summary>
        /// 继续执行
        /// </summary>
        public virtual void OnResume() {}
        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void OnPause() {}
        /// <summary>
        /// 退出
        /// </summary>
        public virtual void OnExit() {}
    }
}