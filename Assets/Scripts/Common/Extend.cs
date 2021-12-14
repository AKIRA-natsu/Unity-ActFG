using System;
using UnityEngine;
using Color = UnityEngine.Color;
using ActFG.Manager;

namespace ActFG.Util.Tools {
    public static class Extend {
        #region ResourceLoad
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(this string path) {
            return (T)(object)Resources.Load(path);
        }
        #endregion

        #region GameObject
        /// <summary>
        /// 不销毁对象
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static GameObject DontDestory(this GameObject go) {
            GameObject.DontDestroyOnLoad(go);
            return go;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject SetParent(this GameObject child, Transform parent) {
            child.transform.SetParent(parent);
            return child;
        }
        #endregion

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
            return $"<color={color}>{str}</color>";
        }

        /// <summary>
        /// 富文本
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string StringColor(this string Str, Color color) {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{Str}</color>";
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