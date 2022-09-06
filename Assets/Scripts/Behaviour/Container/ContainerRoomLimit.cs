using System;
using UnityEngine;

/// <summary>
/// 容器容量限制
/// </summary>
public class ContainerRoomLimit : MonoBehaviour {
    // 容量与升级挂钩
    public UpgradeType upgradeType = UpgradeType.None;
    // 容器最大值
    public int maxValue = 0;
    // 最大容量
    public int Value => upgradeType == UpgradeType.None ? maxValue : (int)UpgradeManager.Instance.GetUpgradeValue(upgradeType);

    private void Awake() {
        maxValue = maxValue <= 0 ? Int32.MaxValue : maxValue;
    }
}