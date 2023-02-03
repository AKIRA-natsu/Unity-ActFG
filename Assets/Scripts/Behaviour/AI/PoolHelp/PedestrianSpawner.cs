using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Behaviour.AI {
    public class PedestrianSpawner : MonoBehaviour {
        public GameObject pedestrianPrefab;
        public int pedestriansToSpawn;

        private void Start() {
            this.UniRepeat(Spawn, pedestriansToSpawn, 1f);
        }

        private void Spawn(int index) {
            GameObject obj = ObjectPool.Instance.Instantiate(pedestrianPrefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
        }
    }
}
