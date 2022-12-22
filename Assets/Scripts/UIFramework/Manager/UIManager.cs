using System;
using System.Collections.Generic;
using UnityEngine;
using AKIRA.Manager;

namespace AKIRA.UIFramework {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        // 字典表
        private Dictionary<string, UIComponent> UIMap = new Dictionary<string, UIComponent>();
        // UI注册结束事件
        private Action onAfterUIInit;

        // 是否已经初始化
        private bool isInited = false;

        private UIManager() {
            // 默认 [UI] 为UI根节点
            var root = UI.UIManagerPath.Load<GameObject>();
            if (root == null)
                throw new ArgumentNullException($"{UI.UIManagerPath} 不存在");
            UI.Initialize(root.Instantiate());
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
                var com = win.CreateInstance<UIComponent>();
                // var com = (UIComponent)AttributeHelp<WinAttribute>.Type2Obj(win);
                var info = win.GetCustomAttributes(false)[0] as WinAttribute;
                // 注册在 UIDataManager
                if (info.Data.@enum != WinEnum.None)
                    UIDataManager.Instance.Register(com, info.Data);
                // Awake UI
                com.Awake(info.Data.@type);
                // 注册在 UIManager
                AddUI(com);
            }

            isInited = true;
            onAfterUIInit?.Invoke();

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
        /// <para>可适配列表收集</para>
        /// <para>正常只会在开始运行一次，所以list放里面new了一个</para>
        /// </summary>
        /// <returns></returns>
        public List<RectTransform> MatchableColleation() {
            List<RectTransform> list = new List<RectTransform>();
            foreach (var ui in UIMap.Values) {
                foreach (var rect in ui.MatchableList)
                    list.Add(rect);
            }
            return list;
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

        /// <summary>
        /// 注册UI初始化结束事件
        /// </summary>
        /// <param name="action"></param>
        public void RegistAfterUIIInitAction(Action action) {
            if (isInited)
                action?.Invoke();
            else
                onAfterUIInit += action;
        }
    }
}