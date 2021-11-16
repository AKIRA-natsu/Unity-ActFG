using System;
using System.Collections.Generic;
using UnityEngine;
using Util.Tools;
using ActFG.Entity;

namespace ActFG.Manager {
    /// <summary>
    /// 消息管理
    /// </summary>
    public class MessageManager : Singleton<MessageManager> {
        public delegate void MsgDeletege(Msg obj);
        private static Dictionary<string, MsgDeletege> _Msg = new Dictionary<string, MsgDeletege>();

        protected override void Awake() {
            base.Awake();
            this.gameObject.DontDestory();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="key">消息名称</param>
        /// <param name="action">消息体</param>
        public static void AddListener(string key, MsgDeletege action) {
            if (!_Msg.ContainsKey(key)) {
                _Msg[key] = action;
            } else {
                _Msg[key] += action;
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="key">消息名称</param>
        /// <param name="action">消息体</param>
        public static void RemoveListener(string key, MsgDeletege action) {
            if (!_Msg.ContainsKey(key)) {
                Debug.Log($"message dont contains {key}".StringColor(Color.red));
                if (!_Msg.ContainsValue(action)) {
                    Debug.Log($"message dont contains {action}".StringColor(Color.red));
                    return;
                }
                _Msg[key] -= action;
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public static void RemoveAllListener() {
            _Msg.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        public static void SendMsg(string key) {
            if (_Msg.ContainsKey(key)) {
                _Msg[key]?.Invoke(null);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        public static void SendMsg(string key, Msg msg) {
            if (_Msg.ContainsKey(key)) {
                _Msg[key]?.Invoke(msg);
            }
        }
    }
}