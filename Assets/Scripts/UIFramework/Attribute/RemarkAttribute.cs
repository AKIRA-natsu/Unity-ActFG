using System;
using ActFG.UIFramework;
using UnityEngine;

namespace ActFG.Attribute {
    /// <summary>
    /// 标记，解释
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class RemarkAttribute : System.Attribute {
        public string Remark { get; private set; }

        public RemarkAttribute(string remark) => 
            this.Remark = remark;
    }
}