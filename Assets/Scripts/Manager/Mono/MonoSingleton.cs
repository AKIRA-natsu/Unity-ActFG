﻿using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// Mono 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DefaultExecutionOrder(-2)]
    [DisallowMultipleComponent]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        /// <summary>
        /// 检测程序是否退出
        /// </summary>

        public static bool IsApplicationOut { get; private set; } = false;

        // 舍弃自动生成Manager防止报错的方式
        // public static T Instance {
        //     get {
        //         if (instance == null) {
        //             // 直接 return 初始化前被调用导致会报错
        //             GameObject manager = new GameObject($"[{typeof(T).Name}]").DontDestory();
        //             instance = manager.AddComponent(typeof(T)) as T;
        //         }
        //         return instance;
        //     }
        // }

        public static T Instance => instance;

        protected virtual void Awake() {
            if (instance == null)
                instance = (T)this;
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
