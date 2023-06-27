using System;
using System.Collections.Generic;
using System.Reflection;
using AKIRA.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AKIRA.Manager {
    public class SourceSystem : Singleton<SourceSystem> {
        // 
        private GameObject root;

        protected SourceSystem() {
            if (!Application.isPlaying)
                return;
            root = new GameObject("[Source]").DontDestory();
        }

        /// <summary>
        /// 加载开始
        /// </summary>
        /// <returns></returns>
        public async void Load() {
            // 字典
            Dictionary<int, List<SourceAttribute>> map = new Dictionary<int, List<SourceAttribute>>();

            GetAttributes(ref map, GameData.DLL.AKIRA_Runtime);
            GetAttributes(ref map, GameData.DLL.Default);

            foreach (var values in map.Values) {
                foreach (var source in values) {
                    $"加载 {source.path}".Log(GameData.Log.Source);
                    var request = await Resources.LoadAsync(source.path) as GameObject;
                    if (request == null) {
                        $"{source.ToString()} 路径不存在，跳过。。".Log(GameData.Log.Source);
                        continue;
                    }
                    var s = request.Instantiate();
                    var parent = root.transform.Find(source.parentName)?.gameObject;
                    if (parent == null) {
                        parent = new GameObject(source.parentName);
                        parent.SetParent(root);
                    }
                    s.SetParent(parent);

                    // 如果存在异步加载接口，触发接口
                    if (s.TryGetComponent<ISource>(out ISource isource)) {
                        await isource.Load();
                    }
                }
            }

            EventManager.Instance.TriggerEvent(GameData.Event.OnAppSourceEnd);
        }

        /// <summary>
        /// 获得字典表
        /// </summary>
        /// <param name="dll"></param>
        /// <param name="map"></param>
        private void GetAttributes(ref Dictionary<int, List<SourceAttribute>> map, string dll) {
            var types = Assembly.Load(dll).GetTypes();
            foreach (var type in types) {
                var source = type.GetCustomAttribute<SourceAttribute>(true);
                if (source == null)
                    continue;

                if (map.ContainsKey(source.calledOrder)) {
                    map[source.calledOrder].Add(source);
                } else {
                    map.Add(source.calledOrder, new List<SourceAttribute>() { source });
                }
            }
        }

        /// <summary>
        /// 测试加载
        /// </summary>
        public void Test() {
            "测试入口，测试资源加载（非真正加载，已经按照顺序加载）".Log();
            Dictionary<int, List<SourceAttribute>> map = new Dictionary<int, List<SourceAttribute>>();

            GetAttributes(ref map, GameData.DLL.AKIRA_Runtime);
            GetAttributes(ref map, GameData.DLL.Default);

            foreach (var values in map.Values) {
                foreach (var source in values) {
                    source.ToString().Log(GameData.Log.Source);
                }
            }

            $"测试结束，调用EventManager触发 {GameData.Event.OnAppSourceEnd}".Log();
        }
    }

    /// <summary>
    /// 资源，仅GameObject
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SourceAttribute : Attribute {
        // load路径
        public string path { get; private set; }
        // 父亲名称
        public string parentName { get; private set; }
        // 调用顺序
        public int calledOrder { get; private set; }

        /// <summary>
        /// 资源加载，仅GameObject
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentName">严格使用统一脚本的const name（例 GameData.Source.Manager）</param>
        /// <param name="calledOrder"></param>
        public SourceAttribute(string path, string parentName, int calledOrder = 0) {
            this.path = path;
            this.parentName = parentName;
            this.calledOrder = calledOrder;
        }

        public override string ToString() {
            return $"Source Attaribute: {path}, parent: {parentName}, order: {calledOrder}";
        }
    }

    /// <summary>
    /// 异步加载接口
    /// </summary>
    public interface ISource {
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <returns></returns>
        UniTask Load();
    }
}