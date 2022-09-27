using System;
using System.Collections.Generic;
using AKIRA.Coroutine;
using AKIRA.Manager;
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
/// 更新驱动管理
/// </summary>
public class UpdateManager : MonoSingleton<UpdateManager> {
    // 更新列表
    private Dictionary<UpdateMode, List<IUpdate>> updateMap = new Dictionary<UpdateMode, List<IUpdate>>();
    // 间隔更新列表
    private Dictionary<UpdateMode, List<SpaceUpdateInfo>> spaceUpdateMap = new Dictionary<UpdateMode, List<SpaceUpdateInfo>>();

    /// <summary>
    /// 程序是否退出
    /// 单例先被销毁bug
    /// </summary>
    public static bool isApplicationOut { get; private set; } = false;

    protected override void Awake() {
        base.Awake();
        // 表初始化
        foreach (var mode in Enum.GetValues(typeof(UpdateMode))) {
            updateMap.Add((UpdateMode)mode, new List<IUpdate>());
            spaceUpdateMap.Add((UpdateMode)mode, new List<SpaceUpdateInfo>());
        }
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

    private void Update() {
        // 协程更新
        CoroutineManager.Instance.UpdateCoroutine();
        Update(UpdateMode.Update);
    }

    private void FixedUpdate()
        => Update(UpdateMode.FixedUpdate);

    private void LateUpdate()
        => Update(UpdateMode.LateUpdate);

    /// <summary>
    /// <paramref name="mode" />更新
    /// </summary>
    /// <param name="mode"></param>
    private void Update(UpdateMode mode) {
        // 遍历更新
        var updateList = updateMap[mode];
        for (int i = 0; i < updateList.Count; i++)
            updateList[i].GameUpdate();
        // 遍历更新间隔
        var spaceUpdateList = spaceUpdateMap[mode];
        for (int i = 0; i < spaceUpdateList.Count; i++) {
            var info = spaceUpdateList[i];
            if (Time.time - info.lastUpdateTime <= info.interval)
                continue;
            // 更新并更新时间
            info.iupdate.GameUpdate();
            info.lastUpdateTime = Time.time;
        }
    }

    private void OnDisable() {
        isApplicationOut = true;
    }
}