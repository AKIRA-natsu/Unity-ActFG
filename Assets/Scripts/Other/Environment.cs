using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

namespace AKIRA.Behaviour.Prepare {
    /// <summary>
    /// 场景环境
    /// </summary>
    public class Environment : MonoBehaviour {
        // 后处理
        private Volume volume;
        // 虚拟摄像机
        private static CinemachineVirtualCameraBase[] cameras;

        // 是否开启后处理
        [SerializeField]
        private bool enableVolume;

        private void Awake() {
            volume = this.GetComponentInChildren<Volume>();
            cameras = this.GetComponentsInChildren<CinemachineVirtualCameraBase>();

            // 统一关闭所有虚拟摄像机
            foreach (var camera in cameras)
                camera.gameObject.SetActive(false);
        }

        private void Start() {
            volume?.gameObject.SetActive(enableVolume);
        }

#region 虚拟摄像机相关
        /// <summary>
        /// 获得场景所有虚拟摄像机
        /// </summary>
        /// <returns></returns>
        public static CinemachineVirtualCameraBase[] GetCameras() => cameras;

        /// <summary>
        /// 获得场景所有类型的虚拟摄像机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetCameras<T>() where T : CinemachineVirtualCameraBase {
            List<T> result = new();
            foreach (var camera in cameras) {
                if (camera is T)
                    result.Add(camera as T);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获得摄像机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCamera<T>() where T : CinemachineVirtualCameraBase {
            foreach (var camera in cameras) {
                if (camera is T)
                    return camera as T;
            }
            return default;
        }

        /// <summary>
        /// 尝试获得虚拟摄像机
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetCamera<T>(out T value) where T : CinemachineVirtualCameraBase {
            value = GetCamera<T>();
            return value != default;
        }
    }
#endregion
}