using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// AI控制器
    /// </summary>
    public class CharacterNavigationController : AIBase {
        [SerializeField]
        private FSMMachine fsm;
        /// <summary>
        /// 所持状态机
        /// </summary>
        public FSMMachine FSM => fsm;

        // // 视野内的侵略者
        // private GameObject invader;
        // // 移动速度
        // public float moveSpeed = 1f;
        // // 旋转速度
        // public float turnSpeed = 1f;

        public override void Recycle() {
            this.Remove(Group, mode);
        }

        public override void Wake() {
            this.Regist(Group, mode);
        }
        
        public override void GameUpdate() {
            if (fsm != null) {
                fsm.GameUpdate(out object data);
                Animation?.SwitchAnima(fsm.curState, data);
            }

        //     // FIXME: 测试视野
        //     if (this.TryGetComponent<FieldView>(out FieldView view)) {
        //         if (view.ViewRayHit(out RaycastHit hit)) {
        //             hit.transform.Log();
        //         }
        //     }
        }

        public override void OnUpdateStop() {
            fsm.Stop();
        }

        public override void OnUpdateResume() {
            fsm.Resume();
        }

        // /// <summary>
        // /// 是否朝向侵略者
        // /// </summary>
        // /// <returns></returns>
        // private bool IsFacingInvader() {
        //     if (invader == null)
        //         return false;
        //     Vector3 v1 = invader.transform.position - this.transform.position;
        //     v1.y = 0;
        //     return Vector3.Angle(this.transform.forward, v1) < 1;
        // }

        // /// <summary>
        // /// 转向侵略者
        // /// </summary>
        // private void RotateToInvader() {
        //     if (invader == null)
        //         return;
        //     Vector3 v1 = invader.transform.position - this.transform.position;
        //     v1.y = 0;
        //     Vector3 cross = Vector3.Cross(this.transform.forward, v1);
        //     float angle = Vector3.Angle(this.transform.forward, v1);
        //     this.transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
        // }
        
        // /// <summary>
        // /// 朝向方向
        // /// </summary>
        // /// <param name="rotation"></param>
        // private void RotateToDirection(Quaternion rotation) {
        //     Quaternion.RotateTowards(this.transform.rotation, rotation, turnSpeed);
        // }

        // /// <summary>
        // /// 是否到达目标
        // /// </summary>
        // /// <param name="position"></param>
        // /// <returns></returns>
        // private bool Reach(Vector3 position) {
        //     Vector3 v = position - this.transform.position;
        //     v.y = 0;
        //     return v.magnitude < 0.05;
        // }

        // /// <summary>
        // /// 移动到目标
        // /// </summary>
        // /// <param name="position"></param>
        // private void MoveToPosition(Vector3 position) {
        //     Vector3 v = position - this.transform.position;
        //     v.y = 0;
        //     this.transform.position += v.normalized * moveSpeed * Time.deltaTime;
        // }

        #region Inspector Methods
        #if UNITY_EDITOR
        /// <summary>
        /// 添加状态机
        /// </summary>
        public void AddFSM() {
            fsm = this.gameObject.AddComponent<FSMMachine>();
            // 加入动画
            this.GetComponentInChildren<Animator>()?.gameObject.AddComponent<AnimatorBehaviour>();
        }

        /// <summary>
        /// 移除状态机
        /// </summary>
        public void RemoveFSM() {
            DestroyImmediate(fsm);
            // 移除动画
            if (Animation != null)
                DestroyImmediate(this.GetComponentInChildren<AnimatorBehaviour>());
            fsm = null;
        }
        #endif
        #endregion
    }
}
