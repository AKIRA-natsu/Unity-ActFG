using UnityEngine;

/// <summary>
/// 放置到容器
/// </summary>
public class SetStore : StoreBase {
    protected override void CalculateValue(out float interval, out int preCount) {
        interval = Mathf.Clamp(1f / tarContainer.Room, 0, 0.1f);
        preCount = (int)(Time.smoothDeltaTime / interval) + 1;
    }

    protected override void Store() {
        // 自身容器为空
        if (tarContainer.Empty || selfContainer.ReachMaxRoom)
            return;
        
        var count = Mathf.Min(tarContainer.Room, preCount);
        for (int i = 0; i < count; i++)
            selfContainer.AddRoom(tarContainer.SubRoom());
    }
}