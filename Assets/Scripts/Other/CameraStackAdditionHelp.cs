using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraStackAdditionHelp : MonoBehaviour {
    private void Awake() {
        var data = CameraExtend.MainCamera.GetUniversalAdditionalCameraData();
        data.cameraStack.Add(this.GetComponent<Camera>());
    }
}