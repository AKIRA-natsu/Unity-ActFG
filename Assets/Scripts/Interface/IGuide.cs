/// <summary>
/// 指引接口
/// </summary>
public interface IGuide {
    /// <summary>
    /// 解锁条件
    /// </summary>
    /// <returns></returns>
    bool UnlockCondition();

    /// <summary>
    /// 完成条件
    /// </summary>
    /// <returns></returns>
    bool FinishCondition();
}