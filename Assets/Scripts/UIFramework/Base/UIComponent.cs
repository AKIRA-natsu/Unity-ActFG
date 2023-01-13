using System;
using System.Collections.Generic;
using UnityEngine;
using AKIRA.Manager;

namespace AKIRA.UIFramework {
    public abstract class UIComponent : UIBase {
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        // 优化UI页面的显示与隐藏
        protected CanvasGroup group;

        // 可适配组件列表
        public List<RectTransform> MatchableList { get; private set; } = new List<RectTransform>();

        public override void Awake(WinType type) {
            // 初始化创建
            this.gameObject = UIDataManager.Instance.GetUIData(this).path.Load<GameObject>()
                                                .Instantiate()
                                                .SetParent(GetParent(type), true);
            this.transform = gameObject.transform;
            BindFields();

            group = this.gameObject.AddComponent<CanvasGroup>();
        }

        /// <summary>
        /// 获得UI Game Object父节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private GameObject GetParent(WinType type) {
            switch (type) {
                case WinType.Normal:
                    return UI.View;
                case WinType.Interlude:
                    return UI.Top;
                case WinType.Notify:
                    return UI.Background;
                default:
                    return UI.View;
            }
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
        /// 内部获得UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Get<T>() where T : UIComponent {
            return UIManager.Instance.Get<T>();
        }

        /// <summary>
        /// 内部注册UI事件
        /// </summary>
        /// <param name="action"></param>
        protected void RegistAfterUIInited(Action action) {
            UIManager.Instance.RegistAfterUIIInitAction(action);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show() {
            // this.gameObject.SetActive(true);
            group.alpha = 1f;
            group.blocksRaycasts = true;
            group.interactable = true;
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