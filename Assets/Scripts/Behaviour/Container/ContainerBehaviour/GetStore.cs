using UnityEngine;

/// <summary>
/// 从容器拿取
/// </summary>
public class GetStore : StoreBase {
    protected override void CalculateValue(out float interval, out int preCount) {
        interval = Mathf.Clamp(1f / selfContainer.Room, 0, 0.1f);
        preCount = (int)(Time.smoothDeltaTime / interval) + 1;
    }

    protected override void Store() {
        // 自身容器为空
        if (selfContainer.Empty || tarContainer.ReachMaxRoom)
            return;
        
        var count = Mathf.Min(selfContainer.Room, preCount);
        for (int i = 0; i < count; i++)
            tarContainer.AddRoom(selfContainer.SubRoom());
    }
}