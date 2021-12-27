using System;
using ActFG.UIFramework;
using ActFG.Util.Tools;
using UnityEngine;

namespace ActFG.Attribute {
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class RemarkAttribute : System.Attribute {
        public string remark { get; private set; }

        public RemarkAttribute(string remark) => 
            this.remark = remark;
    }
}