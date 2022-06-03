using System;

public static class RandomExtend {
    /// <summary>
    /// 获得随机Enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RandomEnum<T>() where T : Enum {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    /// <summary>
    /// <para>获得随机Enum</para>
    /// <para>如果尾端比开端小，直接变成列表长度</para>
    /// </summary>
    /// <param name="begin">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RandomEnum<T>(int begin = 0, int end = 0) where T : Enum {
        var values = Enum.GetValues(typeof(T));
        // 如果尾端比开端小，直接变成列表长度
        if (end < begin)
            end = values.Length;
        return (T)values.GetValue(UnityEngine.Random.Range(begin, end));
    }
}