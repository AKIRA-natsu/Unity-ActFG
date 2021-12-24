using System.Collections.Generic;
using ActFG.UIFramework;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIPanelManager : Singleton<UIPanelManager> {
        private Stack<UIComponent> stackPanel = new Stack<UIComponent>();

        private UIPanelManager() {}

        /// <summary>
        /// <para>UI 入栈，此操作会显示 UI</para>
        /// <para>只显示一个 UI ，别的 UI 如果开启会暂停</para>
        /// </summary>
        /// <param name="next"></param>
        public void Push(UIComponent next) {
            if (stackPanel.Count > 0) {
                // 暂停上一个 UI
                var last = stackPanel.Peek();
                last.OnPause();
            }
            stackPanel.Push(next);
            next.Show();
            next.OnEnter();
        }

        /// <summary>
        /// UI 出栈
        /// </summary>
        public void Pop() {
            if (stackPanel.Count > 0) {
                stackPanel.Peek().OnExit();
                stackPanel.Pop();
            }
            if (stackPanel.Count > 0)
                stackPanel.Peek().OnResume();
        }
    }
}