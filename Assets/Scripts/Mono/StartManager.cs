using UnityEngine;

namespace AKIRA.Manager {
    public class StartManager : MonoBehaviour {

        private void Awake() {
            UIManager.Instance.Initialize();
        }

        private void Start() {
        }

        private void Update() {

        }
    }
}