using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 点击缩放效果 不受Auto Update影响
/// </summary>
public class ClickPunchSelf : PunchSelf, IPointerDownHandler, IPointerUpHandler {
    protected override void OnEnable() {
        Enable = false;
        punchOffset = originScale * (punchValue - 1f);
    }

    private void OnValidate() {
        punchOffset = originScale * (punchValue - 1f);
    }

    public override void GameUpdate() {
        if (ward == Ward.Forward) {
            if (time >= punchTime)
                return;
            time += Time.deltaTime;
        } else {
            if (time <= 0) {
                Enable = false;
                return;
            }
            time -= Time.deltaTime;
        }

        this.transform.localScale = punchCurve.Evaluate(time / punchTime) * punchOffset + originScale;
    }

    public void OnPointerDown(PointerEventData eventData) {
        ward = Ward.Forward;
        Enable = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        ward = Ward.Backward;
    }
}