using System.Collections.Generic;
using System;
using AKIRA.UIFramework;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState {
    /// <summary>
    /// 无
    /// </summary>
    None,
}

namespace AKIRA.Manager {
    public class GameManager : MonoSingleton<GameManager> {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public GameState state = GameState.None;
        // 状态 - 事件
        private Dictionary<GameState, Action> StateActionMap = new Dictionary<GameState, Action>();

        protected override void Awake() {
            base.Awake();
            // ProjectSetting => Qualitv => VSync Count: Dont VSync
            Application.targetFrameRate = 60;
        }

        private void Start() {
            UIManager.Instance.Initialize();
            ResourceCollection.Instance.Load();
        }

        private void Update() {
            // UI 冲突
            if (EventSystem.current.currentSelectedGameObject)
                return;

        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <param name="state"></param>
        public void Switch(GameState state) {
            if (!StateActionMap.ContainsKey(state))
                return;
            StateActionMap[state]?.Invoke();
            this.state = state;
        }

        /// <summary>
        /// 注册游戏状态事件
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        public void RegistStateAction(GameState state, Action action) {
            if (StateActionMap.ContainsKey(state)) {
                StateActionMap[state] += action;
            } else {
                StateActionMap.Add(state, action);
            }
        }
    }
}