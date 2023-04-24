using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机扩展
/// </summary>
public static class CameraExtend {
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

    /// <summary>
    /// Transform
    /// </summary>
    public static Transform Transform => MainCamera.transform;

    /// <summary>
    /// 是否存在主摄像机
    /// </summary>
    public static bool ExistMainCamera => MainCamera != null;
}