using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 引用对象基类
    /// </summary>
    public class ReferenceBase : IPool {
        /// <summary>
        /// 是否被激活
        /// </summary>
        /// <value></value>
        public bool active { get; private set; } = false;

        public virtual void Wake() {
            active = true;
        }

        public virtual void Recycle() {
            active = false;
        }
    }

    /// <summary>
    /// 引用池协助
    /// </summary>
    public static class ReferenceHelp {
        private static Dictionary<Component, List<ReferenceBase>> ComponentReferenceMap = new();

        /// <summary>
        /// 为Component添加对象引用
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Attach<T>(this Component component) where T : ReferenceBase, new() {
            var refer = ReferencePool.Instance.Instantiate<T>();
            if (ComponentReferenceMap.ContainsKey(component)) {
                ComponentReferenceMap[component].Add(refer);
            } else {
                ComponentReferenceMap.Add(component, new() { refer });
            }
            return refer;
        }

        /// <summary>
        /// 为Component移除对象引用
        /// </summary>
        /// <param name="component"></param>
        /// <param name="refer"></param>
        /// <typeparam name="T"></typeparam>
        public static bool Detach<T>(this Component component) where T : ReferenceBase, new() {
            if (ComponentReferenceMap.ContainsKey(component)) {
                var componentList = ComponentReferenceMap[component];
                for (int i = 0; i < componentList.Count; i++) {
                    var refer = componentList[i];
                    if (refer is T) {
                        componentList.RemoveAt(i);
                        ReferencePool.Instance.Destory(refer);
                        return true;
                    }
                }
                $"{component}不包含{typeof(T)}引用".Colorful(Color.red).Log();
                return false;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 为Component去掉所有引用
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool Detach(this Component component) {
            if (ComponentReferenceMap.ContainsKey(component)) {
                var componentList = ComponentReferenceMap[component];
                foreach (var refer in componentList)
                    ReferencePool.Instance.Destory(refer);
                componentList.Clear();
                ComponentReferenceMap.Remove(component);
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 获得Component的 T 引用
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetReference<T>(Component component) where T : ReferenceBase, new() {
            if (ComponentReferenceMap.ContainsKey(component)) {
                var componentList = ComponentReferenceMap[component];
                foreach (var refer in componentList) {
                    if (refer is T) {
                        return refer as T;
                    }
                }
                return default;
            } else {
                return default;
            }
        }
    }
}