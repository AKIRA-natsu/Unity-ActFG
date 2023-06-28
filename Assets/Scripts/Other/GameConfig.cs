using AKIRA.Data;
using AKIRA.Manager;
using AKIRA.Manager.Audio;
using UnityEngine;

namespace AKIRA.Behaviour {
    /// <summary>
    /// <para>游戏设置</para>
    /// <para>作弊注册中间件</para>
    /// <para>模块的装载卸载</para>
    /// </summary>
    [Source("Source/Base/[GameConfig]", GameData.Source.Base, 1)]
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
            CheatManager.Instance.RegistSpecialAction(GameData.Cheat.GetMoney, () => MoneyManager.Instance.Earn(100000));
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