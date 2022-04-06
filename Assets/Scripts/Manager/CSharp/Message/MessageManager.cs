using System;
using System.Collections.Generic;
using AKIRA.Manager;

namespace AKIRA.Message {
    /// <summary>
    /// 消息管理
    /// 参考：https://blog.csdn.net/u014361280/article/details/104199926
    /// </summary>
    public class MessageManager : Singleton<MessageManager> {
        public delegate void MsgDeletege(Msg obj);
        private Dictionary<int, MsgDeletege> MsgMap = new Dictionary<int, MsgDeletege>();

        /// <summary>
        /// 防止 new
        /// </summary>
        private MessageManager() {}

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="handle">事件</param>
        public void RegisterListener(int type, MsgDeletege handle) {
            if (handle == null) return;

            MsgDeletege myHandle = null;
            MsgMap.TryGetValue(type, out myHandle);
            MsgMap[type] = (MsgDeletege)Delegate.Combine(myHandle, handle);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="type">消息名称</param>
        /// <param name="handle">事件</param>
        public void RemoveListener(int type, MsgDeletege handle) {
            if (handle == null) return;
            MsgMap[type] = (MsgDeletege)Delegate.Remove(MsgMap[type], handle);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear() {
            MsgMap.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void SendMsg(int type, object data = null) {
            MsgDeletege handle;
            if (MsgMap.TryGetValue(type, out handle)) {
                Msg evt = new Msg(type, data);
                try {
                    if (handle != null)
                    {
                        handle(evt);
                    }
                } catch (System.Exception e) {
                    $"SendMessage: {evt.Type.ToString()}, {e.Message}, {e.StackTrace}, {e}".Error();
                }
            }
        }
    }
}