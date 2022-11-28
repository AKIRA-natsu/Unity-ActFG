using UnityEngine;
using AKIRA.AI;
using AKIRA.AI.StateMachine;

/// <summary>
/// AI等待状态
/// </summary>
public class AiIdleState : IAiState {
    public float time;

    public void Enter(HumanAgentBase agent) {
        time = 0f;
    }

    public void Exit(HumanAgentBase agent) { }

    public void GameUpdate(HumanAgentBase agent) {
        if (Physics.CheckSphere(agent.transform.position, agent.config.chaseDistance, 1 << Layer.Character)) {
            agent.machine.ChangeState(AiState.Chase);
            return;
        }

        agent.aiAnimation.Walk(agent.Reach ? 0f : 1f);
        time += Time.deltaTime;
        if (time <= agent.config.alertTime)
            return;
        
        agent.machine.ChangeState(AiState.Patrol);
    }

    public AiState GetStateID() {
        return AiState.Idle;
    }
}