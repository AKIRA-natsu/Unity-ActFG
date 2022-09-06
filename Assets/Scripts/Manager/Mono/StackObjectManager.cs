using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

/// <summary>
/// 堆叠物工厂管理器
/// </summary>
public class StackObjectManager : MonoSingleton<StackObjectManager> {
    // 可收集物品列表
    public CollectableObjectBase[] collectableObjectBases;

    // 可收集物品字典
    private Dictionary<StackObjectType, CollectableObjectBase> collectableObjectMap = new Dictionary<StackObjectType, CollectableObjectBase>();

    protected override void Awake() {
        base.Awake();
        foreach (var obj in collectableObjectBases)
            collectableObjectMap.Add(obj.stackObjectType, obj);
    }

    /// <summary>
    /// 实例化获得某类型物品
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public CollectableObjectBase GetCollectableObject(StackObjectType type) {
        return ObjectPool.Instance.Instantiate(collectableObjectMap[type]);
    }

    /// <summary>
    /// 实例化获得某类型物品
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public CollectableObjectBase GetCollectableObject(StackObjectType type, Vector3 position, Quaternion rotation) {
        return ObjectPool.Instance.Instantiate(collectableObjectMap[type], position, rotation);
    }

    /// <summary>
    /// 回收收集物品
    /// </summary>
    /// <param name="collectableObjectBase"></param>
    public void RecycleCollectableObject(CollectableObjectBase collectableObjectBase) {
        ObjectPool.Instance.Destory(collectableObjectBase);
    }
}
