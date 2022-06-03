using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>特殊事件</para>
/// </summary>
public class RegistSpecialAction : MonoBehaviour {
    /// <summary>
    /// 按键
    /// </summary>
    [HideInInspector]
    public KeyCode keyCode;
    /// <summary>
    /// 目标脚本
    /// </summary>
    [HideInInspector]
    public MonoBehaviour target;

    /// <summary>
    /// 目标方法
    /// </summary>
    public MethodInfo methodInfo;

    /// <summary>
    /// 参数值数组
    /// </summary>
    public object[] parameterValues;

    /// <summary>
    /// 方法键值
    /// </summary>
    [HideInInspector]
    public int index = 0;

    private void Update() {
        if (Input.GetKeyDown(keyCode))
            methodInfo.Invoke(target, parameterValues);
    }
}
