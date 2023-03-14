using System;
using AKIRA.Behaviour.AI;
using UnityEngine;

/// <summary>
/// 动画控制器
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorBehaviour : MonoBehaviour, IAnima {
    // 控制器
    private Animator animator;

    // 速度hash
    private readonly int speedHash = Animator.StringToHash("Speed");
    // 跳跃hash
    private readonly int jumpHash = Animator.StringToHash("Jump");

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    public void SwitchAnima(AIState state, object data = null) {
        switch (state) {
            case AIState.Speed:
                Move(Convert.ToSingle(data));
            break;
            case AIState.Jump:
                Jump((NextPlayerMovement)data);
            break;
            // other states
            default:
            break;
        }
    }

    /// <summary>
    /// 切换动画，CrossFade
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="time"></param>
    protected void SwitchAnima(int hash, float time = 0.2f) {
        animator.CrossFade(hash, time);
    }

    /// <summary>
    /// 移动 设置动画
    /// </summary>
    /// <param name="value"></param>
    private void Move(float value) {
        animator.SetFloat(speedHash, Mathf.Lerp(animator.GetFloat(speedHash), value, Time.deltaTime * 10f));
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump(NextPlayerMovement movement) {
        switch (movement) {
            case NextPlayerMovement.Jump:
                animator.SetTrigger(jumpHash);
            break;
            case NextPlayerMovement.ClimbLow:
            break;
            case NextPlayerMovement.ClimbHeight:
            break;
        }
    }

    private void OnAnimatorIK(int layerIndex) {
        // 矫正双手
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    }
}