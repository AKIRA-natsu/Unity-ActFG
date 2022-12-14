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
    /// <para>加载</para>
    /// <para>ResourceCollection里用的是Mono的Coroutine</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator Load();
}