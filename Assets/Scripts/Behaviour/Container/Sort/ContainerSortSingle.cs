using DG.Tweening;
using UnityEngine;

/// <summary>
/// 单个排序
/// </summary>
public class ContainerSortSingle : ContainerSortBase {
    // 间隔高度
    public float height;

    public override void Sort(CollectableObjectBase sortObj) {
        sortObj.SetParent(this);
        var position = Vector3.up * positions.Count * height;
        positions.Push(position);
        sortObj.transform.DOLocalJump(position, sortObj.transform.position.y, 1, 0.3f)
            .OnComplete(sortObj.PlayPunchScaleTween);
        sortObj.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
    }
}