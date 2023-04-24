using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// <para>对象池</para>
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool> {
        // 泛型池
        private Dictionary<string, PoolBase> ObjectPoolMap = new Dictionary<string, PoolBase>();
        // 纯GameObject
        private Dictionary<string, Pool> GameObjectMap = new Dictionary<string, Pool>();

        private GameObject root;
        /// <summary>
        /// 对象池根节点
        /// </summary>
        /// <value></value>
        public GameObject Root {
            get {
                if (root == null) {
                    root = GameObject.Find("[ObjectPool]");
                    if (root == null) {
                        // 不存在创建
                        root = new GameObject("[ObjectPool]").DontDestory();
                    }
                }
                return root;
            }
        }
        
        private ObjectPool() {}

        # region 泛型
        /// <summary>
        /// <para>数据访问</para>
        /// <para>https://cloud.tencent.com/developer/ask/sof/468615</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerable<Pool<T>> GetPoolType<T>() where T : Component, IPool {
            return ObjectPoolMap.Values.OfType<Pool<T>>();
        }

        #region path 获得对象
        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="space">默认世界坐标</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, Space space = Space.World) where T : Component, IPool {
            T com = Instantiate<T>(path, parent);
            if (space == Space.World) {
                com.transform.position = position;
                com.transform.rotation = rotation;
            } else {
                com.transform.localPosition = position;
                com.transform.localRotation = rotation;
            }
            return com;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation) where T : Component, IPool {
            T com = Instantiate<T>(path);
            com.transform.position = position;
            com.transform.rotation = rotation;
            return com;
        }

        /// <summary>
        /// 对象池获得对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(string path, Transform parent) where T : Component, IPool {
            T com = Instantiate<T>(path);
            com.SetParent(parent);
            return com;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(string path) where T : Component, IPool {
            var names = path.Split('/');
            var name = names[names.Length - 1];
            if (ObjectPoolMap.ContainsKey(name)) {
                var pool = ObjectPoolMap[name] as Pool<T>;
                return pool.Instantiate(path);
                // return GetPoolType<T>().First().Instantiate(path);
            } else {
                var pool = new Pool<T>().Init(Root.transform, name);
                ObjectPoolMap.Add(name, pool);
                return pool.Instantiate(path);
            }
        }
        #endregion

        #region T 获得对象
        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="space">默认世界坐标</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(T target, Vector3 position, Quaternion rotation, Transform parent, Space space = Space.World) where T : Component, IPool {
            T com = Instantiate(target, parent);
            if (space == Space.World) {
                com.transform.position = position;
                com.transform.rotation = rotation;
            } else {
                com.transform.localPosition = position;
                com.transform.localRotation = rotation;
            }
            return com;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(T target, Vector3 position, Quaternion rotation) where T : Component, IPool {
            T com = Instantiate(target);
            com.transform.position = position;
            com.transform.rotation = rotation;
            return com;
        }

        /// <summary>
        /// 从对象池中获得对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(T target, Transform parent) where T : Component, IPool {
            T com = Instantiate(target);
            com.SetParent(parent);
            return com;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="com"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(T com) where T : Component, IPool {
            var name = com.name;
            if (ObjectPoolMap.ContainsKey(name)) {
                var pool = ObjectPoolMap[name] as Pool<T>;
                return pool.Instantiate(com);
                // return GetPoolType<T>().First().Instantiate(com);
            } else {
                var pool = new Pool<T>().Init(Root.transform, name);
                ObjectPoolMap.Add(name, pool);
                return pool.Instantiate(com);
            }
        }
        #endregion

        /// <summary>
        /// 对象池销毁池对象
        /// </summary>
        /// <param name="com"></param>
        /// <typeparam name="T"></typeparam>
        public void Destory<T>(T com) where T : Component, IPool {
            var name = com.name;
            if (ObjectPoolMap.ContainsKey(name)) {
                // FIXME: 继承类是空
                var pool = ObjectPoolMap[name] as Pool<T>;
                pool.Destory(com);
                // GetPoolType<T>().First().Destory(com);
            } else {
                var pool = new Pool<T>().Init(Root.transform, name);
                ObjectPoolMap.Add(name, pool);
                pool.Destory(com);
            }
        }

        /// <summary>
        /// 对象池释放池
        /// </summary>
        /// <param name="com"></param>
        /// <typeparam name="T"></typeparam>
        public void Free<T>(T com) where T : Component, IPool {
            // var name = typeof(T).Name;
            var name = com.name;
            if (!ObjectPoolMap.ContainsKey(name)) {
                $"{name} 池子不存在！".Colorful(Color.red).Log();
                return;
            }
            var pool = ObjectPoolMap[name] as Pool<T>;
            pool.Free();
            // GetPoolType<T>().First().Free();
            ObjectPoolMap.Remove(name);
        }
        #endregion
    
    
        #region 纯GameObject
        #region path 获得对象
        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="space">默认世界坐标</param>
        /// <returns></returns>
        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent, Space space = Space.World) {
            GameObject go = Instantiate(path, parent);
            if (space == Space.World) {
                go.transform.position = position;
                go.transform.rotation = rotation;
            } else {
                go.transform.localPosition = position;
                go.transform.localRotation = rotation;
            }
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation) {
            GameObject go = Instantiate(path);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Instantiate(string path, Transform parent) {
            GameObject go = Instantiate(path);
            go.transform.SetParent(parent);
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public GameObject Instantiate(string path) {
            var names = path.Split('/');
            var name = names[names.Length - 1];
            if (GameObjectMap.ContainsKey(name)) {
                return GameObjectMap[name].Instantiate(path);
            } else {
                var pool = new Pool().Init(Root.transform, name);
                GameObjectMap.Add(name, pool);
                return pool.Instantiate(path);
            }
        }
        #endregion

        #region GameObject 获得对象
        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="space">默认世界坐标</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject target, Vector3 position, Quaternion rotation, Transform parent, Space space = Space.World) {
            GameObject go = Instantiate(target, parent);
            if (space == Space.World) {
                go.transform.position = position;
                go.transform.rotation = rotation;
            } else {
                go.transform.localPosition = position;
                go.transform.localRotation = rotation;
            }
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject target, Vector3 position, Quaternion rotation) {
            GameObject go = Instantiate(target);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject target, Transform parent) {
            GameObject go = Instantiate(target);
            go.transform.SetParent(parent);
            return go;
        }

        /// <summary>
        /// 对象池获得池对象
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject target) {
            var name = target.name;
            if (GameObjectMap.ContainsKey(name)) {
                return GameObjectMap[name].Instantiate(target);
            } else {
                var pool = new Pool().Init(Root.transform, name);
                GameObjectMap.Add(name, pool);
                return pool.Instantiate(target);
            }
        }
        #endregion

        /// <summary>
        /// 对象池销毁池对象
        /// </summary>
        /// <param name="go"></param>
        /// <typeparam name="T"></typeparam>
        public void Destory(GameObject go) {
            var name = go.name;
            if (GameObjectMap.ContainsKey(name)) {
                GameObjectMap[name].Destory(go);
            } else {
                var pool = new Pool().Init(Root.transform, go.name);
                GameObjectMap.Add(name, pool);
                pool.Destory(go);
            }
        }

        /// <summary>
        /// 对象池释放池(一整个GameObject相关)
        /// </summary>
        /// <param name="go"></param>
        public void Free(GameObject go) {
            var name = go.name;
            if (!GameObjectMap.ContainsKey(name)) {
                $"{name} 池子不存在！".Colorful(Color.red).Log();
                return;
            }
            GameObjectMap[name].Free();
            GameObjectMap.Remove(name);
        }
        #endregion
    }
}