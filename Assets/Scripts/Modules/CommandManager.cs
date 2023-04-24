using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 指令枚举
/// </summary>
public enum Command {
    None,
    EarnMoney,
}

/// <summary>
/// 键盘映射表
/// </summary>
[System.Serializable]
public struct CommandBindKey {
    // 键盘按键
    public Key keycode;
    // 名称
    public Command command;
}

namespace AKIRA.Manager {
    /// <summary>
    /// 键盘映射事件管理中心
    /// </summary>
    public class CommandManager : MonoSingleton<CommandManager>, IUpdate {
        // 特殊事件字典
        private Dictionary<Command, Action> CommandMap = new Dictionary<Command, Action>();
        
        // 手动注册
        [SerializeField]
        private CommandBindKey[] commands;

        /// <summary>
        /// 键盘是否在使用中
        /// </summary>
        public bool KeyBoardUsed = true;

        /// <summary>
        /// 更新组
        /// </summary>
        public const string Group = "Command";

        public void GameUpdate() {
            if (KeyBoardUsed)
                return;

            foreach (var value in commands) {
                if (!CommandMap.ContainsKey(value.command)) {
                    continue;
                }

                if (Keyboard.current[value.keycode].wasPressedThisFrame) {
                    CommandMap[value.command]?.Invoke();
                    $"作弊触发：{value.command}".Colorful(System.Drawing.Color.Coral).Log();
                }
            }
        }

        private void Start() {
            UpdateManager.GetOrCreateDefaultInstance().Regist(this, Group);
        }

        /// <summary>
        /// 注册特殊事件
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        public void RegistSpecialAction(Command command, Action action) {
            if (CommandMap.ContainsKey(command)) {
                CommandMap[command] += action;
            } else {
                CommandMap.Add(command, action);
            }
        }

        /// <summary>
        /// 获得作弊表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GetActionMap("></param>
        /// <returns></returns>
        public (string name, Action action)[] GetActionMap() {
            (string, Action)[] result = new (string, Action)[CommandMap.Count];
            var index = 0;
            foreach (var kvp in CommandMap)
                result[index++] = (kvp.Key.ToString(), kvp.Value);
            return result;
        }
    }
}