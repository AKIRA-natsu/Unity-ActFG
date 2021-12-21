using System.Collections.Generic;
using ActFG.UIFramework;
using ActFG.Attribute;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// UI 数据管理
    /// </summary>
    public class UIDataManager : Singleton<UIDataManager> {
        private Dictionary<UIComponent, WinData> ComDataMap = new Dictionary<UIComponent, WinData>();

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
                Register(com, info.data);
                // 同时注册在UIManager
                UIManager.Instance.AddUI(info.data.@enum, com);
                // Awake UI
                com.Awake();
            }
        }

        /// <summary>
        /// 注册 UI
        /// </summary>
        /// <param name="com"></param>
        /// <param name="data"></param>
        private void Register(UIComponent com, WinData data) {
            if (ComDataMap.ContainsKey(com)) {
                $"ComMap contains {com}".Error();
                return;
            }
            ComDataMap[com] = data;
        }

        /// <summary>
        /// 获得 UI 数据
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public WinData GetUIData(UIComponent com) {
            if (!ComDataMap.ContainsKey(com)) {
                $"ComMap dont contain {com}".Error();
                return null;
            }
            return ComDataMap[com];
        }

        /// <summary>
        /// 移除 UI
        /// </summary>
        /// <param name="com"></param>
        public void Remove(UIComponent com) {
            if (!ComDataMap.ContainsKey(com)) {
                $"ComMap dont contain {com}".Error();
                return;
            }
            ComDataMap.Remove(com);
        }
    }
}