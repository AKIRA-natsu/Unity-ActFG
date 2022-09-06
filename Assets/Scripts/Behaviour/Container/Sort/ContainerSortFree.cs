using DG.Tweening;
using UnityEngine;

/// <summary>
/// 放入后重新开启物理，随意放置
/// </summary>
public class ContainerSortFree : ContainerSortBase {
    public override void Sort(CollectableObjectBase sortObj) {
        sortObj.SetParent(this);
        sortObj.transform.DOLocalJump(Vector3.zero, 1, 1, 0.3f).OnComplete(() => {
            sortObj.PlayPunchScaleTween();
            sortObj.EnablePhysics();
            sortObj.selfCollider.enabled = false;
        });
    }

    public override void Free() {}
}