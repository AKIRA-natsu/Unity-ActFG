using System.Collections;
using UnityEngine;

namespace AKIRA.Coroutine {
    /// <summary>
    /// Use Coroutine By MonoBehaviour
    /// </summary>
    public static class CoroutineMonoManager {
        private class CoroutineMonoManagerHelp : MonoBehaviour {}

        private static MonoBehaviour instance;
        private static MonoBehaviour Instance {
            get {
                if (instance == null)
                    instance = new GameObject("[CoroutineMonoManager]", typeof(CoroutineMonoManagerHelp)).DontDestory().GetComponent<CoroutineMonoManagerHelp>();
                return instance;
            }
        }

        /// <summary>
        /// Start a Coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public static UnityEngine.Coroutine StartCoroutine(IEnumerator coroutine) {
            return Instance.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stop a Coroutine
        /// </summary>
        /// <param name="coroutine"></param>
        public static void StopCoroutine(UnityEngine.Coroutine coroutine) {
            Instance.StopCoroutine(coroutine);
        }
    }
}