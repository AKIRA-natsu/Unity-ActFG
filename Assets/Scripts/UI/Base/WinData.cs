namespace ActFG.UI {
    /// <summary>
    /// UI窗口
    /// </summary>
    public enum WinEnum {
        // 主界面
        Main,
        // 暂停界面
        Pause,
        // 设置界面
        Setting,
    }

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

    /// <summary>
    /// UI 数据
    /// </summary>
    public class WinData {
        // UI 窗口
        public WinEnum @enum { get; private set; }
        // UI 路径
        public string path { get; private set; }
        // UI 类型
        public WinType @type { get; private set; }
        // UI 名称
        public string name { get; private set; }

        public WinData(WinEnum @enum, string path, WinType @type) {
            this.@enum = @enum;
            this.path = path;
            this.@type = @type;
            this.name = path.Substring(path.LastIndexOf('/') + 1);
        }
    }
}