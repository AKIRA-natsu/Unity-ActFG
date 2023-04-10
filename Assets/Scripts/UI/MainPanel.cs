using AKIRA.Manager;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Main, "Prefabs/UI/Main", WinType.Normal)]
    public class MainPanel : MainPanelProp {
        public override void Awake(object obj) {
            base.Awake(obj);
            this.EnterBtn.onClick.AddListener(() => GameManager.Instance.Switch(GameState.Playing));
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
    }
}