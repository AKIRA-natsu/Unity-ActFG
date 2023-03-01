using System;
using System.Collections.Generic;
using AKIRA.Manager;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Command, "Prefabs/UI/Command", WinType.Interlude)]
    public class CommandPanel : CommandPanelProp, IUpdate {
        // 显示位置
        private Vector3 scrollviewShowPosition = Vector3.zero;
        private Vector3 btnShowPosition;
        // 隐藏位置
        private Vector3 scrollviewHidePosition;
        private Vector3 btnHidePosition;

        // 页面是否显示中
        public static bool CommandMode { get; private set; }
        // 作弊指令
        internal static (string name, Action action)[] CommandDatas;

        public override void Awake(object obj) {
            base.Awake(obj);
            this.CloseBtn.onClick.AddListener(() => AcitvePanel(!CommandMode));

            var btnRect = CloseBtn.GetComponent<RectTransform>();
            // 高度适配
            var height = UI.Canvas.GetComponent<CanvasScaler>().referenceResolution.y;
            scrollviewHidePosition = Vector3.down * height;
            btnShowPosition = btnRect.anchoredPosition3D;
            btnHidePosition = Vector3.down * (height - btnRect.sizeDelta.y) / 2;

            this.Regist(UI.UIGroup);
            this.Hide();
        }

        public void GameUpdate() {
            if (Keyboard.current.backquoteKey.wasPressedThisFrame) {
                this.Show();
                this.Remove(UI.UIGroup);
            }
        }

        public override void Show() {
            base.Show();
            // 没有数据，进行一次拿取和ScrollComponent的初始化
            if (CommandDatas == null) {
                CommandDatas = CommandManager.Instance.GetActionMap();
                Content.Initialization(CommandDatas.Length, Mask, typeof(GridButton));
            }
            AcitvePanel(true);
        }

        /// <summary>
        /// 显示/隐藏页面
        /// </summary>
        /// <param name="active"></param>
        private void AcitvePanel(bool active) {
            if (CommandMode == active) {
                return;
            }

            CommandMode = active;
            if (active) {
                View.DOLocalMove(scrollviewShowPosition, 0.3f);
                CloseBtn.transform.DOLocalMove(btnShowPosition, 0.3f);
            } else {
                View.DOLocalMove(scrollviewHidePosition, 0.3f);
                CloseBtn.transform.DOLocalMove(btnHidePosition, 0.3f);
            }
        }
    }

    /// <summary>
    /// Scroll View 作弊格子
    /// </summary>
    public class GridButton : MonoBehaviour, IScrollItem {
        // 作弊按钮
        private Button button;
        // 作弊文本
        private TextMeshProUGUI text;
        // 事件
        private Action commandCallback;

        private void Awake() {
            button = this.GetComponent<Button>();
            text = this.GetComponentInChildren<TextMeshProUGUI>();

            button.onClick.AddListener(() => commandCallback?.Invoke());
        }

        public void UpdateContent(int index) {
            commandCallback = null;
            var data = CommandPanel.CommandDatas[index];
            text.text = data.name.ToString();
            commandCallback = data.action;
        }
    }
}