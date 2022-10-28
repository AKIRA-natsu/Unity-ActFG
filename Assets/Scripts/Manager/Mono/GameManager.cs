using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    public class GameManager : MonoSingleton<GameManager> {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public static GameState State { get; private set; }
        // 上一个状态
        public GameState LastState { get; private set; }

        // 状态 - 事件
        private Dictionary<GameState, Action> StateActionMap = new Dictionary<GameState, Action>();
        // 状态改变事件
        private Action<GameState> onStateChange;

        protected override void Awake() {
            base.Awake();
            // ProjectSetting => Qualitv => VSync Count: Dont VSync
            // Application.targetFrameRate = 60;
            UIManager.Instance.Initialize();
            State = GameState.None;
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
            if (State == state)
                return;

            $"游戏状态：切换{state}".Colorful(Color.magenta).Log();
            if (StateActionMap.ContainsKey(state))
                StateActionMap[state]?.Invoke();
            
            onStateChange?.Invoke(state);
            LastState = State;
            State = state;
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
            onStateChange?.Invoke(State);
            this.onStateChange += onStateChange;
        }

        /// <summary>
        /// 移除状态改变事件
        /// </summary>
        /// <param name="onStateChange"></param>
        public void RemoveOnStateChangeAction(Action<GameState> onStateChange) {
            if (IsApplicationOut)
                return;
            this.onStateChange -= onStateChange;
        }

        /// <summary>
        /// 判断状态是否等于
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static bool IsStateEqual(params GameState[] states) {
            foreach (var state in states)
                if (state == State)
                    return true;
            return false;
        }
    }
}