using UnityEngine;
using Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// 日志
    /// </summary>
    public class DebugManager : Singleton<DebugManager> {
        [SerializeField]
        private bool GlobalDebug = true;

        protected override void Awake()
        {
            base.Awake();
            this.gameObject.DontDestory();
        }

        public void Debug<T>(T message) {
            if (GlobalDebug)
                UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// 无视全局bool打印
        /// </summary>
        /// <param name="message"></param>
        /// <param name="nessary"></param>
        /// <typeparam name="T"></typeparam>
        public void Debug<T>(T message, bool nessary = false) {
            if (nessary)
                UnityEngine.Debug.Log(message);
            else
                Debug(message);
        }
    }
}