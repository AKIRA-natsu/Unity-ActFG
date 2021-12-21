using System;
using System.Reflection;
using System.Linq;

namespace ActFG.Util.Tools {
    public static class AttributeHelp<T> {
        /// <summary>
        /// 获得attribute标记的类
        /// </summary>
        /// <returns></returns>
        public static Type[] Handle() {
            Assembly asm = Assembly.GetAssembly(typeof(T));
            var types = asm.GetExportedTypes();
            return types.Where(o => {
                return IsMyAttribute(System.Attribute.GetCustomAttributes(o, false));
            }).ToArray();
        }

        /// <summary>
        /// 是否是需要的attribute
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        private static bool IsMyAttribute(System.Attribute[] attrs) {
            foreach (System.Attribute a in attrs) {
                if (a is T) return true;
            }
            return false;
        }

        /// <summary>
        /// type 转 object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Type2Obj(Type type) {
            Assembly asm = Assembly.GetExecutingAssembly();
            return asm.CreateInstance(type.FullName);
        }
    }
}