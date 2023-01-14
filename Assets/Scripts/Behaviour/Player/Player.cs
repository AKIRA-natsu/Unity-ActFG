using AKIRA.AI;
using AKIRA.Manager;
using AKIRA.Behaviour.Prepare;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : AIBase {
    // 更新方式
    [SerializeField]
    private UpdateMode mode;
    // 移动方向存储 差值
    private Vector3 moveDir;
    // 移动速度
    [SerializeField]
    private float speed = 1f;
    /// <summary>
    /// 移动速度 跑步加倍
    /// </summary>
    public float Speed => run ? speed * 2f : speed;
    private bool run = false;
    /// <summary>
    /// 输入系统 C#类
    /// </summary>
    private PlayerInputAction InputActions
        => PlayerInputSystem.Instance.InputActions;

    private void Start() {
        var inputSystem = PlayerInputSystem.Instance;
        inputSystem.RegistOnInputSwitchPlayer(() => this.Regist(mode));
        inputSystem.RegistOnInputSwitchUI(() => this.Remove(mode));
        InputActions.Player.Run.performed += OnRunPreformed;
        InputActions.Player.Jump.performed += OnJumpPreformed;
        
        this.Regist(mode);

        // 主摄像机参数设置
        var camera = Environment.GetCamera<CinemachineFreeLook>();
        camera.gameObject.SetActive(true);
        camera.LookAt = this.transform;
        camera.Follow = this.transform;
        // Middle Rig
        camera.GetRig(1).LookAt = this.transform;
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    /// <param name="context"></param>
    private void OnJumpPreformed(InputAction.CallbackContext context)
    {
        Animation.SwitchAnima(AIState.Jump);
    }

    /// <summary>
    /// 跑步
    /// </summary>
    /// <param name="context"></param>
    private void OnRunPreformed(InputAction.CallbackContext context)
    {
        if (context.performed)
            run = !run;
    }

    public override void GameUpdate() {
        Move(InputActions.Player.Move.ReadValue<Vector2>());
    }

    public override void Wake() { }
    public override void Recycle() { }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="inputPosition"></param>
    private void Move(Vector2 inputPosition) {
        Vector3 dir = new Vector3(inputPosition.x, 0f, inputPosition.y);
        if (dir.sqrMagnitude == 0f)
            moveDir = Vector3.zero;
        else
            moveDir = Vector3.Lerp(moveDir, dir, Time.deltaTime * 10f);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir.Equals(Vector3.zero) ? this.transform.forward : moveDir), Time.deltaTime * 10f);
        // 如果转向小于50才移动
        if (Vector3.Angle(this.transform.forward, moveDir.normalized) <= 50f) {
            this.transform.Translate(moveDir * Speed * Time.deltaTime, Space.World);
            Animation.SwitchAnima(AIState.Speed, moveDir.Equals(Vector3.zero) ? 0 : run ? 1f : 0.5f);
        } else
            Animation.SwitchAnima(AIState.Speed, 0);
    }
}