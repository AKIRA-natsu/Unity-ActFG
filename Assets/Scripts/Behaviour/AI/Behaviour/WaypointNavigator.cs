using UnityEngine;

namespace AKIRA.Behaviour.AI {
    [RequireComponent(typeof(CharacterNavigationController))]
    public class WaypointNavigator : MonoBehaviour, IUpdate {
        private CharacterNavigationController controller;
        public Waypoint currentWaypoint;

        private int direction;

        private void Awake() {
            controller = this.GetComponent<CharacterNavigationController>();
        }

        private void Start() {
            direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            controller.FSM.SetDestination(currentWaypoint.GetPosition());
            this.Regist();
        }

        private void OnDisable() {
            this.Remove();
        }

        public void GameUpdate() {
            if (controller.FSM.Reach) {
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

                controller.FSM.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}