using System.Collections;
using System.Collections.Generic;
using AKIRA.Coroutine;
using UnityEngine;

namespace AKIRA.Manager {
    public class ResourceCollection : Singleton<ResourceCollection> {
        // 排序字典
        private SortedDictionary<int, List<ResourceBase>> ResourceMap = new SortedDictionary<int, List<ResourceBase>>();
        /// <summary>
        /// 加载委托
        /// </summary>
        /// <param name="i">当前进度</param>
        /// <param name="j">总进度</param>
        public delegate void OnLoad(int i, int j);
        public OnLoad onLoad;
        
        // 一次加载总进度
        private int totalProgress = 0;

        private ResourceCollection() {}

        /// <summary>
        /// 加载
        /// </summary>
        public void Load() {
            if (totalProgress == 0) {
                $"无加载项".Colorful(Color.red).Log();
                return;
            }
            CoroutineManager.Instance.Start(ResourceLoad());
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResourceLoad() {
            var currentProgress = 0;
            foreach (var list in ResourceMap.Values) {
                foreach (var res in list) {
                    yield return CoroutineManager.Instance.Start(res.Load());
                    currentProgress++;
                    onLoad?.Invoke(currentProgress, totalProgress);
                }
                yield return null;
            }
            // 全部结束 清空
            totalProgress = 0;
            ResourceMap.Clear();
        }

        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="order">顺序编号</param>
        public void Regist(ResourceBase resource, int order) {
            if (!ResourceMap.ContainsKey(order))
                ResourceMap.Add(order, new List<ResourceBase>());
            ResourceMap[order].Add(resource);

            totalProgress++;
        }

        /// <summary>
        /// 注册资源加载事件
        /// </summary>
        /// <param name="onLoad"></param>
        public void Regist(OnLoad onLoad) {
            this.onLoad += onLoad;
        }
    }
}