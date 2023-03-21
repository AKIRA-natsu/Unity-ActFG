using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 巡逻状态
    /// </summary>
    public class WaitState : IState {
        public AIState State => AIState.Wait;

        // 等待时间
        private float lastUpdateTime;

        public AIState Enter() {
            lastUpdateTime = Time.time;
            return State;
        }

        public void Exit() { }

        public void GameUpdate(FSMMachine machine) {
            if (Time.time - lastUpdateTime <= machine.Config.waitTime) {
                return;
            }

            machine.TransitionState(AIState.Partol);
        }
    }
}