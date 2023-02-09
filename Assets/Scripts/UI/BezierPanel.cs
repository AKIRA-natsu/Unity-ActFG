using System;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AKIRA.UIFramework {
    /// <summary>
    /// 应对要多个Bezier表现的时候用
    /// </summary>
    [Win(WinEnum.Bezier, "Prefabs/UI/Bezier", WinType.Notify)]
    public class BezierPanel : BezierPanelProp {
        // Tween go 前缀路劲
        private const string Path = "Prefabs/";
        // 画布模式
        private RenderMode mode;

        // 
        // private List<BezierTween> tweens = new List<BezierTween>();

        public override void Awake(object obj) {
            base.Awake(obj);
            mode = UI.Canvas.renderMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">path => "Prefabs/<paramref name="name" />", has BezierTween Behaviour</param>
        /// <param name="anchorPos"></param>
        /// <param name="targetTrans"></param>
        /// <param name="count"></param>
        /// <param name="type">0 到 5</param>
        /// <param name="onComplete"></param>
        /// <param name="duration"></param>
        public void ShowBezier(string name, Vector2 anchorPos, Transform targetTrans, int count = 0, int type = 1, Action onComplete = null, float duration = 1f) {
            TempRect.anchoredPosition = anchorPos;
            this.ShowBezier(name, TempRect.transform.position, targetTrans.position, count, type, onComplete, duration);
        }

        private void ShowBezier(string name, Vector3 start, Vector3 end, int count, int type, Action onComplete, float duration) {
            // tweens.Clear();
            count = Mathf.Clamp(count, 1, 20);
            // for (int i = 0; i < count; i++)
            //     tweens.Add(ObjectPool.Instance.Instantiate<BezierTween>(Path + name, Vector3.zero, Quaternion.identity, TempRect, Space.Self));

            // for (int i = 0; i < count; i++) {
            //     int index = i;
            //     tweens[index].Move(index, start, GetBezierPosByType(type), end, -1,
            //         i == count - 1 ? () => onComplete?.Invoke() : null, duration);
            // }

            for (int i = 0; i < count; i++) {
                var tween = ObjectPool.Instance.Instantiate<BezierTween>(Path + name, Vector3.zero, Quaternion.identity, TempRect, Space.Self);
                tween.Move(i, start, GetBezierPosByType(type), end, -1,
                    i == 0 ? () => onComplete?.Invoke() : null, duration);
            }
        }

        private Vector3 GetBezierPosByType(int typeIndex) {
            var midPos = BezierControlRect.position;
            float offset = mode == RenderMode.ScreenSpaceCamera ? 1 : 100;
            switch (typeIndex) {
                case 1:
                    midPos += new Vector3(Random.Range(1.5f, -1.5f), Random.Range(-2, 0), 0) * offset;
                    break;
                case 2:
                    midPos += new Vector3(Random.Range(3, -3), Random.Range(-3, 0), 0) * offset;
                    break;
                case 3:
                    midPos += new Vector3(Random.Range(6, -6), Random.Range(-3, 3), 0) * offset;
                    break;
                case 4:
                    midPos += new Vector3(Random.Range(6, -6), Random.Range(-3, 0), 0) * offset;
                    break;
                case 5:
                    midPos += new Vector3(Random.Range(7, -6), Random.Range(-8, 0), 0) * offset;
                    break;
            }
            return midPos;
        }
    }
}