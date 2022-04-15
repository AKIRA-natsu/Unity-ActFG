using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Coroutine {
    public class CoroutineManager : Singleton<CoroutineManager> {
        private LinkedList<Coroutine> coroutineList = new LinkedList<Coroutine>();
        private LinkedList<Coroutine> coroutinesToStop = new LinkedList<Coroutine>();

        private CoroutineManager() {}
    
        /// <summary>
        /// 开启一个协程
        /// </summary>
        /// <param name="ie"></param>
        /// <returns></returns>
        public Coroutine Start(IEnumerator ie) {
            var c = new Coroutine(ie);
            coroutineList.AddLast(c);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ie"></param>
        public void Stop(IEnumerator ie) {}

        /// <summary>
        /// 关闭一个协程
        /// </summary>
        /// <param name="coroutine"></param>
        public void Stop(Coroutine coroutine) {
            coroutinesToStop.AddLast(coroutine);
        }

        /// <summary>
        /// 主程序驱动所有协程对象
        /// </summary>
        public void UpdateCoroutine() {
            var node = coroutineList.First;
            while (node != null) {
                var cor = node.Value;

                bool ret = false;
                if (cor != null) {
                    bool toStop = coroutinesToStop.Contains(cor);
                    if (!toStop) {
                        // 一旦协程返回false意味着该协程要退出
                        ret = cor.MoveNext();
                    }
                }

                if (!ret) {
                    coroutineList.Remove(node);
                    // Debug.Log($"[CoroutineManager] remove {cor}");
                }

                node = node.Next;
            }
        }

    }   
}