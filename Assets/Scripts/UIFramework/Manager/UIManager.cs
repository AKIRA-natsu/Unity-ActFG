using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActFG.Coroutine;
using ActFG.UIFramework;

namespace ActFG.Manager {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        private Dictionary<WinEnum, UIComponent> UIMap = new Dictionary<WinEnum, UIComponent>();

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
                // 注册在 UIManager
                AddUI(info.Data.@enum, com);
                // Awake UI
                com.Awake();
            }
        }

        /// <summary>
        /// 添加 UI
        /// </summary>
        public void AddUI(WinEnum @enum, UIComponent com) {
            if (UIMap.ContainsKey(@enum)) {
                $"UI has contained {@enum}".Colorful(Color.red).Log();
                return;
            }
            UIMap[@enum] = com;
        }

        /// <summary>
        /// 获得UI
        /// </summary>
        /// <param name="@enum"> UI 数据 </param>
        /// <returns></returns>
        public T GetUI<T>(WinEnum @enum) where T : UIComponent {
            if (!UIMap.ContainsKey(@enum)) {
                $"UI dont contains {@enum}".Colorful(Color.red).Log();
                return null;
            }
            return UIMap[@enum] as T;
        }

        /// <summary>
        /// 销毁 UI
        /// </summary>
        /// <param name="@enum"></param>
        public void DestoryUI(WinEnum @enum) {
            if (UIMap.ContainsKey(@enum)) {
                UIMap[@enum].Destory();
                UIDataManager.Instance.Remove(UIMap[@enum]);
                UIMap.Remove(@enum);
            }
        }

        /// <summary>
        /// 切换UI
        /// </summary>
        /// <param name="cur">隐藏</param>
        /// <param name="tar">显示</param>
        /// <param name="time"></param>
        public void Switch(WinEnum cur, WinEnum tar, float time = 0f) {
            CoroutineManager.Instance.Start(SwitchUI(cur, tar, time));
        }

        /// <summary>
        /// 协程，切换UI
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="tar"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator SwitchUI(WinEnum cur, WinEnum tar, float time) {
            yield return new Coroutine.WaitForSeconds(time);
            UIMap[cur].Hide();
            UIMap[tar].Show();
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="tar"></param>
        /// <param name="time"></param>
        public void Open(WinEnum tar, float time = 0f) {
            CoroutineManager.Instance.Start(OpenUI(tar, time));
        }

        /// <summary>
        /// 协程，打开UI
        /// </summary>
        /// <param name="tar"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator OpenUI(WinEnum tar, float time) {
            yield return new Coroutine.WaitForSeconds(time);
            UIMap[tar].Show();
        }
    }
}