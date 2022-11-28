using System;
using System.Collections;
using System.Collections.Generic;
using AKIRA.Coroutine;
using UnityEngine;

namespace AKIRA.Manager {
    public class ResourceCollection : Singleton<ResourceCollection> {
        // 排序字典
        private SortedDictionary<int, List<IResource>> ResourceMap = new SortedDictionary<int, List<IResource>>();
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="i">当前进度</param>
        /// <param name="j">总进度</param>
        private Action<int, int> onLoad;
        /// <summary>
        /// 加载全部完成事件
        /// </summary>
        private Action onComplete;

        // 一次加载总进度
        private int totalProgress = 0;

        private ResourceCollection() {}

        /// <summary>
        /// 加载
        /// </summary>
        public void Load() {
            if (totalProgress == 0) {
                $"无加载项".Colorful(Color.green).Log();
                onComplete?.Invoke();
                onComplete = null;
            } else {
                $"加载开始".Colorful(Color.green).Log();
                CoroutineManager.Instance.Start(ResourceLoad());
            }
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
            onComplete?.Invoke();
            onComplete = null;
            onLoad = null;
            $"加载完成".Colorful(Color.green).Log();
        }

        /// <summary>
        /// 注册资源
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="order">顺序编号</param>
        public void Regist(IResource resource, int order) {
            if (!ResourceMap.ContainsKey(order))
                ResourceMap.Add(order, new List<IResource>());
            ResourceMap[order].Add(resource);

            totalProgress++;
        }

        /// <summary>
        /// 注册资源加载事件
        /// </summary>
        /// <param name="onLoad"></param>
        public void RegistOnLoadAction(Action<int, int> onLoad) {
            this.onLoad += onLoad;
        }

        /// <summary>
        /// 注册资源加载完事件
        /// </summary>
        /// <param name="onComplete"></param>
        public void RegistOnCompleteAction(Action onComplete) {
            this.onComplete += onComplete;
        }
    }
}