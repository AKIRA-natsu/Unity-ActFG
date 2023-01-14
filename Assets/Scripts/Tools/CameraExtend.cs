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
    public static bool ExistMainCamera => mainCamera != null;

    /// <summary>
    /// 主摄像机下的脚本
    /// </summary>
    /// <typeparam name="CameraBehaviour"></typeparam>
    /// <returns></returns>
    private static Dictionary<Type, CameraBehaviour> CameraBehaviourMap = new Dictionary<Type, CameraBehaviour>();

    /// <summary>
    /// 获得主摄像机下的脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetBehaviour<T>() where T : CameraBehaviour {
        var type = typeof(T);
        if (CameraBehaviourMap.ContainsKey(type))
            return CameraBehaviourMap[type] as T;
        else {
            var behaviour = MainCamera.GetComponent<T>();
            CameraBehaviourMap.Add(type, behaviour);
            return behaviour;
        }
    }
}