using System;
using AKIRA.UIFramework;
using UnityEngine;

public static class Extend {
    #region 属性 字段

    private static Camera mainCamera;
    /// <summary>
    /// 主摄像机
    /// </summary>
    /// <value></value>
    public static Camera MainCamera {
        get {
            if (mainCamera == null)
                mainCamera = Camera.main;
            return mainCamera;
        }
    }
    #endregion

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
        GameObject.Destroy(com, time);
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
    public static T GetComponent<T>(this Transform parent, string path) {
        var child = parent.Find(path);
        return child.GetComponent<T>();
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
        Debug.Log(action.Method.ToString().Colorful(Color.yellow) + " => " + stopwatch.Elapsed.TotalMilliseconds);
    }
    #endregion

    #region 字体颜色
    /// <summary>
    /// 富文本
    /// </summary>
    /// <param name="str"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Colorful(this string str, string color) {
        return "<color=" + color + ">" + str + "</color>";
    }

    /// <summary>
    /// 富文本
    /// </summary>
    /// <param name="Str"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Colorful(this string Str, Color color) {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + Str + "</color>";
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
        var UISize = UICanvas.Rect.sizeDelta;
        Vector2 uipos;
        uipos.x = (screenpos2.x / Screen.width) * UISize.x;
        uipos.y = (screenpos2.y / Screen.height) * UISize.y;
        return uipos;
    }

    /// <summary>
    /// 世界坐标转 UGUI 坐标
    /// </summary>
    public static Vector2 WorldToUGUI(this Vector3 position) {
        Vector2 ScreenPoint = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(position);
        Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
        ScreenPoint -= ScreenSize / 2;//将屏幕坐标变换为以屏幕中心为原点
        Vector2 anchorPos = ScreenPoint / ScreenSize * UICanvas.Rect.sizeDelta;//缩放得到UGUI坐标
        return anchorPos;
    }

    /// <summary>
    /// <para>运行模式下Texture转换成Texture2D</para>
    /// <para>来源：https://blog.csdn.net/s15100007883/article/details/80411638</para>
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    private static Texture2D TextureToTexture2D(Texture texture) {
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
    #endregion

    #region 数字转换
    public static string ShortNumStr(this string num)//show in short string
    {
        int num1 = num.Length - 1;
        string str1;
        if (num1 / 3 > 0)
        {
            string str2 = num.Substring(num1 % 3 + 1, 3 - num1 % 3);
            string str3;
            if (str2 == "0" || str2 == "00" || str2 == "000")
            {
                str3 = string.Empty;
            }
            else
            {
                if (str2.Length == 2)
                {
                    if (str2[1] == '0')
                        str2 = str2.Substring(0, 1);
                }
                else if (str2.Length == 3 && str2[2] == '0')
                {
                    str2 = str2.Substring(0, 2);
                    if (str2[1] == '0')
                        str2 = str2.Substring(0, 1);
                }
                str3 = "." + str2;
            }
            str1 = num.Substring(0, num1 % 3 + 1) + str3;
        }
        else
            str1 = num;
        switch (num1 / 3)
        {
            case 0:
                return str1 + string.Empty;
            case 1:
                return str1 + "K";
            case 2:
                return str1 + "M";
            case 3:
                return str1 + "B";
            case 4:
                return str1 + "t";
            case 5:
                return str1 + "q";
            case 6:
                return str1 + "Q";
            case 7:
                return str1 + "S";
            case 8:
                return str1 + "O";
            case 9:
                return str1 + "n";
            case 10:
                return str1 + "d";
            case 11:
                return str1 + "U";
            case 12:
                return str1 + "D";
            case 13:
                return str1 + "T";
            case 14:
                return str1 + "Qt";
            case 15:
                return str1 + "Qd";
            case 16:
                return str1 + "Sd";
            case 17:
                return str1 + "St";
            case 18:
                return str1 + "N";
            case 19:
                return str1 + "v";
            case 20:
                return str1 + "c";
            case 21:
                return str1 + "AM";
            case 22:
                return str1 + "PM";
            case 23:
                return str1 + "Af";
            default:
                return str1 + "E";
        }
    }
    #endregion

    #region Enum
    /// <summary>
    /// 获得随机Enum
    /// </summary>
    public static T RandomEnum<T>() {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
    #endregion
}