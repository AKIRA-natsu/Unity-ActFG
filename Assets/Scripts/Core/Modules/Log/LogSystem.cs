using System.Collections.Generic;
using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Manager {
    public class LogSystem : Singleton<LogSystem> {
        // 配置文件
        private LogConfig config;
        // 颜色表
        private Dictionary<string, (Color, bool)> ColorMap = new Dictionary<string, (Color, bool)>();

        // 是否详细
        private bool fully;
        
        protected LogSystem() {
            config = GameData.Path.LogConfig.Load<LogConfig>();
            fully = config?.logfully ?? true;

            // 默认四种
            ColorMap.Add(GameData.Log.Default, (Color.white, true));
            ColorMap.Add(GameData.Log.Success, (Color.green, true));
            ColorMap.Add(GameData.Log.Warn, (Color.yellow, true));
            ColorMap.Add(GameData.Log.Error, (Color.red, true));
        }

        public void Log(object message) {
            Debug.Log(message);
        }

        public void Log(string message, string key) {
            var data = GetData(key);
            if (!data.logable)
                return;
            AddFullTag(ref message, key);
            Log(message.Colorful(data.color));
        }

        public void Warn(object message) {
            Debug.LogWarning(message);
        }

        public void Error(object message) {
            Debug.LogError(message);
        }

        /// <summary>
        /// 获得颜色
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private (Color color, bool logable) GetData(string key) {
            if (ColorMap.ContainsKey(key)) {
                return ColorMap[key];
            } else {
                var color = config.GetData(key);
                ColorMap.Add(key, color);
                return color;
            }
        }

        /// <summary>
        /// 添加详细日志开头
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        private void AddFullTag(ref string message, string key) {
            if (!fully)
                return;

            message = $"{key}日志： {message}";
        }
    }
}

public static class LogExtend {
    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    public static void Log(this object message) {
        LogSystem.Instance.Log(message);
    }

    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    public static void Warn(this object message) {
        LogSystem.Instance.Warn(message);
    }

    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    public static void Error(this object message) {
        LogSystem.Instance.Error(message);
    }

    /// <summary>
    /// string color 日志
    /// </summary>
    /// <param name="message"></param>
    /// <param name="key"></param>
    public static void Log(this string message, string key = GameData.Log.Default) {
        LogSystem.Instance.Log(message, key);
    }

}