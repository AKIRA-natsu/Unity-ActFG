using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Behaviour.Prepare {
    public class GamePrepare : MonoBehaviour {
        // 管理器
        [SerializeField]
        private GameObject[] managers;

        // 场景初始化物体
        [SerializeField]
        private GameObject[] sceneInitedObjs;

        // 测试物体
        [SerializeField]
        private GameObject[] testObjs;

        // manager Dont Destory
        [Space]
        [SerializeField]
        private bool managerAsDontDestory = false;

        // 场景初始化物体 Dont Destory
        [SerializeField]
        private bool sceneAsDontDestory = false;

        // 完成所有Instantiate后切换的Game状态
        [Space]
        [SerializeField]
        private GameState calledEndState = GameState.None;

        private void Start() {
            // Call Managers
            if (managers.Length != 0) {
                GameObject managerRoot = new GameObject("[Managers]");
                if (managerAsDontDestory)
                    managerRoot.DontDestory();
                foreach (var manager in managers)
                    manager.Instantiate().SetParent(managerRoot);
            }
            // Call Scene Inited GameObjects
            if (sceneInitedObjs.Length != 0) {
                GameObject sceneObjRoot = new GameObject("[Base]");
                if (sceneAsDontDestory)
                    sceneObjRoot.DontDestory();
                foreach (var obj in sceneInitedObjs)
                    obj.Instantiate().SetParent(sceneObjRoot);
            }
            // Call Test Objs Active After Scene Inited
            if (testObjs.Length != 0) {
                GameObject TestRoot = new GameObject("[Tests]");
                foreach (var obj in testObjs)
                    obj.SetParent(TestRoot).SetActive(true);
            }

            if (GameManager.Instance != null) 
                GameManager.Instance.Switch(calledEndState);

            // 销毁自身
            Destroy(this.gameObject);
        }
    }
}