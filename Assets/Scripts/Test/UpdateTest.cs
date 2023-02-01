using AKIRA.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 更新，引用池测试脚本
/// </summary>
namespace AKIRA.Test {
    public class UpdateTest : MonoBehaviour, IUpdate {
        public UpdateMode mode;

        private void OnEnable() {
            this.Regist();
        }

        private void OnDisable() {
            this.Remove();
        }

        public void GameUpdate() {
            var board = Keyboard.current;
            if (board.numpad1Key.wasPressedThisFrame) {
                this.Attach<UpdateClass>();
            }
            if (board.numpad2Key.wasPressedThisFrame) {
                this.Detach<UpdateClass>();
            }

            if (board.numpad4Key.wasPressedThisFrame) {
                this.Attach<UpdateIntervalClass>();
            }
            if (board.numpad5Key.wasPressedThisFrame) {
                this.Detach<UpdateIntervalClass>();
            }

            if (board.numpad7Key.wasPressedThisFrame) {
                UpdateClass.Key.EnableGroupUpdate(true);
            }
            if (board.numpad8Key.wasPressedThisFrame) {
                UpdateClass.Key.EnableGroupUpdate(false);
            }

            if (board.numpad0Key.wasPressedThisFrame) {
                UpdateManager.Instance.DetachGroup(UpdateIntervalClass.Key);
            }
        }
    }

    public class UpdateClass : ReferenceBase, IUpdate {
        public UpdateClass() {}

        private UpdateMode mode = UpdateMode.FixedUpdate;
        public const string Key = "UpdateClass";

        public void GameUpdate() {
            "这是更新测试".Log();
        }

        public override void Wake() {
            base.Wake();
            this.Regist(Key, mode);
        }

        public override void Recycle() {
            base.Recycle();
            this.Remove(Key, mode);
        }
    }

    public class UpdateIntervalClass : ReferenceBase, IUpdate {
        public UpdateIntervalClass() {}

        public const string Key = "UpdateIntervalClass";

        public void GameUpdate() {
            "这是延迟3s更新测试".Log();
        }

        public override void Wake() {
            base.Wake();
            this.Regist(3f, Key);
        }

        public override void Recycle() {
            base.Recycle();
            this.RemoveSpaceUpdate(Key);
        }
    }
}