using System.Collections.Generic;
using UnityEngine;
using AKIRA.Manager;

namespace AKIRA.UIFramework {
    public class UIComponent : UIBase {
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }

        // 可适配组件列表
        public List<RectTransform> MatchableList { get; private set; } = new List<RectTransform>();

        public override void Awake() {
            base.Awake();
            // 初始化创建
            this.gameObject = UIDataManager.Instance.GetUIData(this).path.Load<GameObject>().Instantiate().SetParent(UI.Root, true);
            this.transform = gameObject.transform;
            BindFields();
        }

        /// <summary>
        /// 绑定私有字段
        /// </summary>
        private void BindFields() {
            var fields = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields) {
                var uIControls = field.GetCustomAttributes(typeof(UIControlAttribute), false);
                if (uIControls.Length == 0) continue;
                var uIControl = uIControls[0] as UIControlAttribute;
                field.SetValue(this, this.transform.Find(uIControl.Path).GetComponent(field.FieldType));
                if (uIControl.Matchable)
                    MatchableList.Add(this.transform.Find(uIControl.Path).GetComponent<RectTransform>());
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show() {
            this.gameObject.SetActive(true);
            this.OnEnter();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide() {
            this.gameObject.SetActive(false);
            this.OnExit();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Destory() {
            this.gameObject.Destory();
        }
    }
}