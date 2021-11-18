using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : Singleton<ObjectPool> {

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
        /// <param name="gameobjectName"></param>
        /// <param name="path">Resources/{path}</param>
        /// <returns></returns>
        public GameObject CreateGameObject(string gameobjectName, string path = null) {
            if (ObjectPoolDic.ContainsKey(gameobjectName)) {
                var queue = ObjectPoolDic[gameobjectName];
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
            try {
                if (!string.IsNullOrEmpty(path))
                    gameobjectName = Path.Combine(path, gameobjectName);
                GameObject go = Instantiate(Resources.Load<GameObject>(gameobjectName));
                ObjectPoolDic[gameobjectName] = new Queue<GameObject>();
                return go;
            } catch (System.Exception ex) {
                DebugManager.Instance.Debug($"CreateError[{gameobjectName}]  ".StringColor(Color.red) + ex);
            }
            return null;
        }

        /// <summary>
        /// 销毁对象
        /// 隐藏对象，设置父节点为ObjectPool，放进队列
        /// </summary>
        /// <param name="go"></param>
        public void DestoryGameObject(GameObject go) {
            go.SetActive(false);
            go.transform.parent = this.transform;
            // name contains (clone)
            var goName = go.name.Split('(')[0];
            ObjectPoolDic[goName].Enqueue(go);
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