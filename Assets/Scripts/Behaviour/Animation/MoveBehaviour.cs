using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {
    // 控制器
    private Animator animator;

    // 速度hash
    private readonly int speedHash = Animator.StringToHash("Speed");
    // 跳跃hash
    private readonly int jumpHash = Animator.StringToHash("Jump");

    // 移动方向存储 差值
    private Vector3 moveDir;
    // 移动速度
    public float speed = 1f;
    /// <summary>
    /// 移动速度 跑步加倍
    /// </summary>
    public float Speed => InputSystem.Instance.Faster ? speed * 2f : speed;

    private void Awake() {
        animator = this.GetComponentInChildren<Animator>();
    }

    private void Start() {
        // 注册移动
        InputSystem.Instance.RegistMoveAction(Move);
        InputSystem.Instance.RegistJumpAction(Jump);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(this.transform.position, -this.transform.up);
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="dir"></param>
    private void Move(Vector3 dir) {
        if (dir.magnitude == 0f)
            moveDir = Vector3.zero;
        else
            moveDir = Vector3.Lerp(moveDir, dir, Time.deltaTime * 10f);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir.Equals(Vector3.zero) ? this.transform.forward : moveDir), Time.deltaTime * 10f);
        // 如果转向小于50才移动
        if (Vector3.Angle(this.transform.forward, moveDir.normalized) <= 50f) {
            this.transform.Translate(moveDir * Speed * Time.deltaTime, Space.World);
            Move(moveDir.Equals(Vector3.zero) ? 0 : InputSystem.Instance.Faster ? 1f : 0.5f);
        } else
            Move(0f);
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
