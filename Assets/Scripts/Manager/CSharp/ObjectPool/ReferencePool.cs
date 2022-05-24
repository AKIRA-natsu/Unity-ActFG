using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// <para>引用池</para>
    /// </summary>
    public class ReferencePool : Singleton<ReferencePool> {

        private Dictionary<string, PoolBase> ReferenceMap = new Dictionary<string, PoolBase>();
        
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
        public K Instantiate<K>() where K : ReferenceBase, new() {
            var name = typeof(K).Name;
            if (ReferenceMap.ContainsKey(name)) {
                var rpool = ReferenceMap[name] as RPool<K>;
                return rpool.Instantiate();
                // return GetPoolType<K>().First().Instantiate();
            } else {
                var rpool = new RPool<K>().Init();
                ReferenceMap.Add(name, rpool);
                return rpool.Instantiate();
            }
        }

        /// <summary>
        /// <para>回收/销毁对象</para>
        /// <para>@class为空时，释放所有@clas的引用</para>
        /// </summary>
        /// <typeparam name="K"></typeparam>
        public void Destory<K>(K @class = null) where K : ReferenceBase, new() {
            var name = typeof(K).Name;
            if (ReferenceMap.ContainsKey(name)) {
                var rpool = ReferenceMap[name] as RPool<K>;
                if (@class == null) {
                    $"释放{name}的所有{@class}引用".Colorful(Color.yellow).Log();
                    rpool.Free();
                    // GetPoolType<K>().First().Free();
                    ReferenceMap.Remove(name);
                } else {
                    rpool.Destroy(@class);
                    // GetPoolType<K>().First().Destroy(@class);
                }
            } else {
                if (@class == null) {
                    return;
                } else {
                    var rpool = new RPool<K>().Init();
                    ReferenceMap.Add(name, rpool);
                    rpool.Destroy(@class);
                }
            }
        }
    }
}