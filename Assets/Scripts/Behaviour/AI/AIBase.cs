using System.Collections;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.AI {
    /// <summary>
    /// AI 基类
    /// </summary>
    [SelectionBase]
    public abstract class AIBase : MonoBehaviour, IPool, IUpdate, IResource {
        public abstract int order { get; }
        // 初始化数据
        protected Object Data { get; private set; }
        // 实体
        protected Transform Render;

        public abstract void GameUpdate();
        public abstract IEnumerator Load();

        public abstract void Wake();
        public abstract void Recycle();

        /// <summary>
        /// 注册加载
        /// </summary>
        /// <param name="data"></param>
        public void RegistLoad(Object data = null) {
            this.Data = data;
            ResourceCollection.Instance.Regist(this, order);
        }
    }
}