using System.Collections;
using UnityEngine;

namespace AKIRA.Manager {
    public abstract class ResourceBase : MonoBehaviour {
        /// <summary>
        /// 顺序
        /// </summary>
        protected int order = 0;

        /// <summary>
        /// 是否已经加载完，手动改值，Update使用
        /// </summary>
        protected bool isLoaded = false;
        
        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator Load();

        protected virtual void Awake() {
            ResourceCollection.Instance.Regist(this, order);
        }
    }
}