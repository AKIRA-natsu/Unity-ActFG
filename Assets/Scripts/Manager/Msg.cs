using System.Collections.Generic;
using UnityEngine;
using Util.Tools;
using ActFG.Manager;

namespace ActFG.Entity {
    /// <summary>
    /// 消息
    /// </summary>
    public class Msg {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key) {
            if (data.ContainsKey(key))
                return data[key];
            return null;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Msg SetData(string key, object value) {
            if (data.ContainsKey(key)) {
                DebugManager.Instance.Debug($"Msg has contains {key}".StringColor(Color.red));
            } else {
                data[key] = value;
                return this;
            }
            return null;
        }
    }
}