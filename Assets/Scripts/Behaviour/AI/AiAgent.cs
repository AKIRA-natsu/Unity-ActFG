using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class AiAgent : AIBase {
    // AI
    public NavMeshAgent navMeshAgent { get; private set; }
    /// <summary>
    /// 实际模型物体
    /// </summary>
    /// <value></value>
    public Transform Render { get; protected set; }
    /// <summary>
    /// 到达终点
    /// </summary>
    public bool Reach => !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

    /// <summary>
    ///  状态机
    /// </summary>
    /// <value></value>
    public AiStateMachine machine { get; private set; }
    /// <summary>
    /// 动画控制器
    /// </summary>
    /// <value></value>
    public AiBehaviour animator { get; private set; }

    [SerializeField]
    protected AiState initializeState = AiState.Idle;

    /// <summary>
    /// 一些统一的设置
    /// </summary>
    public AiDefaultConfig config;

    protected virtual void Awake() {
        Render = this.transform.Find("Render");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        machine = new AiStateMachine(this);
        animator = new AiBehaviour(Render.GetComponentInChildren<Animator>());

        // 如果不设置就设置默认
        if (config == null)
            config = AiDefaultConfig.DefaultPath.Load<AiDefaultConfig>();
    }

    private void OnEnable() {
        this.Regist();
    }

    private void OnDisable() {
        this.Remove();
    }

    public override void GameUpdate() {
        machine.GameUpdate();
    }
}