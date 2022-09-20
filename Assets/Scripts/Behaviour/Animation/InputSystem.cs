using System;
using AKIRA.Manager;
using UnityEngine;

public class InputSystem : MonoSingleton<InputSystem> {
    public KeyCode MoveForward = KeyCode.W;
    public KeyCode MoveBehand = KeyCode.S;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode Run = KeyCode.LeftShift;
    public KeyCode Jump = KeyCode.Space;
    public KeyCode CursorState = KeyCode.LeftAlt;

    // 移动事件
    private Action<Vector3> onMove;
    // 跳跃事件
    private Action onJump;
    // 移动方向
    private Vector3 moveDir;

    /// <summary>
    /// 是否按住跑步
    /// </summary>
    /// <returns></returns>
    public bool Faster => Input.GetKey(Run);

    /// <summary>
    /// 注册移动事件
    /// </summary>
    /// <param name="onMove"></param>
    public void RegistMoveAction(Action<Vector3> onMove) {
        this.onMove += onMove;
    }

    /// <summary>
    /// 注册跳跃事件
    /// </summary>
    /// <param name="onJump"></param>
    public void RegistJumpAction(Action onJump) {
        this.onJump += onJump;
    }

    private void Update() {
        if (Input.GetKeyDown(Jump))
            onJump?.Invoke();
        
        Cursor.lockState = Input.GetKey(CursorState) ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    private void FixedUpdate() {
        moveDir = Vector3.zero;
        if (Input.GetKey(MoveForward)) {
            moveDir += Vector3.forward;
        }
        if (Input.GetKey(MoveBehand)) {
            moveDir += Vector3.back;
        }
        if (Input.GetKey(MoveLeft)) {
            moveDir += Vector3.left;
        }
        if (Input.GetKey(MoveRight)) {
            moveDir += Vector3.right;
        }

        // 相对摄像机的前后左右
        moveDir = Quaternion.Euler(0, CameraExtend.MainCamera.transform.localEulerAngles.y, 0) * new Vector3(moveDir.x, 0, moveDir.z);

        onMove?.Invoke(moveDir);
    }

}