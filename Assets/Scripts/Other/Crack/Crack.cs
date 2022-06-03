using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 来源：https://www.bilibili.com/video/BV1rZ4y1k7Z3/
/// </summary>
public class Crack : MonoBehaviour, IPool {
    // 长度 这里是1
    public int length;
    // 
    public int blendShapeCount;

    // 裂纹
    public SkinnedMeshRenderer _crack;
    // 遮罩
    public SkinnedMeshRenderer _crackMask;

    // 转角
    public List<Transform> corners = new List<Transform>();

    public float GetBlendShape(int index) {
        return 100 - _crack.GetBlendShapeWeight(index);
    }

    public void SetBlendShape(int index, float value) {
        _crack.SetBlendShapeWeight(index, 100 - value);
        _crackMask.SetBlendShapeWeight(index, 100 - value);
    }

    public void Wake() {}
    public void Recycle() {}
}
