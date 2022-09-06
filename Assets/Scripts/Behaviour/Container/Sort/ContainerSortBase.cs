using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContainerSortBase : MonoBehaviour {
    // 排序位置
    protected Stack<Vector3> positions = new Stack<Vector3>();

    /// <summary>
    /// <para>排序</para>
    /// <para>塞入保存位置</para>
    /// </summary>
    /// <param name="sortObj"></param>
    public abstract void Sort(CollectableObjectBase sortObj);

    /// <summary>
    /// <para>释放位置</para>
    /// <para>基本是一样的释放</para>
    /// </summary>
    public virtual void Free() {
        if (positions.Count <= 0)
            return;

        positions.Pop();
    }

    /// <summary>
    /// 清空所有位置
    /// </summary>
    public virtual void Clear() {
        positions.Clear();
    }
}
