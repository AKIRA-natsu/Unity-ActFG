using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 可收集物品基类
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
[SelectionBase]
public abstract class CollectableObjectBase : MonoBehaviour, IPool {
    /// <summary>
    /// 堆叠物类型
    /// </summary>
    public StackObjectType stackObjectType;

    // 渲染体
    public Transform render;
    // 渲染体碰撞体
    public Collider renderCollider;
    // 自身刚体
    public Rigidbody selfRigid;
    // 自身碰撞体
    public Collider selfCollider;

    // 缩放动画
    private Tween scaleTween;

    public virtual void Wake() {
        EnablePhysics();
    }

    public virtual void Recycle() {
        DisablePhysics();
        // 排序HideSelf清空下回收重新显示render
        if (!render.gameObject.activeSelf)
            render.gameObject.SetActive(true);
    }

    /// <summary>
    /// 开启物理
    /// </summary>
    public virtual void EnablePhysics() {
        renderCollider.enabled = true;
        selfCollider.enabled = true;
        selfRigid.useGravity = true;
        selfRigid.isKinematic = false;
    }

    /// <summary>
    /// 关闭物理
    /// </summary>
    public virtual void DisablePhysics() {
        renderCollider.enabled = false;
        selfCollider.enabled = false;
        selfRigid.useGravity = false;
        selfRigid.isKinematic = true;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.GetComponentInChildren<ContainerController>()) {
            other.GetComponentInChildren<ContainerController>().GetContainer(stackObjectType).AddRoom(this);
        } else if (other.GetComponentInChildren<ContainerObject>()) {
            other.GetComponentInChildren<ContainerObject>().AddRoom(this);
        }
    }

    /// <summary>
    /// 播放缩放动效
    /// </summary>
    public void PlayPunchScaleTween() {
        scaleTween?.Kill(true);
        render.localScale = Vector3.one;
        render.DOPunchScale(Vector3.one * 0.1f, 0.3f);
    }

}