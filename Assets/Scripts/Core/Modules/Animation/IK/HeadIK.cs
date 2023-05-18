using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 要挂在Animator同一物体下
/// </summary>
public class HeadIK : MonoBehaviour {
    // 控制器
    private Animator animator;
    // 头看向目标
    public Transform headLookPoint;
    // 是否启用
    public bool acitve = true;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        if (!acitve || headLookPoint == null) {
            animator.SetLookAtWeight(0f);
        } else {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(headLookPoint.position);
        }
    }
}
