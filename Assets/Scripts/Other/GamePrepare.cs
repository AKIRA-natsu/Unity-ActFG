using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Behaviour.Prepare {
    /// <summary>
    /// 游戏准备入口
    /// </summary>
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
        // 如果直接进入游戏，进入Playing，否则进入Ready
        [ExtraInfo(@"如果true，进入Playing状态，否则进入Ready状态")]
        [SerializeField]
        private bool enterGameDirect = false;

        private void Start() {
            // Call Managers
            if (globalObjs.Length != 0) {
                GameObject GlobalRoot = new GameObject("[Global]");
                if (globalAsDontDestory)
                    GlobalRoot.DontDestory();
                foreach (var obj in globalObjs)
                    obj.Instantiate().SetParent(GlobalRoot);
            }

            // 
            var manager = GameManager.Instance;
            if (manager == null) 
                return;
            // regist gamestate method
            var inputSystem = PlayerInputSystem.Instance;
            if (enterGameDirect) {
                manager.RegistStateAction(GameState.Playing, EnterGame);
                manager.Switch(GameState.Playing);
                inputSystem.SwtichPlayer();
            } else {
                manager.RegistStateAction(GameState.Ready, GameReady);
                manager.Switch(GameState.Ready);
                inputSystem.SwitchUI();
            }
        }

        /// <summary>
        /// 游戏准备
        /// </summary>
        private void GameReady() {
            GameManager.Instance.RegistStateAction(GameState.Playing, EnterGame);
            ExitGame();
        }

        /// <summary>
        /// 进入游戏
        /// </summary>
        private void EnterGame() {
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

            // 销毁自身
            // Destroy(this.gameObject);

            // 进入游戏后移除事件
            GameManager.Instance.RemoveStateAction(GameState.Playing, EnterGame);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        private void ExitGame() {
            GameObject.Find("[Test]")?.SetActive(false);
            GameObject.Find("[Base]")?.SetActive(false);
        }
    }
}