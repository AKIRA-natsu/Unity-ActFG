using UnityEngine;

/// <summary>
/// 动画
/// </summary>
public class AiBehaviour {
    // 动画控制器
    private Animator animator;

    // Hash
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private readonly int ClimbHash = Animator.StringToHash("Climb");
    private readonly int ClimbToTopHash = Animator.StringToHash("ClimbToTop");

    public AiBehaviour(Animator animator) => this.animator = animator;

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="value"></param>
    public virtual void Walk(float value) {
        animator.SetFloat(SpeedHash,
            Mathf.Lerp(animator.GetFloat(SpeedHash), value, Time.deltaTime * 10f));
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public virtual void Jump() {
        animator.SetTrigger(JumpHash);
    }

    /// <summary>
    /// 攀爬
    /// </summary>
    /// <param name="climb"></param>
    public virtual void Climb(bool climb) {
        animator.SetBool(ClimbHash, climb);
    }

    /// <summary>
    /// 往上爬的特殊处理
    /// </summary>
    /// <param name="climbToTop"></param>
    public virtual void ClimbToTop(bool climbToTop) {
        animator.SetBool(ClimbToTopHash, climbToTop);
    }
}