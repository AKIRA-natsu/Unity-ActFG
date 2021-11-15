﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActFG.Manager {
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T instance;

        public static T Instance {
            get {
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
