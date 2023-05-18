using AKIRA.Data;
using AKIRA.Manager;
using DG.Tweening;
using UnityEngine;

namespace AKIRA.UIFramework {
    public class BezierTween : MonoBehaviour, IPool, IUpdate {
        private float tweenTimer;
        private float tweenDuration;
        private Vector3 startPos;
        private Vector3 midPos;
        private Vector3 target;
        private int index;
        private System.Action onComplete;
        private float accelerate;

        private Vector3 scale;

        private void Awake() {
            scale = this.transform.localScale;
        }

        public void Move(int index,Vector3 startPos, Vector3 midPos, Vector3 target, float accelerate,
            System.Action onComplete = null, float tweenDuration=1)
        {
            this.startPos = startPos;
            this.midPos = midPos;
            this.target = target;
            this.accelerate = accelerate;
            this.onComplete = onComplete;
            this.index = index;
            this.tweenDuration = tweenDuration;
            

            gameObject.SetActive(true);

            transform.localScale = scale;
            transform.position = startPos;
            tweenTimer = 0;
        }

        public void GameUpdate()
        {
            if (tweenTimer != tweenDuration)
            {
                transform.position = GetBezierPos(tweenTimer / tweenDuration, startPos, midPos, target);
                tweenTimer += Time.deltaTime * (accelerate == -1 ? (1 + index * 0.1f) : accelerate);
                tweenTimer = Mathf.Clamp(tweenTimer, 0, tweenDuration);
                if (tweenTimer == tweenDuration)
                {
                    onComplete?.Invoke();
                    transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => ObjectPool.Instance.Destory(this));
                }
            }
        }

        public static Vector3 GetBezierPos(float t, Vector3 start, Vector3 center, Vector3 end)
        {
            return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
        }

        public void Wake(object data = null) {
            this.Regist(GameData.Group.UI);
        }

        public void Recycle(object data = null) {
            this.Remove(GameData.Group.UI);
        }
    }
}