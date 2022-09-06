using System;
using AKIRA.Manager;

/// <summary>
/// 升级基础
/// </summary>
public abstract class UpgradeBase {
    /// <summary>
    /// 存储键值
    /// </summary>
    private string ID;

    /// <summary>
    /// 等级
    /// </summary>
    public int level { get; protected set; }
    /// <summary>
    /// 数值
    /// </summary>
    public float value { get; protected set; }
    /// <summary>
    /// 升级消耗
    /// </summary>
    public int cost { get; protected set; }
    /// <summary>
    /// 最高等级
    /// </summary>
    public abstract int maxLevel { get; }
    /// <summary>
    /// 是否到达最高等级
    /// </summary>
    public bool ReachMaxLevel => level >= maxLevel;

    /// <summary>
    /// 升级回调
    /// </summary>
    private Action<UpgradeBase> onUpgradeCallback;

    public UpgradeBase() {
        this.ID = this.ToString();
        // 本地获得等级
        level = ID.GetInt();
        // 计算数值
        value = CalculateValue();
        cost = CalculateCost();
    }

    /// <summary>
    /// 升级
    /// </summary>
    public virtual void Upgrade() {
        // 判断是否到最高等级
        if (ReachMaxLevel)
            return;

        // 保存
        ID.Save(++level);
        // 升级顺序问题
        var curCost = cost;
        // 重新计算数值
        value = CalculateValue();
        cost = CalculateCost();
        // 消费
        MoneyManager.Instance.Gain(-curCost);

        // 回调
        onUpgradeCallback?.Invoke(this);
    }
    
    /// <summary>
    /// 计算数值
    /// </summary>
    protected abstract float CalculateValue();
    
    /// <summary>
    /// 计算升级消费
    /// </summary>
    protected abstract int CalculateCost();

    /// <summary>
    /// 注册升级回调
    /// </summary>
    /// <param name="onUpgradeCallback"></param>
    public void RegistOnUpgradeCallback(Action<UpgradeBase> onUpgradeCallback) {
        onUpgradeCallback?.Invoke(this);
        this.onUpgradeCallback += onUpgradeCallback;
    }

    /// <summary>
    /// 移除升级回调
    /// </summary>
    /// <param name="onUpgradeCallback"></param>
    public void RemoveOnUpgradeCallback(Action<UpgradeBase> onUpgradeCallback) {
        this.onUpgradeCallback -= onUpgradeCallback;
    }
}