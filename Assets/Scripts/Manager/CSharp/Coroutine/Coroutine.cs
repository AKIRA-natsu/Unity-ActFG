using System.Collections;
using UnityEngine;

namespace ActFG.Coroutine {
    /// <summary>
    /// 迭代器对象
    /// /// </summary>
    public sealed class Coroutine {
        private IEnumerator _routine;

        public Coroutine(IEnumerator routine) {
            _routine = routine;
        }

        /// <summary>
        /// 如果返回false说明迭代器已经执行完毕
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() {
            if (_routine == null) return false;

            // 迭代器当前流程控制
            IWait wait = _routine.Current as IWait;
            bool moveNext = true;
            if (wait != null) moveNext = wait.Tick();

            if (!moveNext) {
                // 事件没有到时，返回true
                // 告诉管理器后面还有对象需要下一次继续迭代
                Debug.Log("[Coroutine] not movenext");
                return true;
            } else {
                // 此时等待事件或者帧都已经迭代完毕，看IEnumerator对象后续是否还有 yield return 对象
                // 将此结果通知给管理器，管理器会在下一次迭代时决定是否继续迭代该Coroutine对象
                Debug.Log("[Coroutine] movenext");
                return _routine.MoveNext();
            }
        }

        public void Stop() {
            _routine = null;
        }
    }
}