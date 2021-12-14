using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : MonoSingleton<ObjectPool> {

        private Dictionary<string, Queue<GameObject>> ObjectPoolDic;

        protected override void Awake() {
            base.Awake();
            this.gameObject.DontDestory();

            ObjectPoolDic = new Dictionary<string, Queue<GameObject>>();
        }

        /// <summary>
        /// 创建对象
        /// Dictionary中包含并且隐藏去除，不然创建（创建不放进队列）
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public GameObject CreateGameObject(GameObject go) {
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
                go = Instantiate(go);
                ObjectPoolDic[go.name] = new Queue<GameObject>();
                return go;
            } catch (System.Exception ex) {
                Debug.LogError($"CreateError[{go.name}] => " + ex);
            }
            return null;
        }

        /// <summary>
        /// 销毁对象
        /// 隐藏对象，设置父节点为ObjectPool，放进队列
        /// </summary>
        /// <param name="go"></param>
        public void DestoryGameObject(GameObject go) {
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
            var temp = this.transform.Find(goName);
            if (temp == null) {
                temp = new GameObject(goName).transform;
                temp.SetParent(this.transform);
            }
            go.SetParent(temp);
        }

        /// <summary>
        /// 重置除name以外所有对象
        /// </summary>
        /// <param name="name"></param>
        public void ResetGameObject(string name) {
            foreach (var kvp in ObjectPoolDic) {
                if (!kvp.Key.Equals(name))
                    for (int i = 0; i < kvp.Value.Count; i++) {
                        var go = kvp.Value.Dequeue();
                        go.SetActive(true);
                        kvp.Value.Enqueue(go);
                    }
            }
        }

        // <summary>
        /// 重置对象池中所有对象
        /// （设置显示）
        /// </summary>
        public void ResetGameObject() {
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
                    Destroy(go);
                }
            }
        }
    }
}