using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 新的输入系统
/// </summary>
public static class InputHelp {
    /// <summary>
    /// 是否按下
    /// </summary>
    public static bool isPressed =>
#if UNITY_EDITOR
        Mouse.current.leftButton.isPressed;
#else
        Touch.activeTouches.Count > 0;
#endif

    /// <summary>
    /// 按下位置
    /// </summary>
    public static Vector2 inputPosition =>
#if UNITY_EDITOR
        Mouse.current.position.ReadValue();
#else
        Touch.activeTouches[0].screenPosition;
#endif
}