using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AKIRA.UIFramework {
    [Win(WinEnum.Transition, "Prefabs/UI/SampleTransition", WinType.Interlude)]
    public class SampleTransitionPanel : SampleTransitionPanelProp {
        // 起始大小
        private Vector3 StartScale = Vector3.one * 0.2f;
        // 起始位置 屏幕中下位置-100f
        private Vector3 StartPosition;
        // 目标大小
        private Vector3 TargetScale = Vector3.one * 10f;
        // 动画速度 统一
        private const float AnimationSpeed = 5f;

        // 过渡事件
        private Action onTransition;
        // 切换场景事件 异步
        private Func<UniTask> task;
        // 过渡结束事件
        private Action onTransitionEnd;

        public override void Awake(object obj) {
            base.Awake(obj);
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
        /// 注册异步事件
        /// </summary>
        /// <param name="task"></param>
        public void RegistTransitionAction(Func<UniTask> task) {
            this.task += task;
        }

        /// <summary>
        /// 注册过渡结束事件
        /// </summary>
        /// <param name="onTransitionEnd"></param>
        public void RegistTransitionEndAction(Action onTransitionEnd) {
            this.onTransitionEnd += onTransitionEnd;
        }

        /// <summary>
        /// 注册固定控制延迟事件
        /// </summary>
        /// <param name="time"></param>
        public void RegistControlDelay(float time) {
            RegistTransitionAction(async () => await UniTask.Delay(Mathf.RoundToInt(time * 1000)));
        }

        /// <summary>
        /// 过渡
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public void StartTransition(Color color) {
            TransitionImg.color = color;
            StartTransition();
        }

        /// <summary>
        /// 过渡
        /// </summary>
        /// <returns></returns>
        [UIBtnMethod]
        private async void StartTransition() {
            TransitionRect.anchoredPosition = StartPosition;
            Show();
            // 图片上移动画
            var position = TransitionRect.anchoredPosition;
            var scale = StartScale;
            while (Vector3.Distance(position, Vector2.zero) >= 0.01f || Vector3.Distance(scale, TargetScale) >= 0.01f) {
                await UniTask.DelayFrame(0);
                position = Vector3.Lerp(position, Vector2.zero, Time.deltaTime * AnimationSpeed);
                TransitionRect.anchoredPosition = position;
                // 同步图片开始方法放大
                if (position.y >= -Screen.height / 8) {
                    scale = Vector3.Lerp(scale, TargetScale, Time.deltaTime * AnimationSpeed);
                    TransitionRect.localScale = scale;
                }
            }

            // 运行过渡事件
            // 执行事件
            onTransition?.Invoke();
            await UniTask.Delay(100);
            // 执行异步事件
            if (task != null)
                await task.Invoke();

            // 事件完成 图片开始缩小
            while (Vector3.Distance(scale, StartScale) >= 0.01f || Vector3.Distance(position, StartPosition) >= 0.01f) {
                await UniTask.DelayFrame(0);
                scale = Vector3.Lerp(scale, StartScale, Time.deltaTime * AnimationSpeed);
                TransitionRect.localScale = scale;
                // 同步位置开始回到初始位置
                if (scale.x <= TargetScale.x / 8) {
                    position = Vector3.Lerp(position, StartPosition, Time.deltaTime * AnimationSpeed);
                    TransitionRect.anchoredPosition = position;
                }
            }

            // 过渡完成，隐藏页面
            Hide();
            onTransitionEnd?.Invoke();

            onTransition = onTransitionEnd = null;
            task = null;
        }
    }
}