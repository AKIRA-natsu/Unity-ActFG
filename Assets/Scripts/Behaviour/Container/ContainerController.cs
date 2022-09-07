using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Container控制器</para>
/// <para>多个容量</para>
/// </summary>
[SelectionBase]
public class ContainerController : MonoBehaviour {
    // 配置容器表
    public ContainerObject[] cinfos;

    // 容器对应表
    private Dictionary<StackObjectType, ContainerObject> containerMap = new Dictionary<StackObjectType, ContainerObject>();

    private void Awake() {
        foreach (var info in cinfos)
            containerMap.Add(info.stackObjectType, info);
    }

    /// <summary>
    /// 获得容器
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ContainerObject GetContainer(StackObjectType type) {
        if (containerMap.ContainsKey(type))
            return containerMap[type];
        $"容器控制器内类型{type}不存在".Error();
        return default;
    }

    /// <summary>
    /// 清空容器列表
    /// </summary>
    public void Clear() {
        foreach (var container in containerMap.Values)
            container.Clear();
        containerMap.Clear();
    }

    #region  项目 石头和沙子合一起原因特殊处理部分
    /// <summary>
    /// 是否达到最大容量
    /// 只要其中一个满了就true
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public bool ReachMaxRoom(params StackObjectType[] types) {
        foreach (var type in types) {
            // 只要类型有一个满了就返回满
            if (GetContainer(type).ReachMaxRoom)
                return true;
        }
        return false;
    }
    #endregion
}