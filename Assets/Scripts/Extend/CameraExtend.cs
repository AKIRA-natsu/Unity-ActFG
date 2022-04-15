using System;
using System.Collections;
using AKIRA.Coroutine;
// using DG.Tweening;
using UnityEngine;

public static class CameraExtend {
    private static Transform mainCamera;
    /// <summary>
    /// 主摄像机
    /// </summary>
    /// <value></value>
    public static Transform MainCamera {
        get {
            if (mainCamera == null)
                mainCamera = Camera.main.transform;
            return mainCamera;
        }
    }
    /// <summary>
    /// 摄像机是否注视look at target
    /// </summary>
    /// <value></value>
    public static bool LookAt { get; private set; } = false;

    /// <summary>
    /// <para>移动摄像机</para>
    /// <para>DoTween，协程，震动</para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="tar"></param>
    /// <param name="before">摄像机移动前，震动前</param>
    /// <param name="middle">摄像机移动到目标后</param>
    /// <param name="after">摄像机回到初始位置</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CameraLookAt<T>(this T com, Transform tar, Action before = null, Action middle = null, Action after = null) {
        CoroutineManager.Instance.Start(CameraMove(tar, before, middle, after));
        return com;
    }

    /// <summary>
    /// 移动摄像头
    /// </summary>
    /// <param name="tar"></param>
    /// <param name="before"></param>
    /// <param name="middle"></param>
    /// <param name="after"></param>
    /// <returns></returns>
    private static IEnumerator CameraMove(Transform tar, Action before, Action middle, Action after) {
        LookAt = true;
        yield return 0;
        var origin = MainCamera.position;
        before?.Invoke();
        // 摄像机移动震动
        // VibratorManager.Trigger(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
        // MainCamera.DOMove(MainCamera.position - (Extend.Player.position - tar.position), 1f).OnComplete(() => {
        //     middle?.Invoke();
        // });
        yield return new AKIRA.Coroutine.WaitForSeconds(2f);
        // MainCamera.DOMove(origin, 1f).OnComplete(() => {
        //     LookAt = false;
        //     after?.Invoke();
        // });
    }
}