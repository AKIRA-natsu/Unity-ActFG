using System;
using UnityEngine;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;

namespace Util.Tools {
    public static class UtilTools {
        #region 代码耗时
        public static void CodeCost(this Action action) {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            action?.Invoke();
            stopwatch.Stop();
            Debug.Log(action.Method.ToString().StringColor(Color.yellow) + " => " + stopwatch.Elapsed.TotalMilliseconds);
        }
        #endregion

        #region 字体颜色
        /// <summary>
        /// 富文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string StringColor(this string str, string color) {
            return "<color=" + color + ">" + str + "</color>";
        }

        /// <summary>
        /// 富文本
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string StringColor(this string Str, Color color) {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + Str + "</color>";
        }
        #endregion

        #region 延迟触发
        public static float _time = 0; 
        public static void Delay(this Action action, float delayTime) {
            if (_time >= delayTime) {
                action?.Invoke();
                _time = 0;
            }
            
            _time += Time.deltaTime;
        }
        #endregion
    }
}