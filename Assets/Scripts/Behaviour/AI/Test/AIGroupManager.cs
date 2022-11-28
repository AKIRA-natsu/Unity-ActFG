using System.Collections.Generic;
using UnityEngine;
using AKIRA.Manager;

/// <summary>
/// 队伍标签
/// </summary>
public enum GroupTag {
    None,
    Enemy,
    Fish,
}

public class AIGroupManager : Singleton<AIGroupManager> {
    // 队伍表
    private static Dictionary<GroupTag, AIGroup> GroupMap = new Dictionary<GroupTag, AIGroup>();

    protected AIGroupManager() {}

    /// <summary>
    /// 注册组
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="group"></param>
    public void RegistGroup(GroupTag tag, AIGroup group) {
        if (GroupMap.ContainsKey(tag)) {
            $"{tag} 已经包含{group}".Colorful(Color.red).Log();
            return;
        }
        GroupMap.Add(tag, group);
    }

    /// <summary>
    /// 获得组
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static AIGroup GetGroup(GroupTag tag) {
        if (GroupMap.ContainsKey(tag))
            return GroupMap[tag];
        else
            return default;
    }
}