/// <summary>
/// 对象池接口
/// </summary>
public interface IPool {
    /// <summary>
    /// 唤醒
    /// </summary>
    void Wake();
    /// <summary>
    /// 回收
    /// </summary>
    void Recycle();
}