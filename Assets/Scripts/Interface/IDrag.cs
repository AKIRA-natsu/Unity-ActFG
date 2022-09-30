using UnityEngine;

/// <summary>
/// 拖拽接口
/// </summary>
public interface IDrag {
    /// <summary>
    /// 鼠标点击
    /// </summary>
    void OnDragDown();
    /// <summary>
    /// 鼠标抬起
    /// </summary>
    void OnDragUp();
    /// <summary>
    /// 鼠标按住
    /// </summary>
    void OnDrag();
}