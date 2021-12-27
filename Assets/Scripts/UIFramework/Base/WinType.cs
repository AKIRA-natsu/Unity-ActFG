namespace ActFG.UIFramework {
    /// <summary>
    /// UI类型
    /// </summary>
    public enum WinType {
        // 正常UI 隐藏游戏本体
        Normal,
        // 过场 与游戏并存 并且可以操控（过场动画等）
        Interlude,
        // 过场 与游戏并存 屏幕一边通知
        Notify,
    }
}