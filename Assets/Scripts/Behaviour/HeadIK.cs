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
    // 目标原来位置
    private Vector3 originPos;

    private void Awake() {
        animator = this.GetComponent<Animator>();
        originPos = headLookPoint.position;
    }

    private void OnAnimatorIK(int layerIndex) {
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(headLookPoint.position);
    }
}
