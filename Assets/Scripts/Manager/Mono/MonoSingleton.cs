using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActFG.Manager {
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    // 忘记实例的保险
                    GameObject manager = new GameObject($"[{typeof(T).Name}]");
                    instance = manager.AddComponent(typeof(T)) as T;
                }
                return instance;
            }
        }

        public static bool IsInitialized {
            get {
                return instance != null;
            }
        }

        protected virtual void Awake() {
            if (instance == null)
                instance = (T)this;
            else
                Destroy(gameObject);
        }

        protected virtual void OnDestroy() {
            if (instance != null)
                instance = null;
        }
    }
}
