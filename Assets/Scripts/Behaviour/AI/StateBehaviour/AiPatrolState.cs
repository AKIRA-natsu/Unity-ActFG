using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>
public class AiPatrolState : IAiState {
    // 巡逻点
    private Vector3[] positions;
    // 当前巡逻目标点键值
    private int curIndex = -1;

    // 等待时间
    private float waitTime = 0f;

    public AiPatrolState(Transform partrolTransParent) {
        var trans = partrolTransParent.GetChildrenComponents<Transform>();
        positions = new Vector3[trans.Length];
        for (int i = 0; i < trans.Length; i++)
            positions[i] = trans[i].position;
    }

    public void Enter(AiAgent agent) {
        agent.navMeshAgent.speed = agent.config.walkSpeed;
        agent.navMeshAgent.SetDestination(GetNextPatrolPosition(agent.config.patrolInOrder));
        waitTime = 0f;
    }

    public void Exit(AiAgent agent) {
        curIndex--;
    }

    public void GameUpdate(AiAgent agent) {
        agent.animator.Walk(agent.Reach ? 0 : 0.5f);

        if (Physics.CheckSphere(agent.transform.position, agent.config.chaseDistance, 1 << Layer.Character)) {
            agent.machine.ChangeState(AiState.Chase);
            return;
        }

        if (!agent.Reach)
            return;
        
        waitTime += Time.deltaTime;
        if (waitTime <= agent.config.patrolWaitTime)
            return;
        
        waitTime = 0;
        agent.navMeshAgent.SetDestination(GetNextPatrolPosition(agent.config.patrolInOrder));
    }

    public AiState GetStateID() {
        return AiState.Patrol;
    }

    /// <summary>
    /// 获得下一个巡逻点
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNextPatrolPosition(bool inOrder) {
        if (inOrder) {
            if (++curIndex >= positions.Length)
                curIndex = 0;
        } else {
            var index = Random.Range(0, positions.Length);
            if (index == curIndex)
                return GetNextPatrolPosition(inOrder);
            curIndex = index;
        }

        return positions[curIndex];
    }
}