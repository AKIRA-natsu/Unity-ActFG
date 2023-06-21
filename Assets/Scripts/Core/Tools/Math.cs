using UnityEngine;

/// <summary>
/// 数学方法
/// </summary>
public static class MathTool {
    /// <summary>
    /// <para>Lerp</para>
    /// <para>来源：https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/</para>
    /// <para>TODO: 测试</para>
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="lambda"></param>
    public static float Damp(float value1, float value2, float lambda) {
        return Mathf.Lerp(value1, value2, 1 - Mathf.Exp(-lambda * Time.deltaTime));
    }

    /// <summary>
    /// <para>Lerp</para>
    /// <para>来源：https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/</para>
    /// <para>TODO: 测试</para>
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="lambda"></param>
    /// <param name="time"></param>
    public static Vector3 Damp(Vector3 value1, Vector3 value2, float lambda) {
        return Vector3.Lerp(value1, value2, 1 - Mathf.Exp(-lambda * Time.deltaTime));
    }

}