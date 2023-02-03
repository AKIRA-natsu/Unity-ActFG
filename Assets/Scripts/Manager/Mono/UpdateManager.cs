using System;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;
using Time = UnityEngine.Time;

/// <summary>
/// 更新模式
/// </summary>
public enum UpdateMode {
    /// <summary>
    ///  
    /// </summary>
    Update,
    /// <summary>
    /// 
    /// </summary>
    FixedUpdate,
    /// <summary>
    /// 
    /// </summary>
    LateUpdate,
}

/// <summary>
/// 间隔更新信息
/// </summary>
public class SpaceUpdateInfo {
    // 更新主体
    public IUpdate iupdate { get; private set; }
    // 间隔时间
    public float interval { get; private set; }
    // 上一个更新时间
    public float lastUpdateTime { get; set; }

    public SpaceUpdateInfo(IUpdate iupdate, float interval, float lastUpdateTime) {
        this.iupdate = iupdate;
        this.interval = interval;
        this.lastUpdateTime = lastUpdateTime;
    }
}

/// <summary>
/// 更新组
/// </summary>
public class UpdateGroup : ReferenceBase {
    // 更新列表
    private Dictionary<UpdateMode, List<IUpdate>> updateMap = new Dictionary<UpdateMode, List<IUpdate>>();
    // 间隔更新列表
    private Dictionary<UpdateMode, List<SpaceUpdateInfo>> spaceUpdateMap = new Dictionary<UpdateMode, List<SpaceUpdateInfo>>();

    // 更新列表 面板
    public IReadOnlyDictionary<UpdateMode, List<IUpdate>> UpdateMap => updateMap;
    // 间隔更新列表 面板
    public IReadOnlyDictionary<UpdateMode, List<SpaceUpdateInfo>> SpaceUpdateMap => spaceUpdateMap;

    // 更新中
    public bool Updating { get; set; } = true;

    public UpdateGroup() {
        // 表初始化
        foreach (UpdateMode mode in Enum.GetValues(typeof(UpdateMode))) {
            updateMap.Add(mode, new List<IUpdate>());
            spaceUpdateMap.Add(mode, new List<SpaceUpdateInfo>());
        }
    }

    public override void Wake() {
        base.Wake();
        Updating = true;
    }

    public override void Recycle() {
        base.Recycle();
        Updating = false;
        foreach (var value in updateMap.Values)
            value.Clear();
        foreach (var value in spaceUpdateMap.Values)
            value.Clear();
    }

    /// <summary>
    /// <para>注册更新 <paramref name="update" /></para>
    /// <para>只注册无参类型，有参类型根据实际情况父物体遍历子节点更新</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="mode">更新类型</param>
    public void Regist(IUpdate update, UpdateMode mode = UpdateMode.Update) {
        if (updateMap[mode].Contains(update))   
            return;
        updateMap[mode].Add(update);
    }

    /// <summary>
    /// 注册间隔更新 <paramref name="update" />
    /// </summary>
    /// <param name="update"></param>
    /// <param name="interval"></param>
    /// <param name="mode"></param>
    public void Regist(IUpdate update, float interval, UpdateMode mode = UpdateMode.Update) {
        // 查重
        var result = spaceUpdateMap[mode];
        foreach (var info in result)
            if (info.iupdate.Equals(update))
                return;
        result.Add(new SpaceUpdateInfo(update, interval, Time.time));
    }

    /// <summary>
    /// 移除更新
    /// </summary>
    /// <param name="update"></param>
    /// <param name="mode">更新类型</param>
    public void Remove(IUpdate update, UpdateMode mode = UpdateMode.Update) {
        if (!updateMap[mode].Contains(update))
            return;
        updateMap[mode].Remove(update);
    }

    /// <summary>
    /// 移除间隔更新
    /// </summary>
    /// <param name="update"></param>
    /// <param name="mode"></param>
    public void RemoveSpaceUpdate(IUpdate update, UpdateMode mode = UpdateMode.Update) {
        var result = spaceUpdateMap[mode];
        for (int i = 0; i < result.Count; i++) {
            var info = result[i];
            if (info.iupdate.Equals(update)) {
                result.Remove(info);
                return;
            }
        }
    }
}

/// <summary>
/// 更新驱动管理
/// </summary>
public class UpdateManager : MonoSingleton<UpdateManager> {
    // 更新组列表
    private Dictionary<string, UpdateGroup> groupMap = new Dictionary<string, UpdateGroup>();
    // 默认组
    public const string Default = "Default";

    // 更新组 面板
    public IReadOnlyDictionary<string, UpdateGroup> GroupMap => groupMap;

    /// <summary>
    /// <para>注册更新 <paramref name="update" /></para>
    /// <para>只注册无参类型，有参类型根据实际情况父物体遍历子节点更新</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode">更新类型</param>
    public void Regist(IUpdate update, string key = Default, UpdateMode mode = UpdateMode.Update) {
        if (groupMap.ContainsKey(key)) {
            groupMap[key].Regist(update, mode);
        } else {
            var group = this.Attach<UpdateGroup>();
            group.Regist(update, mode);
            groupMap.Add(key, group);
        }
    }

