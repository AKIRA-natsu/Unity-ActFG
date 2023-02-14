using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>给Button使用的未点击时缩放动效</para>
/// <para>punchTime需要统一！！</para>
/// </summary>
[RequireComponent(typeof(Button))]
public class UnClickPunchSelf : PunchSelf, IPointerDownHandler, IPointerUpHandler {
    // 自身按钮
    private Button selfButton;
    // 重写静态PunchWard
    private static Ward ward = Ward.Forward;
    // 重写静态时间time
    private static float time = 0f;
    // 实例计数
    private static int useInstanceCount = 0;

    protected override void Awake() {
        base.Awake();
        selfButton = this.GetComponent<Button>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        useInstanceCount++;
    }

    private void OnDisable() {
        useInstanceCount--;
    }

    public override void GameUpdate() {
        // 维持静态的time增量与Time.deltaTime一致
        var deltaTime = Time.deltaTime / useInstanceCount;

        if (ward == Ward.Forward) {
            time += deltaTime;
            if (time >= punchTime)
                ward = Ward.Backward;
        } else {
            time -= deltaTime;
            if (time <= 0)
                ward = Ward.Forward;
        }

        if (!selfButton.interactable)
            return;

        this.transform.localScale = punchCurve.Evaluate(time / punchTime) * punchOffset + originScale;
    }

    public void OnPointerDown(PointerEventData eventData) {
        this.transform.localScale = originScale;
        Enable = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Enable = true;
    }
}