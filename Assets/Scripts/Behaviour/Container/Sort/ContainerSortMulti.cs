using DG.Tweening;
using UnityEngine;

/// <summary>
/// 多个排序
/// </summary>
public class ContainerSortMulti : ContainerSortBase {
    // 间隔高度
    public float height;
    // 排放位置
    private Transform[] puts;

    private void Awake() {
        puts = this.transform.GetChildrenComponents<Transform>();
    }

    public override void Sort(CollectableObjectBase sortObj) {
        sortObj.SetParent(puts[positions.Count % puts.Length]);
        var position = Vector3.up * height * (positions.Count / puts.Length);
        positions.Push(position);
        sortObj.transform.DOLocalJump(position, height, 1, 0.3f)
            .OnComplete(sortObj.PlayPunchScaleTween);
        sortObj.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
    }
}