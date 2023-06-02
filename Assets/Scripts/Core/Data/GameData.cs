namespace AKIRA.Data {
    /// <summary>
    /// 游戏整体数据存储
    /// </summary>
    public static class GameData {
        #region 程序集
        public class DLL {
            /// <summary>
            /// 默认程序集
            /// </summary>
            public const string Default = "Assembly-CSharp";
            /// <summary>
            /// AKIRA Runtime
            /// </summary>
            public const string AKIRA_Runtime = "AKIRA.Game.Core";
            /// <summary>
            /// AKIRA Editor
            /// </summary>
            public const string AKIRA_Editor = "AKIRA.Core.Editor";
        }
        #endregion

        #region 更新组
        public class Group {
            /// <summary>
            /// 默认组
            /// </summary>
            public const string Default = "Default";
            /// <summary>
            /// 自身动画
            /// </summary>
            public const string UI = "UI";
            /// <summary>
            /// AI
            /// </summary>
            public const string AI = "AI";
            /// <summary>
            /// 摄像机
            /// </summary>
            public const string Camera = "Camera";
            /// <summary>
            /// 自身动画
            /// </summary>
            public const string SelfAnimation = "SelfAnimation";
        }
        #endregion

        #region 路径
        public class Path {
            /// <summary>
            /// 音乐配置文件
            /// </summary>
            public const string AudioConfig = "Config/AudioResourceConfig";
            /// <summary>
            /// UI配置文件
            /// </summary>
            public const string UIConfig = "Config/UIRuleConfig";
            /// <summary>
            /// 日志配置文件
            /// </summary>
            public const string LogConfig = "Config/LogConfig";
        }
        #endregion

        #region 日志
        public class Log {
            /// <summary>
            /// 默认 白色
            /// </summary>
            public const string Default = "Default";
            /// <summary>
            /// 成功
            /// </summary>
            public const string Success = "Success";
            /// <summary>
            /// 警告
            /// </summary>
            public const string Warn = "Warn";
            /// <summary>
            /// 错误
            /// </summary>
            public const string Error = "Error";
            /// <summary>
            /// 编辑器
            /// </summary>
            public const string Editor = "Editor";
            /// <summary>
            /// 游戏状态
            /// </summary>
            public const string GameState = "GameState";
            /// <summary>
            /// 事件
            /// </summary>
            public const string Event = "Event";
            /// <summary>
            /// 资源
            /// </summary>
            public const string Source = "Source";
            /// <summary>
            /// 指引
            /// </summary>
            public const string Guide = "Guide";
            /// <summary>
            /// UI
            /// </summary>
            public const string UI = "UI";
            /// <summary>
            /// AI
            /// </summary>
            public const string AI = "AI";
            /// <summary>
            /// 作弊
            /// </summary>
            public const string Cheat = "Cheat";
            /// <summary>
            /// 测试
            /// </summary>
            public const string Test = "Test";
        }
        #endregion

        #region 音频
        public class Audio {}
        #endregion

        #region 事件
        public class Event {
            /// <summary>
            /// 资源加载结束，SourceSystem结束
            /// </summary>
            public const string OnAppSourceEnd = "OnAppSourceEnd";
            /// <summary>
            /// 游戏Focus
            /// </summary>
            public const string OnAppFocus = "OnAppFocus";
        }
        #endregion

        #region 资源父名称
        public class Source {
            /// <summary>
            /// 管理器
            /// </summary>
            public const string Manager = "[Manager]";
            /// <summary>
            /// 其他基础部分
            /// </summary>
            public const string Base = "[Base]";
            /// <summary>
            /// 测试
            /// </summary>
            public const string Test = "[Test]";
        }
        #endregion

        #region 摄像机
        public class Camera {
            /// <summary>
            /// 主摄像机
            /// </summary>
            public const string Main = "Main";
            /// <summary>
            /// 副摄像机
            /// </summary>
            public const string Sub = "Sub";
            /// <summary>
            /// UI
            /// </summary>
            public const string UI = "UI";
        }
        #endregion
    }
}