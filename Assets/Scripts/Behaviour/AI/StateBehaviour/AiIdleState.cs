using UnityEngine;

/// <summary>
/// AI等待状态
/// </summary>
public class AiIdleState : IAiState {
    public float time;

    public void Enter(AiAgent agent) {
        time = 0f;
    }

    public void Exit(AiAgent agent) { }

    public void GameUpdate(AiAgent agent) {
        if (Physics.CheckSphere(agent.transform.position, agent.config.chaseDistance, 1 << Layer.Character)) {
            agent.machine.ChangeState(AiState.Chase);
            return;
        }

        agent.animator.Walk(agent.Reach ? 0f : 1f);
        time += Time.deltaTime;
        if (time <= agent.config.alertTime)
            return;
        
        agent.machine.ChangeState(AiState.Patrol);
    }

    public AiState GetStateID() {
        return AiState.Idle;
    }
}