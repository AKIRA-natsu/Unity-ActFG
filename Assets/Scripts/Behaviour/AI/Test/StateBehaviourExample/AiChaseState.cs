using UnityEngine;
using AKIRA.AI.StateMachine;
using AKIRA.AI;

/// <summary>
/// 追随
/// </summary>
public class AiChaseState : IAiState {
    // 追随目标
    public Transform chaseTarget;

    // 跟随时间
    private float chaseTime = 0f;

    public void Enter(HumanAgentBase agent) {
        agent.navMeshAgent.speed = agent.config.runSpeed;
        var colliders = Physics.OverlapSphere(agent.transform.position, agent.config.chaseDistance, 1 << Layer.Character);
        if (colliders.Length == 0) {
            agent.machine.ChangeState(AiState.Patrol);
        } else {
            chaseTime = 0f;
            chaseTarget = colliders[0].transform;
            agent.navMeshAgent.SetDestination(chaseTarget.position);
        }
    }

    public void Exit(HumanAgentBase agent) { }

    public void GameUpdate(HumanAgentBase agent) {
        agent.aiAnimation.Walk(agent.Reach ? 0 : 1);

        var distance = Vector3.Distance(agent.transform.position, chaseTarget.position);
        if (distance > agent.config.chaseDistance) {
            agent.machine.ChangeState(AiState.Idle);
            return;
        }

        if (distance <= agent.config.attckDistance)  {
            // agent.machine.ChangeState(AiState.Attack);
            return;
        }

        chaseTime += Time.deltaTime;
        if (chaseTime < agent.config.chaseWaitTime)
            return;

        agent.navMeshAgent.SetDestination(chaseTarget.position);
    }

    public AiState GetStateID() {
        return AiState.Chase;
    }
}