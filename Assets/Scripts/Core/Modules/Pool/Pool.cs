using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    #region Base
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
    #endregion
    /// <summary>
    /// 池
    /// </summary>
    /// <typeparam name="T">可挂载脚本对象</typeparam>
    public class Pool<T> : PoolBase where T : Component, IPool {
        // 先进先出
        private Queue<T> pool;

        /// <summary>
        /// 池子初始化
        /// </summary>
        /// <param name="root">对象池根节点</param>
        /// <param name="name">名称</param>
        /// <param name="maxCount">池子最大数量</param>
        /// <returns></returns>
        public Pool<T> Init(Transform root, string name, int maxCount = 2000) {
            pool = new Queue<T>();
            poolParent = new GameObject(name).transform;
            poolParent.SetParent(root);
            this.maxCount = maxCount;
            return this;
        }

        /// <summary>
        /// 对象池取出对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Instantiate(string path) {
            // 池子超出限制
            if (!TryGetFree(out T com)) return null;
            // 空闲对象存在
            if (com != null) return com;
            // 池子中没有空闲对象，加载实例化
            com = path.Load<T>();
            if (com == null) {
                $"{path} 下 {typeof(T).Name} 错误！".Colorful(Color.yellow).Log();
                return default;
            }
            com = com.Instantiate();
            com.name = poolParent.name;
            pool.Enqueue(com);
            com?.Wake();
            return com;
        }

        /// <summary>
        /// 对象池取出对象
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public T Instantiate(T target) {
            // 池子超出限制
            if (!TryGetFree(out T com)) return null;
            // 空闲对象存在
            if (com != null) return com;
            // 池子中没有空闲对象，加载实例化
            com = target.Instantiate();
            com.name = poolParent.name;
            pool.Enqueue(com);
            com?.Wake();
            return com;
        }

        /// <summary>
        /// 获得空闲对象
        /// </summary>
        /// <returns></returns>
        private bool TryGetFree(out T com) {
            com = null;
            // 循环池子拿出空闲对象
            for (int i = 0; i < pool.Count; i++) {
                var poolCom = pool.Dequeue();
                if (!poolCom.gameObject.activeSelf) {
                    com = poolCom;
                    poolCom.gameObject.SetActive(true);
                    poolCom?.Wake();
                    return true;
                }
                pool.Enqueue(poolCom);
            }
            // 判断超过个数
            if (pool.Count >= maxCount) {
                $"{this}超过最大个数".Colorful(Color.red).Log();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="com"></param>
        public void Destory(T com) {
            com?.Recycle();
            com.SetParent(poolParent);
            com.gameObject.SetActive(false);
            pool.Enqueue(com);
        }

        public override void Free() {
            while (pool.Count != 0)
                pool.Dequeue().Destory();
            poolParent.Destory();
            maxCount = 0;
        }
    }

    /// <summary>
    /// GameObject池
    /// </summary>
    public class Pool : PoolBase {
        private Queue<GameObject> pool;

        /// <summary>
        /// 池子初始化
        /// </summary>
        /// <param name="root"></param>
        /// <param name="parentName">池子名称</param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public Pool Init(Transform root, string parentName, int maxCount = 2000) {
            pool = new Queue<GameObject>();
            poolParent = new GameObject(parentName).transform;
            poolParent.SetParent(root);
            this.maxCount = maxCount;
            return this;
        }

        /// <summary>
        /// 对象池取出对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public GameObject Instantiate(string path) {
            // 池子超出限制
            if (!TryGetFree(out GameObject go)) return null;
            // 空闲对象存在
            if (go != null) return go;
            // 池子中没有空闲对象，加载实例化
            go = path.Load<GameObject>();
            if (go == null) {
                $"{path} 下 {poolParent.name} 错误！".Colorful(Color.yellow).Log();
                return default;
            }
            go = go.Instantiate();
            go.name = poolParent.name;
            pool.Enqueue(go);
            return go;
        }

        /// <summary>
        /// 对象池取出对象
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject target) {
            // 池子超出限制
            if (!TryGetFree(out GameObject go)) return null;
            // 空闲对象存在
            if (go != null) return go;
            // 池子中没有空闲对象，加载实例化
            go = target.Instantiate();
            go.name = poolParent.name;
            pool.Enqueue(go);
            return go;
        }

        /// <summary>
        /// 获得空闲对象
        /// </summary>
        /// <returns></returns>
        private bool TryGetFree(out GameObject com) {
            com = null;
            // 循环池子拿出空闲对象
            for (int i = 0; i < pool.Count; i++) {
                var poolGo = pool.Dequeue();
                if (!poolGo.gameObject.activeSelf) {
                    com = poolGo;
                    poolGo.gameObject.SetActive(true);
                    return true;
                }
                pool.Enqueue(poolGo);
            }
            // 判断超过个数
            if (pool.Count >= maxCount) {
                $"{this}超过最大个数".Colorful(Color.red).Log();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="go"></param>
        public void Destory(GameObject go) {
            go.transform.SetParent(poolParent);
            go.gameObject.SetActive(false);
            pool.Enqueue(go);
        }

        public override void Free() {
            while (pool.Count != 0)
                pool.Dequeue().Destory();
            poolParent.Destory();
            maxCount = 0;
        }
    }

    /// <summary>
    /// 引用池
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class RPool<K> : PoolBase where K : ReferenceBase, new() {
        private Queue<K> rpool;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public RPool<K> Init(int maxCount = 2000) {
            rpool = new Queue<K>();
            this.maxCount = maxCount;
            return this;
        }

        /// <summary>
        /// 获得对象
        /// </summary>
        /// <returns></returns>
        public K Instantiate() {
            if (!TryGetFree(out K @class)) return null;
            // 获得空闲对象
            if (@class != null) return @class;
            // new
            @class = new K();
            rpool.Enqueue(@class);
            @class?.Wake();
            return @class;
        }

        /// <summary>
        /// 尝试获得空闲对象
        /// </summary>
        private bool TryGetFree(out K @class) {
            @class = null;
            for (int i = 0; i < rpool.Count; i++) {
                var c = rpool.Dequeue();
                if (!c.active) {
                    @class = c;
                    @class?.Wake();
                    return true;
                }
                rpool.Enqueue(c);
            }
            if (rpool.Count >= maxCount) {
                $"对象已经new最大数量".Colorful(Color.yellow).Log();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对象回收
        /// </summary>
        public void Destroy(K @class) {
            @class?.Recycle();
            rpool.Enqueue(@class);
        }

        public override void Free() {
            while (rpool.Count != 0) {
                var c = rpool.Dequeue();
                c = null;
            }
            maxCount = 0;
        }
    }

}