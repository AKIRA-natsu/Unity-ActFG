using UnityEngine;

/// <summary>
/// AI等待状态
/// </summary>
public class AiIdleState : IAiState {
    public void Enter(AiAgent agent) {
        throw new System.NotImplementedException();
    }

    public void Exit(AiAgent agent) {
        throw new System.NotImplementedException();
    }

    public void GameUpdate(AiAgent agent) {
        throw new System.NotImplementedException();
    }

    public AiState GetStateID() {
        return AiState.Idle;
    }
}