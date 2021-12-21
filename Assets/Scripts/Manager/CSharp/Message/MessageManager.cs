using System.Collections.Generic;
using UnityEngine;
using ActFG.Manager;
using ActFG.Util.Tools;

namespace ActFG.Message {
    /// <summary>
    /// 消息管理
    /// </summary>
    public class MessageManager : Singleton<MessageManager> {
        public delegate void MsgDeletege(Msg obj);
        private Dictionary<string, MsgDeletege> _Msg = new Dictionary<string, MsgDeletege>();

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="key">消息名称</param>
        /// <param name="action">消息体</param>
        public void AddListener(string key, MsgDeletege action) {
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
        public void RemoveListener(string key, MsgDeletege action) {
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
        public void RemoveAllListener() {
            _Msg.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        public void SendMsg(string key) {
            if (_Msg.ContainsKey(key)) {
                _Msg[key]?.Invoke(null);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        public void SendMsg(string key, Msg msg) {
            if (_Msg.ContainsKey(key)) {
                _Msg[key]?.Invoke(msg);
            }
        }
    }
}