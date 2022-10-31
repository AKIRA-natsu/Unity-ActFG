using UnityEngine;

public class AgentTest : AiAgent {
    [CNName("巡逻父节点")]
    public Transform patrolRootTransform;

    private void Start() {
        machine.RegistState(new AiPatrolState(patrolRootTransform));
        machine.RegistState(new AiChaseState());
        machine.RegistState(new AiIdleState());
        machine.ChangeState(initializeState);
    }

    private void OnDrawGizmos() {
        if (config == null)
            config = AiDefaultConfig.DefaultPath.Load<AiDefaultConfig>();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, config.chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, config.attckDistance);
    }
}