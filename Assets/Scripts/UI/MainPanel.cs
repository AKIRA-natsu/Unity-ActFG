using AKIRA.Manager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Main, "Prefabs/UI/Main", WinType.Normal)]
    public class MainPanel : MainPanelProp {
        public override void Awake(object obj) {
            base.Awake(obj);
            this.EnterBtn.onClick.AddListener(GamePrepareManager.Instance.EnterGame);
            this.SettingBtn.onClick.AddListener(() => {});
            this.ExitBtn.onClick.AddListener(
                #if UNITY_EDITOR
                () => UnityEditor.EditorApplication.isPlaying = false
                #else
                Application.Quit
                #endif
            );

            GamePrepareManager.Instance.RegistOnGameEnter(HidePanel);
            GamePrepareManager.Instance.RegistOnGameExit(ShowPanel);
        }

        /// <summary>
        /// 现实页面 异步
        /// </summary>
        /// <returns></returns>
        private async UniTask ShowPanel() {
            await UniTask.DelayFrame(0);
            var transitionPanel = Get<SampleTransitionPanel>();
            transitionPanel.RegistTransitionAction(Show);
            transitionPanel.StartTransition(System.Drawing.Color.Orange.ToUnityColor());
        }

        /// <summary>
        /// 隐藏页面 异步
        /// </summary>
        /// <returns></returns>
        private async UniTask HidePanel() {
            var transitionPanel = Get<SampleTransitionPanel>();
            transitionPanel.RegistTransitionAction(Hide);
            transitionPanel.RegistTransitionEndAction(PlayerInputSystem.Instance.SwtichPlayer);
            transitionPanel.StartTransition(System.Drawing.Color.Orange.ToUnityColor());
            await UniTask.DelayFrame(0);
        }
    }
}