using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Behaviour.AI {
    public class PedestrianSpawner : MonoBehaviour {
        public GameObject pedestrianPrefab;
        public int pedestriansToSpawn;

        private async void Start() {
            for (int i = 0; i < pedestriansToSpawn; i++) {
                Spawn(i);
                await UniTask.Delay(1000);
            }
        }

        private void Spawn(int index) {
            GameObject obj = ObjectPool.Instance.Instantiate(pedestrianPrefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
        }
    }
}
