using System.Collections.Generic;
using System;
using UnityEngine;
using AKIRA.Manager;
using AKIRA.UIFramework;

/// <summary>
/// 升级管理
/// </summary>
public class UpgradeManager : Singleton<UpgradeManager> {
    // 升级字典
    private Dictionary<UpgradeType, UpgradeBase> upgradeMap = new Dictionary<UpgradeType, UpgradeBase>();

    private UpgradeManager() {
        foreach (UpgradeType value in Enum.GetValues(typeof(UpgradeType))) {
            if (value == UpgradeType.None)
                continue;
            var type = value.ToString().GetConfigTypeByAssembley();
            var upgradeBase = type.CreateInstance<UpgradeBase>();
            upgradeMap.Add(value, upgradeBase);
        }
    }

    /// <summary>
    /// 获得升级能力值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public UpgradeBase GetUpgrade(UpgradeType type)
        => upgradeMap[type];

    /// <summary>
    /// 注册升级回调
    /// </summary>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void RegistUpgradeCallback(UpgradeType type, Action<UpgradeBase> callback) {
        upgradeMap[type].RegistOnUpgradeCallback(callback);
    }

    /// <summary>
    /// 注册升级回调
    /// </summary>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void RemoveUpgradeCallback(UpgradeType type, Action<UpgradeBase> callback) {
        upgradeMap[type].RemoveOnUpgradeCallback(callback);
    }
}