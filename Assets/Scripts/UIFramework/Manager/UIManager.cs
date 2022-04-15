using System.Collections.Generic;
using UnityEngine;
using AKIRA.UIFramework;

namespace AKIRA.Manager {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        // 字典表
        private Dictionary<string, UIComponent> UIMap = new Dictionary<string, UIComponent>();

        private UIManager() {
            // 默认 [UI] 为UI根节点
            var root = GameObject.Find("[UI]");
            if (root == null)
                $"[UI] 不存在！".Error();
            UICanvas.Initialize(root);
        }

        /// <summary>
        /// <para>启动只运行一次</para>
        /// <para>Map 添加 UICom</para>
        /// <para>UI Awake</para>
        /// </summary>
        public void Initialize() {
            var wins = AttributeHelp<WinAttribute>.Handle();
            foreach (var win in wins) {
                // attribute运行了两次！
                var com = (UIComponent)AttributeHelp<WinAttribute>.Type2Obj(win);
                var info = win.GetCustomAttributes(false)[0] as WinAttribute;
                // 注册在 UIDataManager
                UIDataManager.Instance.Register(com, info.Data);
                // Awake UI
                com.Awake();
                // 注册在 UIManager
                AddUI(com);
            }
        }

        /// <summary>
        /// 添加 UI
        /// </summary>
        /// <param name="com"></param>
        private void AddUI(UIComponent com) {
            var name = $"{com.gameObject.name}Panel";
            if (UIMap.ContainsKey(name)) {
                $"UI has contained {name}".Colorful(Color.red).Log();
                return;
            }
            UIMap[name] = com;
        }

        /// <summary>
        /// 获得UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : UIComponent {
            var name = typeof(T).Name;
            if (!UIMap.ContainsKey(name)) {
                $"UI dont contains {name}".Colorful(Color.red).Log();
                return null;
            }
            return UIMap[name] as T;
        }

        /// <summary>
        /// 销毁 UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Destory<T>() where T : UIComponent {
            var name = typeof(T).Name;
            if (UIMap.ContainsKey(name)) {
                UIMap[name].Destory();
                UIDataManager.Instance.Remove(UIMap[name]);
                UIMap.Remove(name);
            }
        }
    }
}