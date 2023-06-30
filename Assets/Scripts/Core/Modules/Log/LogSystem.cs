using System;
using System.Collections.Generic;
using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
#endif

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

        public void Log(object message, string key, Object context = null) {
            var data = GetData(key);
            if (!data.logable)
                return;
            Debug.Log(AddFullTag(message, key).Colorful(data.color), context);
        }

        public void Warn(object message, Object context = null) {
            Debug.LogWarning(AddFullTag(message, GameData.Log.Warn), context);
        }

        public void Error(object message, Object context = null) {
            Debug.LogError(AddFullTag(message, GameData.Log.Error), context);
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
        private string AddFullTag(object message, string key) {
            if (!fully)
                return $"<b>{message}</b>";
            else
                return $"<b>{key}日志： {message}</b>";
        }
    }
}

public static class LogExtend {
    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    /// <param name="key"></param>
    /// <param name="context">聚焦到Hierarchy，Debug.Log第二个参数</param>
    public static void Log(this object message, string key = GameData.Log.Default, Object context = null) {
        LogSystem.Instance.Log(message, key, context);
    }

    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context">聚焦到Hierarchy，Debug.Log第二个参数</param>
    public static void Warn(this object message, Object context = null) {
        LogSystem.Instance.Warn(message, context);
    }

    /// <summary>
    /// 日志
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context">聚焦到Hierarchy，Debug.Log第二个参数</param>
    public static void Error(this object message, Object context = null) {
        LogSystem.Instance.Error(message, context);
    }

}

#if UNITY_EDITOR
/// <summary>
/// <para>双击打开日志文件所在位置</para>
/// <para>来源：http://www.cppblog.com/heath/archive/2016/06/21/213777.html</para>
/// </summary>
public class LogEditor {
    [UnityEditor.Callbacks.OnOpenAsset(0)]
    private static bool OnOpenAsset(int instanceID, int line) {
        string stackTrace = GetStackTrace();
        // 过滤标签
        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("日志")) {
            Match matches = Regex.Match(stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
            // 获得当前点击的对象文件路径
            string pathline = AssetDatabase.GetAssetPath(instanceID);
            if (pathline.Contains("LogSystem.cs")) {
                while (matches.Success) {
                    pathline = matches.Groups[1].Value;

                    // 过滤进入LogSystem的行
                    if (!pathline.Contains("LogSystem.cs")) {
                        int splitIndex = pathline.LastIndexOf(":");
                        string path = pathline.Substring(0, splitIndex);
                        line = Convert.ToInt32(pathline.Substring(splitIndex + 1));
                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullPath = fullPath + path;
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                        break;
                    }
                    matches = matches.NextMatch();
                }
            } else {
                // 直接打开对应文件和对应列
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(pathline.Replace('/', '\\'), line);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得日志输出信息
    /// </summary>
    /// <returns></returns>
    private static string GetStackTrace() {
        var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        var fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        var consoleWindowInstance = fieldInfo.GetValue(null);
        if (consoleWindowInstance == null)
            return default;
        if ((object)EditorWindow.focusedWindow != consoleWindowInstance)
            return default;
        // 
        var listViewStateType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ListViewState");
        fieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
        var listView = fieldInfo.GetValue(consoleWindowInstance);
        // 
        fieldInfo = listViewStateType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
        var row = (int)fieldInfo.GetValue(listView);
        // 
        fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
        string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
        return activeText;
    }
}
#endif