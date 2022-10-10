using UnityEngine;
using UnityEngine.AI;
using AKIRA.Manager;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public abstract class AIBase : MonoBehaviour, IPool, IUpdate {
    // 动画控制器
    protected Animator animator;
    // AI
    protected NavMeshAgent agent;
    /// <summary>
    /// 实际模型物体
    /// </summary>
    /// <value></value>
    public Transform Render { get; protected set; }
    /// <summary>
    /// 到达终点
    /// </summary>
    public bool Reach => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

    /// <summary>
    /// 状态机
    /// </summary>
    /// <returns></returns>
    public AIFSM fsm { get; private set; } = new AIFSM();
    /// <summary>
    /// 路线
    /// </summary>
    /// <value></value>
    public PathDebug path { get; private set; }

    protected virtual void Awake() {
        agent = this.GetComponent<NavMeshAgent>();

        Render = this.transform.Find("Render");
        animator = Render.GetComponentInChildren<Animator>();
    }

    public abstract void Wake();
    public abstract void Recycle();
    public abstract void GameUpdate();

    /// <summary>
    /// 创造线路
    /// </summary>
    public void CreatePath() {
        if (path != null)
            return;
        path = ReferencePool.Instance.Instantiate<PathDebug>().Init(this.gameObject);
    }

    /// <summary>
    /// 销毁线路
    /// </summary>
    public void DestoryPath() {
        if (path == null)
            return;
        ReferencePool.Instance.Destory(path);
        path = null;
    }

    /// <summary>
    /// 设置终点
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SetDestination(Vector3 position) {
        return agent.SetDestination(position);
    }
}