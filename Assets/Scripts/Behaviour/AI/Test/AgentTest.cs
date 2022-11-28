using System.Collections;
using AKIRA.AI;
using UnityEngine;

public class AgentTest : HumanAgentBase {
    [CNName("巡逻父节点")]
    public Transform patrolRootTransform;
    public float viewRadius = 8.0f;      // 代表视野最远的距离
    public float viewAngleStep = 30;     // 射线数量，越大就越密集，效果更好但硬件耗费越大。

    public override int order => throw new System.NotImplementedException();

    private void Start() {
        machine.RegistState(new AiPatrolState(patrolRootTransform));
        machine.RegistState(new AiChaseState());
        machine.RegistState(new AiIdleState());
        machine.ChangeState(initializeState);

        this.Regist();
    }

    private void OnDrawGizmos() {
        if (config == null)
            config = AiDefaultConfig.DefaultPath.Load<AiDefaultConfig>();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, config.chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, config.attckDistance);
    }

    public override void GameUpdate() {
        base.GameUpdate();
        // 获得最左边那条射线的向量，相对正前方，角度是-45
        Vector3 forward_left = Quaternion.Euler(0, -45, 0) * transform.forward * viewRadius;
        // 依次处理每一条射线
        for (int i = 0; i <= viewAngleStep; i++) {
            // 每条射线都在forward_left的基础上偏转一点，最后一个正好偏转90度到视线最右侧
            Vector3 v = Quaternion.Euler(0, (90.0f / viewAngleStep) * i, 0) * forward_left;
            // Player位置加v，就是射线终点pos
            Vector3 pos = transform.position + v;
            // 从玩家位置到pos画线段，只会在编辑器里看到
            Debug.DrawLine(transform.position, pos, Color.red);
        }
    }

    public override IEnumerator Load()
    {
        throw new System.NotImplementedException();
    }

    public override void Wake()
    {
        throw new System.NotImplementedException();
    }

    public override void Recycle()
    {
        throw new System.NotImplementedException();
    }
}