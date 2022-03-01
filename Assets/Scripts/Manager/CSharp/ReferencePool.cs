using System.Collections;
using System.Collections.Generic;
using ActFG.Manager.Interface;
using UnityEngine;

namespace ActFG.Manager {
    /// <summary>
    /// 引用池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReferencePool<T> : Singleton<ReferencePool<T>> where T : class, new() {
        private Queue<T> PoolQue = new Queue<T>();
        
        private ReferencePool() {}

        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
        public T Instantiate() {
            T reference;
            if (PoolQue.Count == 0)
                reference = new T();
            else
                reference = PoolQue.Dequeue();
            if (reference is IReferencePoolAction)
                ((IReferencePoolAction)reference).Wake();
            return reference;
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="reference"></param>
        public void Recycle(T reference) {
            if (reference is IReferencePoolAction)
                ((IReferencePoolAction)reference).Recycle();
            PoolQue.Enqueue(reference);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear() {
            PoolQue.Clear();
        }
    }
}
