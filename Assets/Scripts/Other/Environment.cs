using System.Linq;
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
        /// 隐藏所有摄像机时开启的摄像机
        /// </summary>
        /// <returns></returns>
        private static List<CinemachineVirtualCameraBase> ActiveCameras = new();

        /// <summary>
        /// 获得场景所有类型的虚拟摄像机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetCameras<T>() where T : CinemachineVirtualCameraBase {
            return cameras.Where(camera => camera is T).ToArray() as T[];
        }

        /// <summary>
        /// 获得摄像机
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCamera<T>() where T : CinemachineVirtualCameraBase {
            return cameras.First(camera => camera is T) as T;
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

        /// <summary>
        /// 开启上次关闭时激活的摄像机
        /// </summary>
        public static void EnableCameras() {
            cameras.ToList().ForEach(camera => {
                camera.enabled = ActiveCameras.Contains(camera);
            });
            ActiveCameras.Clear();
        }

        /// <summary>
        /// 关闭所有摄像机，并记录开启的摄像机
        /// </summary>
        public static void DisableCameras() {
            ActiveCameras.Clear();
            cameras.Where(camera => camera.enabled).ToList().ForEach(camera => {
                ActiveCameras.Add(camera);
                camera.enabled = false;
            });
        }
        #endregion
    }
}