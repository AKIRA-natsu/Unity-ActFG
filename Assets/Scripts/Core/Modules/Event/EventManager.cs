using System;
using System.Collections.Generic;
using AKIRA.Data;

namespace AKIRA.Manager {
    /// <summary>
    /// 事件中心
    /// </summary>
    public class EventManager : Singleton<EventManager> {
        /// <summary>
        /// 事件表
        /// </summary>
        private Dictionary<string, Action<object>> EventMap = new Dictionary<string, Action<object>>();

        protected EventManager() {}

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void AddEventListener(string key, Action<object> action) {
            if (String.IsNullOrEmpty(key) || action == null)
                return;

            if (EventMap.ContainsKey(key))
                EventMap[key] += action;
            else
                EventMap[key] = action;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(string key, Action<object> action) {
            if (String.IsNullOrEmpty(key) || action == null)
                return;

            if (EventMap.ContainsKey(key))
                EventMap[key] -= action;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void TriggerEvent(string key, object value = null) {
            if (String.IsNullOrEmpty(key))
                return;

            if (!EventMap.ContainsKey(key))
                return;

            $"Trigger Event {key}".Log(GameData.Log.Event);
            EventMap[key]?.Invoke(value);
        }

        /// <summary>
        /// 清除事件
        /// </summary>
        /// <param name="key"></param>
        public void ClearEvent(string key) {
            if (String.IsNullOrEmpty(key))
                return;

            if (EventMap.ContainsKey(key))
                EventMap.Remove(key);
        }

        /// <summary>
        /// 清空事件
        /// </summary>
        public void ClearEvent() {
            EventMap.Clear();
        }
    }
}