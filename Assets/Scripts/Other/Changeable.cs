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
            if (this.value.Equals(value)) {
                return;
            }
            onValueChange?.Invoke(this.value, value);
            this.value = value;
        }
    }

    /// <summary>
    /// <para>值改变监听事件</para>
    /// <para>参数1：原来值</para>
    /// <para>参数2：目标值</para>
    /// </summary>
    private Action<T, T> onValueChange;

    /// <summary>
    /// 注册改变函数
    /// </summary>
    /// <param name="onValueChange"></param>
    public void Regist(Action<T, T> onValueChange) {
        onValueChange?.Invoke(value, value);
        this.onValueChange += onValueChange;
    }

    /// <summary>
    /// 移除改变函数
    /// </summary>
    /// <param name="onValueChange"></param>
    public void Remove(Action<T, T> onValueChange) => this.onValueChange -= onValueChange;

    /// <summary>
    /// 清空注册函数
    /// </summary>
    public void Clear() => this.onValueChange = null;
}
