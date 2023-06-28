using AKIRA.Data;
using AKIRA.Manager;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Main, "Prefabs/UI/Main", WinType.Normal)]
    public class MainPanel : MainPanelProp {
        public override void Awake(object obj) {
            base.Awake(obj);
            this.EnterBtn.onClick.AddListener(OnGameStart);
            this.SettingBtn.onClick.AddListener(() => {});
            this.ExitBtn.onClick.AddListener(
                #if UNITY_EDITOR
                () => UnityEditor.EditorApplication.isPlaying = false
                #else
                Application.Quit
                #endif
            );

            GameManager.Instance.RegistStateAction(GameState.Playing, Hide);
            GameManager.Instance.RegistStateAction(GameState.Ready, Show);
        }

        /// <summary>
        /// 游戏开始事件
        /// </summary>
        private void OnGameStart() {
            GameManager.Instance.Switch(GameState.Playing);
            EventManager.Instance.TriggerEvent(GameData.Event.OnGameStart);
        }
    }
}