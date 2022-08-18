using System;
using System.Collections;
using AKIRA.Coroutine;
using UnityEngine;

public static class CameraExtend {
    private static Camera mainCamera;
    /// <summary>
    /// 主摄像机
    /// </summary>
    /// <value></value>
    public static Camera MainCamera {
        get {
            if (mainCamera == null)
                mainCamera = Camera.main.GetComponent<Camera>();
            return mainCamera;
        }
    }
    /// <summary>
    /// 摄像机是否注视look at target
    /// </summary>
    /// <value></value>
    public static bool LookAt { get; private set; } = false;
}