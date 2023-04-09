/// <summary>
/// 可交互物品
/// </summary>
public interface IMutual {
    /// <summary>
    /// 拾起
    /// </summary>
    /// <returns>是否拾起成功</returns>
    bool Pick();

    /// <summary>
    /// 使用
    /// </summary>
    /// <returns>是否使用成功</returns>
    bool Use();

    /// <summary>
    /// 丢弃
    /// </summary>
    /// <returns>是否丢弃成功</returns>
    bool Discard();
}