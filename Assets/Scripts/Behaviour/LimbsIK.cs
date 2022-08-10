using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 要挂在Animator同一物体下
/// </summary>
public class LimbsIK : MonoBehaviour {
    // 控制器
    private Animator animator;
    // 目标
    public Transform targetPoint;

    // 
    public AvatarIKGoal goal;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        animator.SetIKPositionWeight(goal, 1);
        animator.SetIKPosition(goal, targetPoint.position);
    }
}
