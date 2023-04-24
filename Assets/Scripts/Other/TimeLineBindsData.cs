using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;
using UnityEngine.Playables;

/// <summary>
/// Timeline绑定字段
/// </summary>
[Serializable]
public class TimeLineBindsData {
    /// <summary>
    /// 标识
    /// </summary>
    public string tag;
    /// <summary>
    /// 动画
    /// </summary>
    public TimelineAsset timeline;

    /// <summary>
    /// 绑定字段字典
    /// </summary>
    private Dictionary<string, Object> bindMap = new();

    // 初始化
    private void Inited() {
        if (bindMap.Count != 0)
            return;
        foreach (var bind in timeline.outputs) {
            bindMap.Add(bind.streamName, bind.sourceObject);
        }
    }

    /// <summary>
    /// 设置 <see cref="director" /> 为当前类的Timeline
    /// </summary>
    /// <param name="director"></param>
    public void SetTimeline(PlayableDirector director) {
        Inited();
        director.playableAsset = timeline;
    }

    /// <summary>
    /// 设置BindValue
    /// </summary>
    /// <param name="director"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetGenericBinding(PlayableDirector director, string key, Object value) {
        Inited();
        if (!bindMap.ContainsKey(key))
            return;
        director.SetGenericBinding(bindMap[key], value);
    }

}