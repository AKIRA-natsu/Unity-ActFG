using System;
using UnityEngine;

namespace ActFG.Attribute {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    // 选择 Prefab 获得路劲
    public class PrefabPath : PropertyAttribute {
        public string path;
    }
}