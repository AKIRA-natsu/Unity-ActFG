using System;
using UnityEngine;

namespace AKIRA.Manager {
    public abstract class PoolBase {
        // 对象池根节点下父节点
        protected Transform poolParent;
        // 池子最大包含数量
        protected int maxCount = 0;

        /// <summary>
        /// 释放池子（销毁）
        /// </summary>
        public abstract void Free();
    }
}