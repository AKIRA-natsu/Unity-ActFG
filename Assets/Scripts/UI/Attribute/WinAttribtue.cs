using System;
using ActFG.UI;

namespace ActFG.Attribute {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WinAttribute : System.Attribute {
        public WinData data { get; private set; }

        public WinAttribute(WinEnum @enum, string path, WinType @type) => this.data = new WinData(@enum, path, @type);
    }
}