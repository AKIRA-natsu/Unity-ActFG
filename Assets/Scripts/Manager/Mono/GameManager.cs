using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    public class GameManager : MonoSingleton<GameManager> {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public GameState state = GameState.None;
        // 上一个状态
        public GameState lastState { get; private set; }
        // 状态 - 事件
        private Dictionary<GameState, Action> StateActionMap = new Dictionary<GameState, Action>();

        // 状态改变事件
        private Action<GameState> onStateChange;

        protected override void Awake() {
            base.Awake();
            // ProjectSetting => Qualitv => VSync Count: Dont VSync
            // Application.targetFrameRate = 60;
            UIManager.Instance.Initialize();
        }

        private void Start() {
            // $"切换Ads平台，UI进行适配屏幕".Colorful(Color.green).Log();
            // var collection = UIManager.Instance.MatchableColleation();

            ResourceCollection.Instance.Load();
        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <param name="state"></param>
        public void Switch(GameState state) {
            // 不变
            if (this.state == state)
                return;

            $"游戏状态：切换{state}".Colorful(Color.magenta).Log();
            if (StateActionMap.ContainsKey(state))
                StateActionMap[state]?.Invoke();
            
            onStateChange?.Invoke(state);
            lastState = this.state;
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

        /// <summary>
        /// 移除游戏状态事件
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        public void RemoveStateAction(GameState state, Action action) {
            if (StateActionMap.ContainsKey(state)) {
                StateActionMap[state] -= action;
            }
        }

        /// <summary>
        /// 注册状态改变事件
        /// </summary>
        /// <param name="onStateChange"></param>
        public void RegistOnStateChangeAction(Action<GameState> onStateChange) {
            onStateChange?.Invoke(state);
            this.onStateChange += onStateChange;
        }
    }
}