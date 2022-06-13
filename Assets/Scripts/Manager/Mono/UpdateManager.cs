using System;
using System.Collections.Generic;
using AKIRA.Coroutine;
using AKIRA.Manager;

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
/// 更新驱动管理
/// </summary>
public class UpdateManager : MonoSingleton<UpdateManager> {
    // 更新列表
    public Dictionary<UpdateMode, List<IUpdate>> updateMap = new Dictionary<UpdateMode, List<IUpdate>>();

    /// <summary>
    /// 程序是否退出
    /// 单例先被销毁bug
    /// </summary>
    public static bool isFouceOut = false;

    private void Start() {
        // 表初始化
        foreach (var mode in Enum.GetValues(typeof(UpdateMode)))
            updateMap.Add((UpdateMode)mode, new List<IUpdate>());
    }

    /// <summary>
    /// <para>注册更新</para>
    /// <para>只注册无参类型，有参类型根据实际情况父物体遍历子节点更新</para>
    /// <para>不检查是否重复（元素可能过多）</para>
    /// </summary>
    /// <param name="update"></param>
    /// <param name="mode">更新类型</param>
    public void Regist(IUpdate update, UpdateMode mode = UpdateMode.Update) {
        if (updateMap[mode].Contains(update))   
            return;
        updateMap[mode].Add(update);
    }

    /// <summary>
    /// 移除更新
    /// </summary>
    /// <param name="update"></param>
    /// <param name="mode">更新类型</param>
    public void Remove(IUpdate update, UpdateMode mode = UpdateMode.Update) {
        updateMap[mode].Remove(update);
    }

    private void Update() {
        // 协程更新
        CoroutineManager.Instance.UpdateCoroutine();
        // 遍历更新
        for (int i = 0; i < updateMap[UpdateMode.Update].Count; i++)
            updateMap[UpdateMode.Update][i].GameUpdate();
    }

    private void FixedUpdate() {
        // 遍历更新
        for (int i = 0; i < updateMap[UpdateMode.FixedUpdate].Count; i++)
            updateMap[UpdateMode.FixedUpdate][i].GameUpdate();
    }

    private void LateUpdate() {
        // 遍历更新
        for (int i = 0; i < updateMap[UpdateMode.LateUpdate].Count; i++)
            updateMap[UpdateMode.LateUpdate][i].GameUpdate();
    }

    private void OnDisable() {
        isFouceOut = true;
    }
}