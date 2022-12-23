using System;
using AKIRA.AI;
using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour, IAnima
{
    // 控制器
    private Animator animator;

    // 速度hash
    private readonly int speedHash = Animator.StringToHash("Speed");
    // 跳跃hash
    private readonly int jumpHash = Animator.StringToHash("Jump");

    private void Awake() {
        animator = this.GetComponentInChildren<Animator>();
    }

    public void SwitchAnima(AIState state, object data = null)
    {
        switch (state) {
            case AIState.Speed:
                Move(Convert.ToSingle(data));
            break;
            case AIState.Jump:
                Jump();
            break;
            // other states
            default:
            break;
        }
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
    private void Jump() {
        animator.SetTrigger(jumpHash);
    }
}