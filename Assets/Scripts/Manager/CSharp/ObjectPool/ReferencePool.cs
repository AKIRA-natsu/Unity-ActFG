using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// <para>引用池</para>
    /// </summary>
    public class ReferencePool : Singleton<ReferencePool> {

        private Dictionary<GameObject, PoolBase> ReferenceMap = new Dictionary<GameObject, PoolBase>();
        
        private ReferencePool() {}

        /// <summary>
        /// <para>数据访问</para>
        /// <para>https://cloud.tencent.com/developer/ask/sof/468615</para>
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        private IEnumerable<RPool<K>> GetPoolType<K>() where K : ReferenceBase, new() {
            return ReferenceMap.Values.OfType<RPool<K>>();
        }

        /// <summary>
        /// 获得引用对象
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public K Instantiate<K>(GameObject target) where K : ReferenceBase, new() {
            if (ReferenceMap.ContainsKey(target)) {
                return GetPoolType<K>().First().Instantiate();
            } else {
                var rpool = new RPool<K>().Init();
                ReferenceMap.Add(target, rpool);
                return rpool.Instantiate();
            }
        }

        /// <summary>
        /// 回收/销毁对象
        /// </summary>
        /// <typeparam name="K"></typeparam>
        public void Destory<K>(GameObject target, K @class = null) where K : ReferenceBase, new() {
            if (ReferenceMap.ContainsKey(target)) {
                if (@class == null) {
                    $"释放{target}的所有{@class}引用".Colorful(Color.yellow).Log();
                    GetPoolType<K>().First().Free();
                    ReferenceMap.Remove(target);
                } else {
                    GetPoolType<K>().First().Destroy(@class);
                }
            } else {
                if (@class == null) {
                    return;
                } else {
                    var rpool = new RPool<K>().Init();
                    ReferenceMap.Add(target, rpool);
                    rpool.Destroy(@class);
                }
            }
        }
    }
}