using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 巡逻状态
    /// </summary>
    public class PartolState : IState {
        public AIState State => AIState.Partol;

        // 是否已经设置初始位置
        private bool hasSetOriginPosition = false;
        // 初始位置
        private Vector3 originPosition;

        /// <summary>
        /// 是否已经设置目标点
        /// </summary>
        private bool hasSetTarget = false;

        /// <summary>
        /// 当前路径点
        /// </summary>
        public Waypoint currentWaypoint;
        /// <summary>
        /// 方向选择
        /// </summary>
        private int direction;

        public AIState Enter() {
            return State;
        }

        public void Exit() {
            hasSetTarget = false;
        }

        public void GameUpdate(FSMMachine machine) {
            var config = machine.Config;
            // 随机周围点巡逻
            if (config.partolRandom) {
                if (!hasSetOriginPosition) {
                    hasSetOriginPosition = true;
                    originPosition = machine.transform.position;
                }

                if (!hasSetTarget) {
                    hasSetTarget = true;
                    // 结束等待，重新设置巡逻点
                    if (TryGetNavWayPoint(config, out Vector3 position)) {
                        machine.SetDestination(position);
                    } else {
                        machine.SetDestination(machine.transform.position);
                    }
                }

                if (hasSetTarget && machine.Reach) {
                    machine.TransitionState(AIState.Wait);
                }
            } else {
                if (currentWaypoint == null) {
                    direction = Mathf.RoundToInt(Random.value);
                    currentWaypoint = WayPath.GetPath(machine.wayTag).GetRandomPoint();
                }

                if (machine.Reach) {
                    bool shouldBranch = false;

                    if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0) {
                        shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio;
                    }

                    if (shouldBranch) {
                        currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
                    } else {
                        if (direction == 0) {
                            if (currentWaypoint.nextWaypoint != null) {
                                currentWaypoint = currentWaypoint.nextWaypoint;
                            } else {
                                currentWaypoint = currentWaypoint.previousWaypoint;
                                direction = 1;
                            }
                        } else if (direction == 1) {
                            if (currentWaypoint.previousWaypoint != null) {
                                currentWaypoint = currentWaypoint.previousWaypoint;
                            } else {
                                currentWaypoint = currentWaypoint.nextWaypoint;
                                direction = 0;
                            }
                        }
                    }

                    machine.SetDestination(currentWaypoint.GetPosition());
                }
            }
        }

        /// <summary>
        /// 获得随机巡逻点
        /// </summary>
        private bool TryGetNavWayPoint(AIDataConfig config, out Vector3 targetPosition) {
            var radius = config.partolRadius;
            var randomX = Random.Range(-radius, radius);
            var randomZ = Random.Range(-radius, radius);
            targetPosition = new Vector3(originPosition.x + randomX, originPosition.y, originPosition.z + randomZ);

            NavMeshHit hit;
            return NavMesh.SamplePosition(targetPosition, out hit, radius, 1);
        }


    }
}