    /// <summary>
    /// 注册间隔更新 <paramref name="update" />
    /// </summary>
    /// <param name="update"></param>
    /// <param name="interval"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public void Regist(IUpdate update, float interval, string key = Default, UpdateMode mode = UpdateMode.Update) {
        if (groupMap.ContainsKey(key)) {
            groupMap[key].Regist(update, interval, mode);
        } else {
            var group = this.Attach<UpdateGroup>();
            group.Regist(update, interval, mode);
            groupMap.Add(key, group);
        }
    }

    /// <summary>
    /// 移除更新
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode">更新类型</param>
    public void Remove(IUpdate update, string key = Default, UpdateMode mode = UpdateMode.Update) {
        if (groupMap.ContainsKey(key)) {
            groupMap[key].Remove(update, mode);
        } else {
            $"Update Log Message: Remove {key} Not Find!".Colorful(Color.yellow).Log();
        }
    }

    /// <summary>
    /// 移除间隔更新
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public void RemoveSpaceUpdate(IUpdate update, string key = Default, UpdateMode mode = UpdateMode.Update) {
        if (groupMap.ContainsKey(key)) {
            groupMap[key].RemoveSpaceUpdate(update, mode);
        } else {
            $"Update Log Message: Remove {key} Not Find!".Colorful(Color.yellow).Log();
        }
    }

    /// <summary>
    /// 移除组
    /// </summary>
    /// <param name="key"></param>
    public void DetachGroup(string key) {
        if (groupMap.ContainsKey(key)) {
            this.Detach(groupMap[key]);
            groupMap.Remove(key);
        } else {
            $"Update Log Message: Detach Group {key} Not Find!".Colorful(Color.yellow).Log();
        }
    }

    /// <summary>
    /// 开启/关闭组的更新
    /// </summary>
    /// <param name="key"></param>
    /// <param name="enable"></param>
    public void EnableGroupUpdate(string key, bool enable) {
        if (groupMap.ContainsKey(key)) {
            groupMap[key].Updating = enable;
        } else {
            $"Update Log Message: Enable/Disable Group Update {key} Not Find!".Colorful(Color.yellow).Log();
        }
    }

    private void Update()
        => Update(UpdateMode.Update);

    private void FixedUpdate()
        => Update(UpdateMode.FixedUpdate);

    private void LateUpdate()
        => Update(UpdateMode.LateUpdate);

    /// <summary>
    /// <paramref name="mode" />更新
    /// </summary>
    /// <param name="mode"></param>
    private void Update(UpdateMode mode) {
        var groups = new List<UpdateGroup>(groupMap.Values);
        for (int i = 0; i < groups.Count; i++) {
            var group = groups[i];
            // 停止组更新
            if (!group.Updating)
                continue;
            // 遍历更新
            var updateList = group.UpdateMap[mode];
            for (int j = 0; j < updateList.Count; j++)
                updateList[j].GameUpdate();
            // 遍历更新间隔
            var spaceUpdateList = group.SpaceUpdateMap[mode];
            for (int j = 0; j < spaceUpdateList.Count; j++) {
                var info = spaceUpdateList[j];
                if (Time.time - info.lastUpdateTime <= info.interval)
                    continue;
                // 更新并更新时间
                info.iupdate.GameUpdate();
                info.lastUpdateTime = Time.time;
            }
        }
    }
}

/// <summary>
/// 更新扩展
/// </summary>
public static class UpdateExtend {
    /// <summary>
    /// <para>注册更新</para>
    /// <para>等同于 UpdateManager.Instance.Regist</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public static void Regist(this IUpdate update, string key = UpdateManager.Default, UpdateMode mode = UpdateMode.Update) {
        UpdateManager.Instance.Regist(update, key, mode);
    }

    /// <summary>
    /// <para>注册间隔更新</para>
    /// <para>等同于 UpdateManager.Instance.Regist</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="interval"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public static void Regist(this IUpdate update, float interval, string key = UpdateManager.Default, UpdateMode mode = UpdateMode.Update) {
        UpdateManager.Instance.Regist(update, interval, key, mode);
    }

    /// <summary>
    /// <para>移除更新</para>
    /// <para>等同于 UpdateManager.Instance.Remove</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public static void Remove(this IUpdate update, string key = UpdateManager.Default, UpdateMode mode = UpdateMode.Update) {
        if (UpdateManager.IsApplicationOut)
            return;
        UpdateManager.Instance.Remove(update, key, mode);
    }

    /// <summary>
    /// <para>移除更新</para>
    /// <para>等同于 UpdateManager.Instance.RemoveSpaceUpdate</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="key">组键值</param>
    /// <param name="mode"></param>
    public static void RemoveSpaceUpdate(this IUpdate update, string key = UpdateManager.Default, UpdateMode mode = UpdateMode.Update) {
        if (UpdateManager.IsApplicationOut)
            return;
        UpdateManager.Instance.RemoveSpaceUpdate(update, key, mode);
    }

    /// <summary>
    /// 启用组更新
    /// </summary>
    /// <param name="key"></param>
    public static void EnableGroupUpdate(this string key, bool updating) {
        UpdateManager.Instance.EnableGroupUpdate(key, updating);
    }
}