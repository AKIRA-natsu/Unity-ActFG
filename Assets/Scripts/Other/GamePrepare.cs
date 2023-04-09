using AKIRA.Manager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Behaviour.Prepare {
    public class GamePrepare : MonoBehaviour {
        // 全局物体
        [SerializeField]
        private GameObject[] globalObjs;

        // 进入游戏的准备物体
        [SerializeField]
        private GameObject[] prepareObjs;

        // 测试物体
        [SerializeField]
        private GameObject[] testObjs;

        // 全局物体 Dont Destory
        [Space]
        [SerializeField]
        private bool globalAsDontDestory = false;

        // 场景初始化物体 Dont Destory
        [SerializeField]
        private bool prepareAsDontDestory = false;

        // 是否直接进入游戏
        [SerializeField]
        private bool enterGameDirect = false;

        // 完成所有Instantiate后切换的Game状态
        [Space]
        [SerializeField]
        private GameState calledEndState = GameState.None;

        private void Start() {
            // regist game action first
            GamePrepareManager.Instance.RegistOnGameEnter(EnterGame);
            GamePrepareManager.Instance.RegistOnGameExit(ExitGame);

            // Call Managers
            if (globalObjs.Length != 0) {
                GameObject GlobalRoot = new GameObject("[Global]");
                if (globalAsDontDestory)
                    GlobalRoot.DontDestory();
                foreach (var obj in globalObjs)
                    obj.Instantiate().SetParent(GlobalRoot);
            }

            if (enterGameDirect) {
                GamePrepareManager.Instance.EnterGame();
                PlayerInputSystem.Instance.SwtichPlayer();
            } else {
                PlayerInputSystem.Instance.SwitchUI();
            }
        }

        /// <summary>
        /// 进入游戏
        /// </summary>
        private async UniTask EnterGame() {
            // Call Config Objs Active After Scene Inited
            if (prepareObjs.Length != 0) {
                if (!GameObject.Find("[Base]")) {
                    GameObject PrepareRoot = new GameObject("[Base]");
                    if (prepareAsDontDestory)
                        PrepareRoot.DontDestory();
                    foreach (var obj in prepareObjs)
                        obj.Instantiate().SetParent(PrepareRoot);
                } else {
                    GameObject.Find("[Base]").SetActive(true);
                }
            }
            await UniTask.DelayFrame(1);


            // Call Test Objs Active After Scene Inited
            if (testObjs.Length != 0) {
                if (!GameObject.Find("[Test]")) {
                    GameObject TestRoot = new GameObject("[Tests]");
                    foreach (var obj in testObjs)
                        obj.SetParent(TestRoot).SetActive(true);
                } else {
                    GameObject.Find("[Test]").SetActive(true);
                }
            }
            await UniTask.DelayFrame(1);

            // switch game state
            if (GameManager.Instance != null) 
                GameManager.Instance.Switch(calledEndState);

            // 销毁自身
            // Destroy(this.gameObject);
        }

        /// <summary>
        /// 初始游戏
        /// </summary>
        /// <returns></returns>
        private async UniTask ExitGame() {
            await UniTask.DelayFrame(1);
            GameObject.Find("[Test]")?.SetActive(false);
            GameObject.Find("[Base]")?.SetActive(false);
        }
    }
}