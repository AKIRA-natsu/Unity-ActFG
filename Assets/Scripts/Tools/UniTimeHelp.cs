using System;
using Cysharp.Threading.Tasks;

/// <summary>
/// UniTask 方法
/// </summary>
public static class UniTimeHelp {
    #region 延迟
    /// <summary>
    /// UniTask 延迟执行
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action">延迟时间</param>
    /// <param name="time">等待时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async static UniTask UniDelay<T>(this T value, Action action, float delay = 0f) {
        int time = Convert.ToInt32(delay * 1000f);
        await UniTask.Delay(time);
        action?.Invoke();
    }
    #endregion

    #region 重复
    /// <summary>
    /// UniTask 重复执行
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <param name="time">重复次数</param>
    /// <param name="delay">重复间隔时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async static UniTask UniRepeat<T>(this T value, Action<int> action, int time = 1, float delay = 0f) {
        var waitTime = Convert.ToInt32(delay * 1000f);
        while (time > 0) {
            action?.Invoke(--time);
            await UniTask.Delay(waitTime);
        }
    }

    /// <summary>
    /// UniTask 重复执行
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <param name="continue">重复条件</param>
    /// <param name="delay">重复间隔时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async static UniTask UniRepeat<T>(this T value, Action action, Func<bool> @continue, float delay = 0f) {
        var waitTime = Convert.ToInt32(delay * 1000f);
        while (@continue.Invoke()) {
            action?.Invoke();
            await UniTask.Delay(waitTime);
        }
    }
    #endregion

    #region 完成
    /// <summary>
    /// UniTask 完成执行
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    /// <param name="delay">延迟时间</param>
    /// <returns></returns>
    public async static UniTask UniCompleted(this UniTask task, Action action, float delay = 0f) {
        var completedTask = new UniTaskCompletionSource();
        int time = Convert.ToInt32(delay * 1000f);
        await UniTask.Delay(time);
        action?.Invoke();
    }

    /// <summary>
    /// <para>UniTask 完成执行</para>
    /// <para>完成全部Task</para>
    /// <para>FIXME: 需要测试</para>
    /// </summary>
    /// <param name="action"></param>
    /// <param name="tasks"></param>
    /// <returns></returns>
    public async static UniTask UniAllCompleted(Action action, params UniTask[] tasks) {
        await UniTask.WhenAll(tasks);
        action?.Invoke();
    }

    /// <summary>
    /// <para>UniTask 完成执行</para>
    /// <para>完成其中一个Task</para>
    /// <para>FIXME: 需要测试</para>
    /// </summary>
    /// <param name="action"></param>
    /// <param name="tasks"></param>
    /// <returns></returns>
    public async static UniTask UniAnyCompleted(Action action, params UniTask[] tasks) {
        await UniTask.WhenAny(tasks);
        action?.Invoke();
    }
    #endregion
}