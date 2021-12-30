using ActFG.Attribute;

namespace ActFG.UIFramework {
    /// <summary>
    /// UI窗口
    /// </summary>
    public enum WinEnum {
        [Remark("空")]
        None = 0,
        [Remark("主界面")]
        Main,
        [Remark("暂停界面")]
        Pause,
        [Remark("设置界面")]
        Setting,
    }
}
