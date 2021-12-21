using System.Globalization;
using System;
using UnityEngine;

namespace ActFG.Util.Tools {
    public static class Extend {
        #region GameObject
        /// <summary>
        /// GameObject Dont Destory
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static GameObject DontDestory(this GameObject go) {
            GameObject.DontDestroyOnLoad(go);
            return go;
        }

        /// <summary>
        /// 加载 T
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(this string path) {
            return (T)(object)Resources.Load(path);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GameObject Instantiate(this GameObject obj) {
            var go = GameObject.Instantiate(obj);
            go.name = obj.name;
            return go;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static GameObject Instantiate(this GameObject obj, Vector3 position, Vector3 rotation) {
            return GameObject.Instantiate(obj, position, Quaternion.Euler(rotation));
        }

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="time">销毁等待时间</param>
        public static void Destory(this GameObject obj, float time = 0) {
            GameObject.Destroy(obj, time);
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

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <param name="isUI">
        ///     <para>是否是 UI</para>
        ///     <para>UI情况下防止父物体对子物体的影响</para>
        ///     <para>参考： https://blog.csdn.net/qq_42672770/article/details/109180796</para>
        /// </param>
        /// <returns></returns>
        public static GameObject SetParent(this GameObject child, GameObject parent, bool isUI = false) {
            child.transform.SetParent(parent.transform, !isUI);
            return child;
        }

        /// <summary>
        /// 获得子物体组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(this GameObject parent, string path) {
            var child = parent.transform.Find(path);
            return child.GetComponent<T>();
        }

        /// <summary>
        /// 获得子物体组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(this Transform parent, string path) {
            var child = parent.Find(path);
            return child.GetComponent<T>();
        }
        #endregion
        
        #region object
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(this object message) {
            Debug.Log(message);
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(this object message) {
            Debug.LogWarning(message);
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(this object message) {
            Debug.LogError(message);
        }
        #endregion

        #region 代码耗时
        /// <summary>
        /// 代码耗时
        /// </summary>
        /// <param name="action"></param>
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
    }
}