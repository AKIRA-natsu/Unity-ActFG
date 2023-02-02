using AKIRA.Behaviour.Prepare;
using Cinemachine;
using UnityEngine;

/// <summary>
/// 晚上身体摄像机跟随点
/// </summary>
public class PlayerCameraBodyPoint : MonoBehaviour {
    private void Start() {
        // 主摄像机参数设置
        var camera = Environment.GetCamera<CinemachineFreeLook>();
        camera.gameObject.SetActive(true);
        camera.LookAt = this.transform;
        camera.Follow = this.transform.parent;
        // Middle Rig
        camera.GetRig(1).LookAt = this.transform;
    }
}