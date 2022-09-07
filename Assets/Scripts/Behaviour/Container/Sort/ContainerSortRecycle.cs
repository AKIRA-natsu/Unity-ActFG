using DG.Tweening;
using UnityEngine;

/// <summary>
/// 排序回收
/// </summary>
public class ContainerSortRecycle : ContainerSortBase {
    public override void Sort(CollectableObjectBase sortObj) {
        // 用位置表示数量
        positions.Push(Vector3.zero);
        sortObj.SetParent(this);
        sortObj.transform.DOJump(this.transform.position, sortObj.transform.position.y, 1, 0.3f).OnComplete(() =>
            StackObjectManager.Instance.RecycleCollectableObject(sortObj));
    }
}