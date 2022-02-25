using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ActFG.Manager.Interface;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool> {

        private Dictionary<string, Queue<GameObject>> ObjectPoolDic = new Dictionary<string, Queue<GameObject>>();

        private ObjectPool() {}

        /// <summary>
        /// 创建对象
        /// Dictionary中包含并且隐藏取出，不然创建（创建不放进队列）
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject go) {
            if (ObjectPoolDic.ContainsKey(go.name)) {
                var queue = ObjectPoolDic[go.name];
                for (int i = 0; i < queue.Count; i++) {
                    var tmpGo = queue.Dequeue();
                    if (!tmpGo.activeSelf) {
                        tmpGo.SetActive(true);
                        return tmpGo;
                    }
                    queue.Enqueue(tmpGo);
                }
            }
            // 创建
            // Debug.Log($"没有{go.name}，创建".StringColor(Color.red));
            try {
                go = go.Instantiate();
                ObjectPoolDic[go.name] = new Queue<GameObject>();
                return go;
            } catch (System.Exception ex) {
                Debug.LogError($"CreateError[{go.name}] => " + ex);
            }
            return null;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject go, Vector3 position = default, Quaternion rotation = default) {
            go = this.Instantiate(go);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="com"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Instantiate<T>(T com, Vector3 position = default, Quaternion rotation = default) where T : Component {
            com = this.Instantiate(com.gameObject).GetComponent<T>();
            com.transform.position = position;
            com.transform.rotation = rotation;
            if (com is IObjectPoolAction)
                ((IObjectPoolAction)com)?.Wake();
            return com;
        }

        /// <summary>
        /// 销毁对象
        /// 隐藏对象，设置父节点为ObjectPool，放进队列
        /// </summary>
        /// <param name="go"></param>
        public void Destory(GameObject go) {
            // if name contains (clone)
            var goName = go.name;
            if (go.name.Contains('('))
                goName = go.name.Split('(')[0];
            if (!ObjectPoolDic.ContainsKey(goName)) {
                ObjectPoolDic[goName] = new Queue<GameObject>();
            }
            ObjectPoolDic[goName].Enqueue(go);
            // 
            go.SetActive(false);
            var temp = ExistPool().transform.Find(goName);
            if (temp == null) {
                temp = new GameObject(goName).transform;
                temp.SetParent(ExistPool().transform);
            }
            go.SetParent(temp);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="com"></param>
        /// <typeparam name="T"></typeparam>
        public void Destory<T>(T com) where T : Component {
            if (com is IObjectPoolAction)
                ((IObjectPoolAction)com)?.Recycle();
            this.Destory(com.gameObject);
        }

        /// <summary>
        /// 重置name物品并设置父节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public void ResetGO(string name, Transform parent) {
            var queue = ObjectPoolDic[name];
            while (queue.Count != 0) {
                var go = queue.Dequeue();
                go.SetActive(true);
                go.SetParent(parent);
            }
        }

        /// <summary>
        /// 重置对象池中所有对象
        /// （设置显示）
        /// </summary>
        public void ResetGO() {
            foreach (var queue in ObjectPoolDic.Values) {
                for (int i = 0; i < queue.Count; i++) {
                    var go = queue.Dequeue();
                    go.SetActive(true);
                    queue.Enqueue(go);
                }
            }
        }

        /// <summary>
        /// 销毁对象池中所有对象
        /// （设置隐藏）
        /// </summary>
        public void DestoryAll() {
            foreach (var queue in ObjectPoolDic.Values) {
                for (int i = 0; i < queue.Count; i++) {
                    var go = queue.Dequeue();
                    go.SetActive(false);
                    queue.Enqueue(go);
                }
            }
        }

        /// <summary>
        /// 销毁全部
        /// </summary>
        public void ClearAll() {
            foreach (var queue in ObjectPoolDic.Values) {
                while (queue.Count != 0) {
                    var go = queue.Dequeue();
                    go.Destory();
                }
            }
        }

        /// <summary>
        /// 是否在dontdestory中存在pool
        /// 不存在创建，存在获得
        /// </summary>
        /// <returns></returns>
        private GameObject ExistPool() {
            var poolGO = GameObject.Find("[ObjectPool]");
            if (poolGO == null) {
                poolGO = new GameObject("[ObjectPool]").DontDestory();
            }
            return poolGO;
        }
    }
}