using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片闪光
/// 范围 1 - -1
/// </summary>
[RequireComponent(typeof(ShinImg))]
public class ShinImg : MonoBehaviour, IUpdate {
    // 闪光连续次数
    [SerializeField, Min(0)]
    private int continuousCount;
    // 闪光次数间隔时间
    [SerializeField, Min(0f)]
    private float interval;
    // 闪光速度
    [SerializeField, Min(0f)]
    private float speed;
    // 闪光图片
    private RawImage image;
    // 初始位置
    private readonly Rect Origin = new Rect(1, 0, 1, 1);

    // 变化值
    private float value = 1;
    // 当前次数
    private int curCount;
    // 上一次间隔时间
    private float lastIntervalTime;

    private void Awake() {
        image = this.GetComponent<RawImage>();
    }

    private void OnEnable() {
        this.Regist();
        image.uvRect = Origin;
    }

    private void OnDisable() {
        this.Remove();
    }

    public void GameUpdate() {
        if (curCount == continuousCount) {
            if (Time.time - lastIntervalTime <= interval)
                return;
            
            curCount = 0;
        }

        if (value <= -1) {
            value = 1;
            image.uvRect = Origin;
            if (++curCount == continuousCount)
                lastIntervalTime = Time.time;
        } else {
            value = Mathf.Lerp(value, -1.1f, Time.deltaTime * speed);
            image.uvRect = new Rect(value, 0, 1, 1);
        }
    }
}