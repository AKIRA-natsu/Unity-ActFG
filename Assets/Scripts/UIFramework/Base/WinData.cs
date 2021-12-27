using System;
using ActFG.Attribute;

namespace ActFG.UIFramework {
    /// <summary>
    /// UI 数据
    /// </summary>
    [SerializableAttribute]
    public class WinData {
        /// <summary>
        /// UI 窗口
        /// </summary>
        public WinEnum @enum;
        /// <summary>
        /// UI 路劲 Resources路劲下
        /// </summary>
        [PrefabPath]
        public string path;
        /// <summary>
        /// UI 类型
        /// </summary>
        public WinType @type;
        /// <summary>
        /// UI 名称
        /// </summary>
        public string name;

        public WinData(WinEnum @enum, string path, WinType @type) {
            this.@enum = @enum;
            this.path = path;
            this.@type = @type;
            this.name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString() {
            return $"{@enum}: path => {path}, type => {@type}";
        }
    }
}