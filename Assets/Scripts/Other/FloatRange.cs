 using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    /// <summary>
    /// 边界
    /// </summary>
    public float min, max;

    /// <summary>
    /// <para>滑动值</para>
    /// <para>PropertyDrawer</para>
    /// </summary>
    public float value;
    /// <summary>
    /// 滑动值
    /// </summary>
    /// <value></value>
    public float Value {
        get => value;
        set {
            this.value = Mathf.Clamp(value, min, max);
        }
    }

    /// <summary>
    /// 随机值
    /// </summary>
    /// <value></value>
    public float RandomValue
    {
        get
        {
            return Random.Range(min, max);
        }
    }
}