namespace AKIRA.UIFramework {
    /// <summary>
    /// UI窗口
    /// </summary>
    public enum WinEnum {
        [Remark("空")]
        None = 100,
        [Remark("主界面")]
        Main,
        [Remark("暂停界面")]
        Pause,
        [Remark("设置界面")]
        Setting,
        [Remark("指引界面")]
        Guide,
        [Remark("贝塞尔界面")]
        Bezier,
        [Remark("过渡界面")]
        Transition,
        [Remark("测试")]
        Test,
    }
}
