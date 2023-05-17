using UnityEngine;

namespace AKIRA.Behaviour.Camera {
    /// <summary>
    /// 摄像机表现基类
    /// </summary>
    public abstract class CameraBehaviour : MonoBehaviour, IUpdate {
        // 更新模式
        [SerializeField]
        protected UpdateMode mode = UpdateMode.Update;
        // 摄像机更新组
        public const string CameraGroup = "Camera";

        protected virtual void Awake() {
            CameraExtend.RegistCameraBehaviour(this);
        }

        protected virtual void OnEnable() {
            this.Regist(CameraGroup, mode);
        }

        protected virtual void OnDisable() {
            this.Remove(CameraGroup, mode);
        }

        protected virtual void OnDestroy() {
            CameraExtend.RemoveCameraBehaviour(this);
        }

        public abstract void GameUpdate();
    }
}