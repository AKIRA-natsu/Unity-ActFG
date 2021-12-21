using UnityEngine;
using ActFG.Util.Tools;
using ActFG.Manager;

namespace ActFG.UIFramework {
    public class UIComponent : UIBase{
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }

        public override void Awake() {
            base.Awake();

            GameObject parent = GameObject.Find("Canvas");
            if (!parent) {
                $"{parent.name} dont find".StringColor(Color.red).Log();
            }
            // 初始化创建
            this.gameObject = UIDataManager.Instance.GetUIData(this).path.Load<GameObject>().Instantiate().SetParent(parent, true);
            this.transform = gameObject.transform;
        }

        /// <summary>
        /// 显示 入栈操作在 UIPanelManager 中
        /// </summary>
        public virtual void Show() {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏 出栈
        /// </summary>
        public virtual void Hide() {
            this.gameObject.SetActive(false);
            UIPanelManager.Instance.Pop();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destory() {
            this.gameObject.Destory();
        }
    }
}