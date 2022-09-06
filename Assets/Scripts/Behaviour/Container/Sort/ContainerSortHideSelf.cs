using DG.Tweening;
using UnityEngine;

/// <summary>
/// 排序隐藏
/// </summary>
public class ContainerSortHideSelf : ContainerSortBase {
    public override void Sort(CollectableObjectBase sortObj) {
        sortObj.SetParent(this);
        sortObj.transform.DOJump(this.transform.position, sortObj.transform.position.y, 1, 0.3f).OnComplete(() =>
            sortObj.render.gameObject.SetActive(false));
    }

    public override void Free() {}
}