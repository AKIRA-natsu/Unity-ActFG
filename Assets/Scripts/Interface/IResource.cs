using System.Collections;

/// <summary>
/// 资源接口
/// </summary>
public interface IResource {
    /// <summary>
    /// 顺序
    /// </summary>
    /// <value></value>
    public int order { get; }

    /// <summary>
    /// 加载
    /// </summary>
    /// <returns></returns>
    IEnumerator Load();
}