using System.Collections.Generic;
using ActFG.UI;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// UI 数据管理
    /// </summary>
    public class UIDataManager : Singleton<UIDataManager> {
        private Dictionary<WinEnum, WinData> WinMap = new Dictionary<WinEnum, WinData>();

        /// <summary>
        /// 注册 UI
        /// </summary>
        public void Register(WinEnum @enum, WinData data) {
            if (WinMap.ContainsKey(@enum)) {
                $"WinMap contains {@enum}".Error();
                return;
            }
            WinMap[@enum] = data;
        }

        /// <summary>
        /// 获得 UI 数据
        /// </summary>
        public WinData GetUIData(WinEnum @enum) {
            if (!WinMap.ContainsKey(@enum)) {
                $"WinMap dont contain {@enum}".Error();
                return null;
            }
            return WinMap[@enum];
        }

        /// <summary>
        /// 移除 UI
        /// </summary>
        public void Remove(WinEnum @enum) {
            if (!WinMap.ContainsKey(@enum)) {
                $"WinMap dont contain {@enum}".Error();
                return;
            }
            WinMap.Remove(@enum);
        }
    }
}