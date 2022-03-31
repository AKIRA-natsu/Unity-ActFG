using System.Collections.Generic;
using UnityEngine;
using ActFG.Manager;

namespace ActFG.UIFramework {
    public class UIComponent : UIBase {
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        // 可适配组件列表
        public List<RectTransform> MatchableList = new List<RectTransform>();

        public override void Awake() {
            base.Awake();
            // 初始化创建
            this.gameObject = UIDataManager.Instance.GetUIData(this).path.Load<GameObject>().Instantiate().SetParent(UICanvas.Root, true);
            this.transform = gameObject.transform;
            BindFields();
        }

        /// <summary>
        /// 绑定私有字段
        /// </summary>
        private void BindFields() {
            var fields = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields) {
                var uIControl = (UIControlAttribute)field.GetCustomAttributes(false)[0];
                field.SetValue(this, this.transform.Find(uIControl.Path).GetComponent(field.FieldType));
                if (uIControl.Matchable)
                    MatchableList.Add(this.transform.Find(uIControl.Path).GetComponent<RectTransform>());
            }
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
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destory() {
            this.gameObject.Destory();
        }
    }
}