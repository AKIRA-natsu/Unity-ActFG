using UnityEngine;

/// <summary>
/// 动画
/// </summary>
public abstract class AIBehaviour : AIBase {
    // Hash
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private readonly int ClimbHash = Animator.StringToHash("Climb");
    private readonly int ClimbToTopHash = Animator.StringToHash("ClimbToTop");

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="value"></param>
    protected virtual void Walk(float value) {
        animator.SetFloat(SpeedHash,
            Mathf.Lerp(animator.GetFloat(SpeedHash), value, Time.deltaTime * 10f));
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    protected virtual void Jump() {
        animator.SetTrigger(JumpHash);
    }

    /// <summary>
    /// 攀爬
    /// </summary>
    /// <param name="climb"></param>
    protected virtual void Climb(bool climb) {
        animator.SetBool(ClimbHash, climb);
    }

    /// <summary>
    /// 往上爬的特殊处理
    /// </summary>
    /// <param name="climbToTop"></param>
    protected virtual void ClimbToTop(bool climbToTop) {
        animator.SetBool(ClimbToTopHash, climbToTop);
    }
}