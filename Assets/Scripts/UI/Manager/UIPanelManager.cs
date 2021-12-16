using System.Collections.Generic;
using ActFG.UI;

namespace ActFG.Manager {
    /// <summary>
    /// UI 管理
    /// </summary>
    public class UIPanelManager : Singleton<UIPanelManager> {
        private Stack<UIBase> stackPanel = new Stack<UIBase>();
        private UIBase panel;

        /// <summary>
        /// UI 入栈，此操作会显示 UI
        /// 只显示一个 UI ，别的 UI 如果开启会暂停
        /// </summary>
        /// <param name="next"></param>
        public void Push(UIBase next) {
            if (stackPanel.Count > 0) {
                var last = stackPanel.Peek();
                last.OnPause();
            }
            stackPanel.Push(next);
            var panel = UIManager.Instance.GetUI(next.data.@enum);
        }

        public void Pop() {
            if (stackPanel.Count > 0)
                stackPanel.Pop().OnExit();
            if (stackPanel.Count > 0)
                stackPanel.Peek().OnResume();
        }
    }
}