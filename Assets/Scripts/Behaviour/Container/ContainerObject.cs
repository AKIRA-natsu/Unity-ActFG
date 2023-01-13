using System;
using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

/// <summary>
/// <para>容器物品</para>
/// <para>一个容器对应一个收集类型，对应一个排序方式</para>
/// </summary>
[SelectionBase]
public class ContainerObject : MonoBehaviour {
    // 存储键值
    private string ID;

    [CNName("堆叠类型")]
    // 堆叠类型
    public StackObjectType stackObjectType;
    // 堆叠物 容器
    private Stack<CollectableObjectBase> collections = new Stack<CollectableObjectBase>();
    // 容器表现实体（排序）
    public ContainerSortBase containerSortBase { get; private set; }
    // 容器容量限制
    public ContainerRoomLimit roomLimit { get; private set; }

    // 容器改变事件
    private Action<int> onRoomChange;

    /// <summary>
    /// 容器容量
    /// </summary>
    public int Room => collections.Count;
    /// <summary>
    /// 是否达到最大容量
    /// </summary>
    public bool ReachMaxRoom => roomLimit ? Room >= roomLimit.Value : false;
    /// <summary>
    /// 是否是空的
    /// </summary>
    public bool Empty => Room <= 0;

    private void Awake() {
        ID = this.GetComponentID();
        containerSortBase = this.GetComponent<ContainerSortBase>();
        if (containerSortBase == null)
            containerSortBase = this.gameObject.AddComponent<ContainerSortRecycle>();
        
        roomLimit = this.GetComponent<ContainerRoomLimit>();
    }

    private void Start() {
        var room = ID.GetInt();
        for (int i = 0; i < room; i++)
            this.AddRoom(this.transform.position);
    }

    /// <summary>
    /// 添加容量
    /// </summary>
    /// <param name="collectableObjectBase"></param>
    /// <returns></returns>
    public bool AddRoom(CollectableObjectBase collectableObjectBase) {
        // 容器容量如果存在 && 达到最大容量
        if (ReachMaxRoom)
            return false;

        collections.Push(collectableObjectBase);
        // 关闭物理
        collectableObjectBase.DisablePhysics();
        containerSortBase.Sort(collectableObjectBase);
        // 只有添加成功触发事件
        onRoomChange?.Invoke(Room);
        ID.Save(collections.Count);
        return true;
    }

    /// <summary>
    /// 添加容量
    /// 生成物体添加
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool AddRoom(Vector3 position) {
        return this.AddRoom(StackObjectManager.Instance.GetCollectableObject(stackObjectType, position, Quaternion.identity));
    }

    /// <summary>
    /// 取出容量
    /// </summary>
    /// <returns></returns>
    public CollectableObjectBase SubRoom() {
        if (Empty)
            return null;

        CollectableObjectBase result = default;
        if (containerSortBase is ContainerSortRecycle) {
            result = StackObjectManager.Instance.GetCollectableObject(stackObjectType, this.transform.position, Quaternion.identity);
            // 容器内还是存在被回收的物品
            collections.Pop();
        } else
            result = collections.Pop();
        containerSortBase.Free();
        // 只有成功取出触发事件
        onRoomChange?.Invoke(Room);

        ID.Save(collections.Count);
        return result;
    }

    /// <summary>
    /// 注册容器改变事件
    /// </summary>
    /// <param name="onRoomChange"></param>
    public void RegistOnRoomChangeAction(Action<int> onRoomChange) {
        onRoomChange.Invoke(Room);
        this.onRoomChange += onRoomChange;
    }

    /// <summary>
    /// 刷新(运行)容器改变事件
    /// </summary>
    public void FreshOnRoomChangeAction() {
        onRoomChange?.Invoke(Room);
    }

    /// <summary>
    /// 清空容器
    /// </summary>
    public void Clear() {
        while (collections.Count != 0)
            StackObjectManager.Instance.RecycleCollectableObject(collections.Pop());
        containerSortBase.Clear();
        ID.Save(0);
    }
}