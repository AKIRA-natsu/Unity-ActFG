using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraStackAdditionHelp : MonoBehaviour {
    
    private void Start() {
        StartCoroutine(WaitToAddMainCamera());
    }

    /// <summary>
    /// 等待添加UI摄像机到主摄像机
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToAddMainCamera() {
        while (!CameraExtend.ExistMainCamera)
            yield return null;

        var data = CameraExtend.MainCamera.GetUniversalAdditionalCameraData();
        data.cameraStack.Add(this.GetComponent<Camera>());
    }
}