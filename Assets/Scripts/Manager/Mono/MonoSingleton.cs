using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// Mono 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DefaultExecutionOrder(-2)]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        /// <summary>
        /// 检测程序是否退出
        /// </summary>

        public static bool IsApplicationOut { get; private set; } = false;

        public static T Instance {
            get {
                if (instance == null) {
                    // 直接 return 初始化前被调用导致会报错
                    GameObject manager = new GameObject($"[{typeof(T).Name}]");
                    instance = manager.AddComponent(typeof(T)) as T;
                }
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance == null) {
                instance = (T)this;
                instance.gameObject.DontDestory();
            }
            else
                Destroy(gameObject);
        }

        protected virtual void OnDisable() {
            if (!IsApplicationOut)
                IsApplicationOut = true;
        }

        protected virtual void OnDestroy() {
            if (instance == this)
                instance = null;
        }
    }
}
