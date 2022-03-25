using System;
using ActFG.UIFramework;
using UnityEngine;

namespace ActFG.Attribute {
    /// <summary>
    /// 控件路径
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class UIControlAttribute : System.Attribute {
        public string Path { get; private set; }
        public bool Matchable { get; private set; }

        public UIControlAttribute(string path, bool matchable = false) {
            this.Path = path;
            this.Matchable = matchable;
        }
            
    }
}