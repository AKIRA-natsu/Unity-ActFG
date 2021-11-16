using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActFG.UI {
    /// <summary>
    /// UI窗口
    /// </summary>
    public enum WinEnum {}

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

    public class WinData {
        // UI窗口
        public WinEnum winEnum;
        // UI路径
        public string winPath;
        // UI类型
        public WinType winType;
    }
}
