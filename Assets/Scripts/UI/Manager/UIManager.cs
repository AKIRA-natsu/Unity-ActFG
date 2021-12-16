using System.Collections.Generic;
using UnityEngine;
using ActFG.UI;
using ActFG.Util.Tools;

namespace ActFG.Manager {
    /// <summary>
    /// UI x GameObject 管理
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        private Dictionary<WinEnum, GameObject> UIMap = new Dictionary<WinEnum, GameObject>();

        /// <summary>
        /// 获得一个 UI
        /// </summary>
        /// <param name="@enum"> UI 数据 </param>
        /// <returns></returns>
        public GameObject GetUI(WinEnum @enum) {
            // FIXME: 改掉，父节点不可能永远都是Canvas，并且判断不应该在取出之前
            GameObject parent = GameObject.Find("Canvas");
            if (!parent) {
                $"{parent.name} dont find".StringColor(Color.red).Log();
                return null;
            }

            if (UIMap.ContainsKey(@enum))
                return UIMap[@enum];
            // 不存在 UI => 创建
            var UI = UIDataManager.Instance.GetUIData(@enum).path.Load<GameObject>().Instantiate().SetParent(parent);
            return UI;
        }

        /// <summary>
        /// 销毁 UI
        /// </summary>
        /// <param name="@enum"></param>
        public void DestoryUI(WinEnum @enum) {
            if (UIMap.ContainsKey(@enum)) {
                UIMap[@enum].Destory();
                UIMap.Remove(@enum);
                UIDataManager.Instance.Remove(@enum);
            }
        }
    }
}