using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Transition, "UI/SampleTransition", WinType.Interlude)]
    public class SampleTransitionPanel : SampleTransitionPanelProp {
        // 起始大小
        private Vector3 StartScale = Vector3.one * 0.2f;
        // 起始位置 屏幕中下位置-100f
        private Vector3 StartPosition;
        // 目标大小
        private Vector3 TargetScale = Vector3.one * 10f;
        // 动画速度 统一
        private const float AnimationSpeed = 7f;

        // 过渡事件
        private Action onTransition;
        // 切换场景事件 协程 一次
        private List<IEnumerator> coroutines = new List<IEnumerator>();
        // 过渡结束事件
        private Action onTransitionEnd;

        public override void Awake(WinType type) {
            base.Awake(type);
            Hide();
            StartPosition = Vector3.down * (Screen.height / 2 + 100f);
            TransitionRect.localScale = StartScale;
        }

        /// <summary>
        /// 注册过渡事件
        /// </summary>
        /// <param name="onTransition"></param>
        /// <param name="calledOnce"></param>
        public void RegistTransitionAction(Action onTransition) {
            this.onTransition += onTransition;
        }

        /// <summary>
        /// 注册协程事件
        /// </summary>
        /// <param name="coroutine"></param>
        public void RegistTransitionAction(IEnumerator coroutine) {
            if (coroutines.Contains(coroutine))
                return;
            coroutines.Add(coroutine);
        }

        /// <summary>
        /// 注册过渡结束事件
        /// </summary>
        /// <param name="onTransitionEnd"></param>
        public void RegistTransitionEndAction(Action onTransitionEnd) {
            this.onTransitionEnd += onTransitionEnd;
        }

        /// <summary>
        /// 移除过渡事件
        /// </summary>
        /// <param name="onTransition"></param>
        public void RemoveTransitionAction(Action onTransition) {
            this.onTransition -= onTransition;
        }

        /// <summary>
        /// 移除协程事件
        /// </summary>
        /// <param name="coroutine"></param>
        public void RemoveTransitionAction(IEnumerator coroutine) {
            if (!coroutines.Contains(coroutine))
                return;
            coroutines.Remove(coroutine);
        }

        /// <summary>
        /// 移除过渡结束事件
        /// </summary>
        /// <param name="onTransitionEnd"></param>
        public void RemoveTransitionEndAction(Action onTransitionEnd) {
            this.onTransitionEnd -= onTransitionEnd;
        }

        /// <summary>
        /// 过渡
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public IEnumerator StartTransition(Color color) {
            TransitionImg.color = color;
            yield return StartTransition();
        }

        /// <summary>
        /// 过渡
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartTransition() {
            yield return null;
        
            TransitionRect.anchoredPosition = StartPosition;
            Show();
            // 图片上移动画
            var position = TransitionRect.anchoredPosition;
            while (Vector3.Distance(position, Vector2.zero) >= 0.01f) {
                position = Vector3.Lerp(position, Vector2.zero, Time.deltaTime * AnimationSpeed);
                TransitionRect.anchoredPosition = position;
                yield return null;
            }
            // 图片到达中央，进行放大
            var scale = StartScale;
            while (Vector3.Distance(scale, TargetScale) >= 0.01f) {
                scale = Vector3.Lerp(scale, TargetScale, Time.deltaTime * AnimationSpeed);
                TransitionRect.localScale = scale;
                yield return null;
            }

            // 运行过渡事件
            // 先执行协程事件
            foreach (var coroutine in coroutines)
                yield return coroutine;
            // 执行事件
            onTransition?.Invoke();

            // 事件完成 图片开始缩小
            while (Vector3.Distance(scale, StartScale) >= 0.01f) {
                scale = Vector3.Lerp(scale, StartScale, Time.deltaTime * AnimationSpeed);
                TransitionRect.localScale = scale;
                yield return null;
            }
            // 图片回到开始位置
            while (Vector3.Distance(position, StartPosition) >= 0.01f) {
                position = Vector3.Lerp(position, StartPosition, Time.deltaTime * AnimationSpeed);
                TransitionRect.anchoredPosition = position;
                yield return null;
            }

            // 过渡完成，隐藏页面
            Hide();
            onTransitionEnd?.Invoke();
        }
    }
}