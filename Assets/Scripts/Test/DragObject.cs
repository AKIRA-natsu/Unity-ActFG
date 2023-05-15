using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AKIRA.Test {
    public class DragObject : MonoBehaviour, IDrag {
        // 原本高度
        private float y;
        // Y偏移量
        public float yOffset;

        private Tween moveTween;

        // 显示图片
        public RectTransform image;

        private void Awake() {
            y = this.transform.position.y;
        }

        private void Start() {
            OutScreenMark.WidthOffset = 50f;
            OutScreenMark.HeightOffset = 50f;
        }

        public void OnDrag() {
            var position = this.GetDragPosition(this.transform.position, yOffset);
            position.y = this.transform.position.y;
            this.transform.position = Vector3.Lerp(this.transform.position, position, Time.deltaTime * 10);
            var result = this.transform.position.TryGetIntersectPoint();
            
            if (image == null)
                return;

            if (result.isCross) 
                // FIXME: 测试没有UIFrameword.UI 使用不需要参数
                image.anchoredPosition = result.crossPoint.ScreenToUGUI(image.parent.GetComponent<RectTransform>());
            else
                image.anchoredPosition = Vector2.up * Screen.height * 2;
        }

        public void OnDragDown() {
            moveTween?.Kill();
            moveTween = this.transform.DOLocalMoveY(y + yOffset, 0.3f).SetEase(Ease.InBack);
        }

        public void OnDragUp() {
            moveTween?.Kill();
            moveTween = this.transform.DOLocalMoveY(y, 0.3f).SetEase(Ease.OutBack);
        }
    }
}
