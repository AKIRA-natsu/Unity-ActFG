namespace ActFG.UI {
    /// <summary>
    /// UI 基类
    /// </summary>
    public abstract class UIBase {
        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        public WinData data { get; private set; }

        public UIBase() => this.data = null;
        public UIBase(WinData data) => this.data = data;
        public UIBase(WinEnum @enum, string path, WinType @type) => this.data = new WinData(@enum, path, @type);

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