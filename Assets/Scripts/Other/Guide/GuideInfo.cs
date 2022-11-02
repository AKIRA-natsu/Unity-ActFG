using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 指引完成方式类型
/// </summary>
public enum GuideCompleteType {
    [InspectorName("无")]
    None,
    [InspectorName("UI世界")]
    UIWorld,
    [InspectorName("3D世界")]
    TDWorld,
}

/// <summary>
/// 指引文本位置
/// </summary>
public enum GuideDialogDirection {
    [InspectorName("不显示")]
    None,
    [InspectorName("上")]
    Up,
    [InspectorName("下")]
    Down,
    [InspectorName("左")]
    Left,
    [InspectorName("右")]
    Right,
    [InspectorName("中心")]
    Center,
}

[Serializable]
public class GuideInfo {
    /// <summary>
    /// 识别ID
    /// </summary>
    public int ID;

    /// <summary>
    /// 完成方式
    /// </summary>
    public GuideCompleteType completeType;
    /// <summary>
    /// 根据指引接口完成
    /// </summary>
    public bool controlByIGuide = false;

    /// <summary>
    /// 是否显示遮罩
    /// </summary>
    public bool isShowBg = false;

    /// <summary>
    /// 提示文本
    /// </summary>
    public string dialog;
    /// <summary>
    /// 提示文本位置
    /// </summary>
    public GuideDialogDirection dialogDirection = GuideDialogDirection.None;

    /// <summary>
    /// 箭头指向目标
    /// </summary>
    public GameObject arrowTarget;
    /// <summary>
    /// 到达距离
    /// </summary>
    public float reachDistance = 0f;

#if UNITY_EDITOR
    /// <summary>
    /// 是否可以提供
    /// </summary>
    /// <returns></returns>
    public bool IsVailiable { get; private set; }

    /// <summary>
    /// 获得指引UGUI颜色及指引信息
    /// </summary>
    /// <returns></returns>
    public (Color Color, string WarnMessage) GetInfoGUIMessage() {
        (Color Color, string WarnMessage) result = (Color.white, default);
        IsVailiable = false;
        if (completeType == GuideCompleteType.None) {
            result = (Color.cyan, default);
            return result;
        }

        if (arrowTarget == null) {
            result = (Color.cyan, "指引目标为空");
            return result;
        }

        if (completeType == GuideCompleteType.UIWorld && !arrowTarget.GetComponent<RectTransform>()) {
            result = (Color.yellow, "指引的对象不是UI对象");
            return result;
        }

        if (completeType == GuideCompleteType.TDWorld && arrowTarget.GetComponent<RectTransform>()) {
            result = (Color.yellow, "指引对象不是3D物体");
            return result;
        }

        IsVailiable = true;
        
        if (controlByIGuide) {
            result = (Color.green, "根据IGuide接口完成指引");
            return result;
        }

        return result;
    }
#endif
}