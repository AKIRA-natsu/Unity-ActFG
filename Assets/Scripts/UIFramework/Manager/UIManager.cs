using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActFG.Coroutine;
using ActFG.UIFramework;
using ActFG.Util.Tools;
using ActFG.Attribute;

namespace ActFG.Manager {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        private Dictionary<WinEnum, UIComponent> UIMap = new Dictionary<WinEnum, UIComponent>();

        private UIManager() {}

        /// <summary>
        /// <para>启动只运行一次</para>
        /// <para>Map 添加 UICom</para>
        /// <para>UI Awake</para>
        /// </summary>
        public void Awake() {
            var wins = AttributeHelp<WinAttribute>.Handle();
            foreach (var win in wins) {
                // attribute运行了两次！
                var com = (UIComponent)AttributeHelp<WinAttribute>.Type2Obj(win);
                var info = win.GetCustomAttributes(false)[0] as WinAttribute;
                // 注册在UIDataManager
                UIDataManager.Instance.Register(com, info.data);
                // 注册在UIManager
                AddUI(info.data.@enum, com);
                // Awake UI
                com.Awake();
            }
        }

        /// <summary>
        /// 添加 UI
        /// </summary>
        public void AddUI(WinEnum @enum, UIComponent com) {
            if (UIMap.ContainsKey(@enum)) {
                $"UI has contained {@enum}".StringColor(Color.red).Log();
                return;
            }
            UIMap[@enum] = com;
        }

        /// <summary>
        /// 获得一个 UI
        /// </summary>
        /// <param name="@enum"> UI 数据 </param>
        /// <returns></returns>
        public UIComponent GetUI(WinEnum @enum) {
            if (!UIMap.ContainsKey(@enum)) {
                $"UI dont contains {@enum}".StringColor(Color.red).Log();
                return null;
            }
            return UIMap[@enum];
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
        /// <para>切换UI</para>
        /// <para>部分需要</para>
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
    }
}