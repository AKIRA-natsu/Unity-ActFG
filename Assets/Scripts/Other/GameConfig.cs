using AKIRA.Manager;
using AKIRA.Manager.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Behaviour.Prepare {
    /// <summary>
    /// <para>游戏设置</para>
    /// <para>作弊注册中间件</para>
    /// <para>模块的装载卸载</para>
    /// </summary>
    public class GameConfig : MonoBehaviour {
        [Space]
        /// <summary>
        /// 注册作弊，只是给面板一个显示
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private bool registCommands = true;

        [Space]
        /// <summary>
        /// 是否装载玩家
        /// </summary>
        [SerializeField]
        private bool loadPlayer = true;
        /// <summary>
        /// 玩家预制体
        /// </summary>
        [SerializeField]
        private Player player;
        /// <summary>
        /// 玩家
        /// </summary>
        public static Player Player { get; private set; }

        private void Start() {
            CommandRegister();
            LoadPlayer();

            #if UNITY_EDITOR
            inited = true;
            #endif
        }

        /// <summary>
        /// 作弊注册
        /// </summary>
        private void CommandRegister() {
            CommandManager.Instance.RegistSpecialAction(Command.EarnMoney, () => MoneyManager.Instance.Earn(100000));
            CommandManager.Instance.RegistSpecialAction(Command.NextLevel, LevelManager.Instance.NextLevel);
            CommandManager.Instance.RegistSpecialAction(Command.LastLevel, LevelManager.Instance.LastLevel);
        }

        #if UNITY_EDITOR
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool inited = false;

        private void OnValidate() {
            if (!Application.isPlaying)
                return;

            if (!inited)
                return;
            
            LoadPlayer();
        }
        #endif

        /// <summary>
        /// 装载玩家
        /// </summary>
        private void LoadPlayer() {
            if (loadPlayer) {
                if (Player == null) {
                    Player = GameObject.Instantiate(player);
                }
            } else {
                if (Player != null) {
                    GameObject.Destroy(Player.gameObject);
                    Player = null;
                }
            }
        }
    }
}