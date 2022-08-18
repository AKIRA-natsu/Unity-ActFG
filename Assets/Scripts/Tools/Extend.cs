using System;
using System.Collections.Generic;
using AKIRA.UIFramework;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public static T Load<T>(this string path) where T : UnityEngine.Object {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Instantiate<T>(this T com) where T : Component {
        return GameObject.Instantiate(com.gameObject).GetComponent<T>();
    }

    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="com"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Instantiate<T>(this T com, Vector3 position = default, Quaternion rotation = default) where T : Component {
        return GameObject.Instantiate(com.gameObject, position, rotation).GetComponent<T>();
    }

    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="obj"></param>
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
    public static GameObject Instantiate(this GameObject obj, Vector3 position = default, Quaternion rotation = default) {
        var go = GameObject.Instantiate(obj, position, rotation);
        go.name = obj.name;
        return go;
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
    /// 销毁
    /// </summary>
    /// <param name="com"></param>
    /// <param name="time">销毁等待时间</param>
    /// <typeparam name="T"></typeparam>
    public static void Destory<T>(this T com, float time = 0) where T : Component {
        GameObject.Destroy(com.gameObject, time);
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
    public static T SetParent<T>(this T child, Component parent, bool isUI = false) where T : Component {
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
    public static T GetComponent<T>(this Component parent, string path) {
        var child = parent.transform.Find(path);
        return child.GetComponent<T>();
    }

    /// <summary>
    /// UI 获得组件
    /// </summary>
    /// <param name="com"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetComponent<T>(this AKIRA.UIFramework.UIComponent com) {
        return com.transform.GetComponent<T>();
    }

    /// <summary>
    /// 获得物体大小
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetSize(this GameObject obj) {
        return GetSize(obj.transform);
    }

    /// <summary>
    /// 获得物体大小
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetSize(this Transform obj) {
        var length = obj.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        Vector3 size;
        size.x = length.x * obj.lossyScale.x;
        size.y = length.y * obj.lossyScale.y;
        size.z = length.z * obj.lossyScale.z;
        return size;
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

    #region string 字体
    /// <summary>
    /// 富文本
    /// </summary>
    /// <param name="str"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Colorful(this string str, string color) {
        return $"<color={color}>{str}</color>";
    }

    /// <summary>
    /// 富文本
    /// </summary>
    /// <param name="str"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Colorful(this string str, Color color) {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";
    }

    /// <summary>
    /// 文本大小
    /// </summary>
    /// <param name="str"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string Size(this string str, int size) {
        return $"<size={size}>{str}</size>";
    }
    #endregion

    #region UI
    /// <summary>
    /// 屏幕坐标转 UGUI 坐标
    /// </summary>
    /// <param name="screenpos"></param>
    /// <returns></returns>
    public static Vector2 ScreenToUGUI(this Vector3 screenpos) {
        Vector2 screenpos2 = new Vector2(screenpos.x - (Screen.width / 2), screenpos.y - (Screen.height / 2));
        var UISize = UI.Rect.sizeDelta;
        Vector2 uipos;
        uipos.x = (screenpos2.x / Screen.width) * UISize.x;
        uipos.y = (screenpos2.y / Screen.height) * UISize.y;
        return uipos;
    }

    /// <summary>
    /// 世界坐标转 UGUI 坐标
    /// </summary>
    public static Vector2 WorldToUGUI(this Vector3 position) {
        Vector2 ScreenPoint = CameraExtend.MainCamera.WorldToScreenPoint(position);
        Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
        ScreenPoint -= ScreenSize / 2;//将屏幕坐标变换为以屏幕中心为原点
        Vector2 anchorPos = ScreenPoint / ScreenSize * UI.Rect.sizeDelta;//缩放得到UGUI坐标
        return anchorPos;
    }

    /// <summary>
    /// <para>运行模式下Texture转换成Texture2D</para>
    /// <para>来源：https://blog.csdn.net/s15100007883/article/details/80411638</para>
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Texture2D TextureToTexture2D(this Texture texture) {
            Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
            Graphics.Blit(texture, renderTexture);

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(renderTexture);

            return texture2D;
    }

    /// <summary>
    /// 移动端判断点击的位置是否有UI
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static bool IsPointerOverUiObject(this Vector2 screenPosition) {
        if (!EventSystem.current.Equals(null)) {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        return true;
    }
    #endregion

    #region Array
    /// <summary>
    /// 获得父节点下所有子节点的 <paramref name="T" />
    /// </summary>
    /// <param name="parent"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] GetChildrenArray<T>(this Transform parent) {
        var count = parent.childCount;
        T[] result = new T[count];
        for (int i = 0; i < count; i++)
            result[i] = parent.GetChild(i).GetComponent<T>();
        return result;
    }
    #endregion

}