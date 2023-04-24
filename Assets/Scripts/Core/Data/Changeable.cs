using System;

/// <summary>
/// 可变值
/// </summary>
/// <typeparam name="T"></typeparam>
public class Changeable<T> {
    private T value = default;
    public T Value {
        get => value;
        set  {
            if (this.value.Equals(value))
                return;
            onValueChange?.Invoke(value);
            this.value = value;
        }
    }

    /// <summary>
    /// <para>值改变监听事件</para>
    /// <para>参数：目标值</para>
    /// </summary>
    private Action<T> onValueChange;

    public Changeable() {}
    public Changeable(T value) => this.value = value;

    /// <summary>
    /// 注册改变函数
    /// </summary>
    /// <param name="onValueChange"></param>
    public void Regist(Action<T> onValueChange) {
        onValueChange?.Invoke(value);
        this.onValueChange += onValueChange;
    }

    /// <summary>
    /// 移除改变函数
    /// </summary>
    /// <param name="onValueChange"></param>
    public void Remove(Action<T> onValueChange) => this.onValueChange -= onValueChange;

    /// <summary>
    /// 清空注册函数
    /// </summary>
    public void Clear() => this.onValueChange = null;
}
