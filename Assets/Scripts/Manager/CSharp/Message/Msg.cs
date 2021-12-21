using System.Collections.Generic;
using UnityEngine;
using ActFG.Util.Tools;

namespace ActFG.Message {
    /// <summary>
    /// 消息体
    /// </summary>
    public class Msg {
        private Dictionary<string, object> DataMap = new Dictionary<string, object>();

        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key) {
            if (DataMap.ContainsKey(key))
                return DataMap[key];
            return null;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Msg SetData(string key, object value) {
            if (DataMap.ContainsKey(key)) {
                Debug.Log($"Msg has contains {key}".StringColor(Color.red));
            } else {
                DataMap[key] = value;
                return this;
            }
            return null;
        }
    }
}