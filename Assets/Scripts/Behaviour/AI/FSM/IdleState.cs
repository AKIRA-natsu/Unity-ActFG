using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 巡逻状态
    /// </summary>
    public class IdleState : IState {
        public AIState State => AIState.Idle;

        public AIState Enter() {
            return State;
        }

        public void Exit() { }

        public void GameUpdate(FSMMachine machine) {
            machine.TransitionState(AIState.Partol);
        }
    }
}