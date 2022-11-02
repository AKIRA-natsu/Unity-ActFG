using UnityEngine;

/// <summary>
/// 在屏幕边缘指示怪物/敌人当前所处的方位
/// 参考链接：https://blog.csdn.net/weixin_39665110/article/details/108498639
/// </summary>
public static class OutScreenMark {
    /// <summary>
    /// 屏幕左下角
    /// </summary>
    public static Vector2 ScreenLeftBottom = new Vector2(0, 0);
    /// <summary>
    /// 屏幕右下角
    /// </summary>
    public static Vector2 ScreenRightBottom = new Vector2(Screen.width, 0);
    /// <summary>
    /// 屏幕左上角
    /// </summary>
    public static Vector2 ScreenLeftTop = new Vector2(0, Screen.height);
    /// <summary>
    /// 屏幕右上角
    /// </summary>
    public static Vector2 ScreenRightTop = new Vector2(Screen.width, Screen.height);
    /// <summary>
    /// 屏幕中心
    /// </summary>
    public static Vector2 ScreenCenter = new Vector2(Screen.width, Screen.height) / 2;

    private static float widthOffset;
    /// <summary>
    /// 宽度偏移量
    /// </summary>
    public static float WidthOffset {
        get => widthOffset;
        set {
            widthOffset = value;
            ResetScreenSize();
        }
    }

    private static float heightOffset;
    /// <summary>
    /// 高度偏移量
    /// </summary>
    /// <value></value>
    public static float HeightOffset {
        get => heightOffset;
        set {
            heightOffset = value;
            ResetScreenSize();
        }
    }

    /// <summary>
    /// 重置屏幕范围
    /// </summary>
    private static void ResetScreenSize() {
        ScreenLeftBottom = new Vector2(WidthOffset, HeightOffset);
        ScreenRightBottom = new Vector2(Screen.width - WidthOffset, HeightOffset);
        ScreenLeftTop = new Vector2(WidthOffset, Screen.height - HeightOffset);
        ScreenRightTop = new Vector2(Screen.width - WidthOffset, Screen.height - HeightOffset);
    }

    /// <summary>
    /// 是否在屏幕内
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    private static bool InScreen(this Vector3 screenPosition) {
        return screenPosition.x >= ScreenLeftBottom.x &&
            screenPosition.x <= ScreenRightTop.x &&
            screenPosition.y >= ScreenLeftBottom.y &&
            screenPosition.y <= ScreenRightTop.y;
    }

    /// <summary>
    /// 尝试获得交点
    /// </summary>
    /// <param name="isCross"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static (bool isCross, Vector3 crossPoint) TryGetIntersectPoint(this Vector3 position) {
        var screenPosition = CameraExtend.MainCamera.WorldToScreenPoint(position);
        if (InScreen(screenPosition))
            return default;

        var isCross = OnLinearAlgebra(ScreenCenter, screenPosition, out Vector3 crossPosition);
        return (isCross, crossPosition);
    }

    /// <summary>
    /// 尝试获得交点
    /// </summary>
    /// <param name="isCross"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static (bool isCross, Unity.Mathematics.float3 crossPoint) TryGetIntersectPoint(this Unity.Mathematics.float3 position) {
        var screenPosition = CameraExtend.MainCamera.WorldToScreenPoint(position);
        if (InScreen(screenPosition))
            return default;

        var isCross = OnLinearAlgebra(ScreenCenter, screenPosition, out Vector3 crossPosition);
        return (isCross, crossPosition);
    }

    /// <summary>
    /// 拿到交点
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="pos3"></param>
    /// <param name="pos4"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private static bool GetPoint(Vector2 pos1, Vector2 pos2, Vector2 pos3, Vector2 pos4, out Vector3 target)
    {
        target = default;

        float a = pos2.y - pos1.y;
        float b = pos1.x - pos2.x;
        float c = pos2.x * pos1.y - pos1.x * pos2.y;
        float d = pos4.y - pos3.y;
        float e = pos3.x - pos4.x;
        float f = pos4.x * pos3.y - pos3.x * pos4.y;
        float x = (f * b - c * e) / (a * e - d * b);
        float y = (c * d - f * a) / (a * e - d * b);
        // if (a * e == d * b)  //A1*B2==A2*B1  平行
        //     return;
        if (x < 0 || y < 0 || x > Screen.width || y > Screen.height) return false;
        if (!GetOnLine(pos1, pos2, new Vector2(x, y))) return false;
        target = new Vector3(x, y);
        return true;
    }

    /// <summary>
    /// 线代解
    /// </summary>
    private static bool OnLinearAlgebra(Vector3 pos1, Vector3 pos2, out Vector3 target)
    {
        target = default;
        //AB线段的abc   //a=y2-y1  b=x1-x2  c=x2y1-x1y2
        if (GetPoint(pos1, pos2, ScreenLeftBottom, ScreenLeftTop, out target)) return true;
        else if (GetPoint(pos1, pos2, ScreenRightBottom, ScreenRightTop, out target)) return true;
        else if (GetPoint(pos1, pos2, ScreenLeftTop, ScreenRightTop, out target)) return true;
        else if (GetPoint(pos1, pos2, ScreenLeftBottom, ScreenRightBottom, out target)) return true;
        return false;

    }
  
    /// <summary>
    /// 判断在哪根线
    /// </summary>
    /// <param name="pos1">a</param>
    /// <param name="pos2">b</param>
    /// <param name="pos3">p</param>
    private static bool GetOnLine(Vector2 pos1, Vector2 pos2, Vector2 pos3)
    {
        float EPS = 1e-3f; //误差
        float d1 = Vector2.Distance(pos1, pos3);
        float d2 = Vector2.Distance(pos2, pos3);
        float d3 = Vector2.Distance(pos1, pos2);
        if (Mathf.Abs(d1 + d2 - d3) <= EPS)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 这个方法是让箭头指向处于屏幕中间的玩家坐标与箭头坐标向量的方向
    /// </summary>
    public static void UILookAt(Transform ctrlObj, Vector3 dir, Vector3 lookAxis) {
        Quaternion quaternion = Quaternion.identity;
        quaternion.SetFromToRotation(lookAxis, dir);
        ctrlObj.eulerAngles = new Vector3(quaternion.eulerAngles.x, 0, quaternion.eulerAngles.z);
    }
}