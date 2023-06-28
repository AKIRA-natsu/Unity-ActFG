using System;
using System.Collections.Generic;
using AKIRA.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

/// <summary>
/// 键盘映射表
/// </summary>
[InputControlLayout()]
[System.Serializable]
public class CheatKeyBind {
    // 键盘按键
    [InputControl]
    public string keycode;
    // 名称
    [SelectionPop(typeof(GameData.Cheat))]
    public string cheat;
}

namespace AKIRA.Manager {
    /// <summary>
    /// 键盘映射事件管理中心
    /// </summary>
    [Source("Source/Manager/[CheatManager]", GameData.Source.Manager)]
    public class CheatManager : MonoSingleton<CheatManager>, IUpdate {
        // 特殊事件字典
        private Dictionary<string, Action> CheatMap = new Dictionary<string, Action>();
        
        // 手动注册
        [SerializeField]
        private CheatKeyBind[] cheats;

        /// <summary>
        /// 键盘是否在使用中
        /// </summary>
        public bool KeyBoardUsed = true;

        public void GameUpdate() {
            if (KeyBoardUsed)
                return;

            foreach (var value in cheats) {
                if (!CheatMap.ContainsKey(value.cheat)) {
                    continue;
                }

                
                if ((InputSystem.FindControl(value.keycode) as KeyControl).wasPressedThisFrame) {
                    CheatMap[value.cheat]?.Invoke();
                    $"作弊触发：{value.cheat}".Log(GameData.Log.Cheat);
                }
            }
        }

        private void Start() {
            UpdateManager.GetOrCreateDefaultInstance().Regist(this);
        }

        /// <summary>
        /// 注册特殊事件
        /// </summary>
        /// <param name="cheat"></param>
        /// <param name="action"></param>
        public void RegistSpecialAction(string cheat, Action action) {
            if (CheatMap.ContainsKey(cheat)) {
                CheatMap[cheat] += action;
            } else {
                CheatMap.Add(cheat, action);
            }
        }

        /// <summary>
        /// 获得作弊表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GetActionMap("></param>
        /// <returns></returns>
        public (string name, Action action)[] GetActionMap() {
            (string, Action)[] result = new (string, Action)[CheatMap.Count];
            var index = 0;
            foreach (var kvp in CheatMap)
                result[index++] = (kvp.Key, kvp.Value);
            return result;
        }
    }
}