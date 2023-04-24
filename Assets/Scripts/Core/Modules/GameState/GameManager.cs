using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using AKIRA.UIFramework;
using UnityEngine;

namespace AKIRA.Manager {
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoSingleton<GameManager> {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public static GameState State { get; private set; } = GameState.None;

        // 状态 - 事件
        private Dictionary<GameState, Action> StateActionMap = new Dictionary<GameState, Action>();
        // 状态改变事件
        private Action<GameState> onStateChange;

        // 是否初始化UI
        [SerializeField]
        private bool callUIInitialize = true;

        protected override void Awake() {
            base.Awake();
            // ProjectSetting => Qualitv => VSync Count: Dont VSync
            // Application.targetFrameRate = 60;

            if (callUIInitialize)
                UIManager.Instance.Initialize();
        }

        // private void Start() {
        //     // $"切换Ads平台，UI进行适配屏幕".Colorful(Color.green).Log();
        //     // var collection = UIManager.Instance.MatchableColleation();
        // }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <param name="state"></param>
        public void Switch(GameState state) {
            // 不变
            if (State == state)
                return;

            $"游戏状态：切换{state}".Colorful(Color.cyan).Log();
            if (StateActionMap.ContainsKey(state))
                StateActionMap[state]?.Invoke();
            
            onStateChange?.Invoke(state);
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
            this.onStateChange -= onStateChange;
        }

        /// <summary>
        /// 判断状态是否等于
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static bool IsStateEqual(params GameState[] states) {
            return states.Contains(State);
        }

        /// <summary>
        /// 协程辅助
        /// </summary>
        /// <param name="coroutine"></param>
        public void CoroutineHelp(IEnumerator coroutine) {
            StartCoroutine(coroutine);
        }
    }
}