using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.UIFramework {
    /// <summary>
    /// <para>适用UI组件 基类</para>
    /// </summary>
    public class UIComponentProp : UIBase {
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        // 优化UI页面的显示与隐藏
        protected CanvasGroup group;

        // 可适配组件列表
        public List<RectTransform> MatchableList { get; private set; } = new List<RectTransform>();
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active { get; protected set; } = true;

        public override void Awake(object obj) {
            this.transform = (Transform)obj;
            this.gameObject = this.transform.gameObject;
            BindFields();
            group = this.gameObject.AddComponent<CanvasGroup>();
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

                if (field.FieldType.IsSubclassOf(typeof(UIComponentProp))) {
                    var componentProp = field.FieldType.CreateInstance<UIComponentProp>();
                    componentProp.Awake(this.transform.Find(uIControl.Path));
                    field.SetValue(this, componentProp);
                } else {
                    field.SetValue(this, this.transform.Find(uIControl.Path).GetComponent(field.FieldType));
                }

                if (uIControl.Matchable)
                    MatchableList.Add(this.transform.Find(uIControl.Path).GetComponent<RectTransform>());
            }
        }

        /// <summary>
        /// 内部获得UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Get<T>() where T : UIComponent {
            return UIManager.Instance.Get<T>();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show() {
            // this.gameObject.SetActive(true);
            group.alpha = 1f;
            group.blocksRaycasts = true;
            group.interactable = true;
            Active = true;
            this.OnEnter();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide() {
            // this.gameObject.SetActive(false);
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;
            Active = false;
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