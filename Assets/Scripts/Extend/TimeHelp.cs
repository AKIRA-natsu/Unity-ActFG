using System;
using System.Collections;
using System.Collections.Generic;
using AKIRA.Coroutine;

public static class TimeHelp {
    /// <summary>
    /// 重复事件委托
    /// </summary>
    /// <returns></returns>
    public delegate bool @Continue();

    #region 相关属性字段存储
    // id 字典 bool
    private static Dictionary<int, bool> IDMap = new Dictionary<int, bool>();
    // 结束事件字典
    private static List<int> IDEndList = new List<int>();
    // 暂存列表
    private static List<int> Temp = new List<int>();

    // 单词执行id列表
    private static Dictionary<Action, bool> OnceIDMap = new Dictionary<Action, bool>();
    #endregion

    #region 延迟执行
    /// <summary>
    /// <para>延迟执行，协程</para>
    /// <para>需要在Update中添加<code>CoroutineManager.Instance.UpdateCoroutine();</code></para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="action"></param>
    /// <param name="time">等待时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>ID</returns>
    public static int Delay<T>(this T com, Action action, float time = 0f) {
        var id = DistributeID();
        CoroutineManager.Instance.Start(CDelay(id, action, time));
        return id;
    }

    /// <summary>
    /// 延迟执行，协程
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private static IEnumerator CDelay(int id, Action action, float time) {
        yield return new WaitForSeconds(time);
        action?.Invoke();
        IDMap[id] = false;
    }
    #endregion

    #region 重复执行
    /// <summary>
    /// <para>循环执行，协程</para>
    /// <para>需要在Update中添加<code>CoroutineManager.Instance.UpdateCoroutine();</code></para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="action"></param>
    /// <param name="time">循环次数</param>
    /// <param name="wait">每次循环等待时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>ID</returns>
    public static int Repeat<T>(this T com, Action<int> action, int time = 1, float wait = 0) {
        var id = DistributeID();
        CoroutineManager.Instance.Start(CRepeat(id, action, time, wait));
        return id;
    }

    /// <summary>
    /// 循环执行，协程
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <param name="time"></param>
    /// <param name="wait"></param>
    /// <returns></returns>
    private static IEnumerator CRepeat(int id, Action<int> action, int time, float wait) {
        var delay = new WaitForSeconds(wait);
        while (time > 0) {
            action?.Invoke(time);
            time--;
            yield return delay;
        }
        IDMap[id] = false;
    }

    /// <summary>
    /// <para>循环执行，协程，委托</para>
    /// <para>需要在Update中添加<code>CoroutineManager.Instance.UpdateCoroutine();</code></para>
    /// <para>委托返回bool</para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="action"></param>
    /// <param name="continue">循环条件</param>
    /// <param name="wait">每次循环等待时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>ID</returns>
    public static int Repeat<T>(this T com, Action action, @Continue @continue, float wait = 0f) {
        var id = DistributeID();
        CoroutineManager.Instance.Start(CRepeat(id, action, @continue, wait));
        return id;
    }

    /// <summary>
    /// 循环执行，协程
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <param name="continue"></param>
    /// <param name="wait"></param>
    /// <returns></returns>
    private static IEnumerator CRepeat(int id, Action action, @Continue @continue, float wait) {
        var delay = new WaitForSeconds(wait);
        while (@continue.Invoke()) {
            action?.Invoke();
            yield return delay;
        }
        IDMap[id] = false;
    }

    /// <summary>
    /// <para>循环执行，协程，boolean</para>
    /// <para>需要在Update中添加<code>CoroutineManager.Instance.UpdateCoroutine();</code></para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="action"></param>
    /// <param name="continue">循环条件</param>
    /// <param name="wait">每次循环等待时间</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>ID</returns>
    public static int Repeat<T>(this T com, Action action, bool @continue, float wait = 0f) {
        var id = DistributeID();
        CoroutineManager.Instance.Start(CRepeat(id, action, () => {
            return @continue;
        }, wait));
        return id;
    }

    #endregion

    #region 结束执行
    /// <summary>
    /// <para>结束执行事件</para>
    /// <para>需要在Update中添加<code>CoroutineManager.Instance.UpdateCoroutine();</code></para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="EndAction"></param>
    /// <param name="wait">结束等待时间</param>
    public static void End(this int id, Action EndAction, float wait = 0) {
        IDEndList.Add(id);
        CoroutineManager.Instance.Start(CEnd(id, EndAction, wait));
    }

    /// <summary>
    /// 结束执行事件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="EndAction"></param>
    /// <returns></returns>
    private static IEnumerator CEnd(int id, Action EndAction, float wait) {
        var delay = new WaitForSeconds(wait);
        while (IDMap[id])
            // 不干任何事情
            yield return delay;
        EndAction?.Invoke();
        IDEndList.Remove(id);
    }
    #endregion
    
    #region 等待执行

    #endregion

    #region 单次执行
    /// <summary>
    /// <para>单词执行</para>
    /// <para>x个物体同一脚本下lambda表达式只执行一次，事件会执行x次</para>
    /// </summary>
    /// <param name="com"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Once<T>(this T com, Action action) {
        if (!OnceIDMap.ContainsKey(action)) {
            OnceIDMap.Add(action, false);
            action?.Invoke();
        }
        return com;
    }
    #endregion

    #region id
    /// <summary>
    /// 分配ID
    /// </summary>
    /// <returns></returns>
    private static int DistributeID() {
        // 分配前清除ID
        ClearID();
        // 分配从0开始
        return CheckID(0);
    }

    /// <summary>
    /// 递归、检查 ID 可行性
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static int CheckID(int id) {
        if (IDMap.ContainsKey(id))
            return CheckID(++id);
        // 添加ID，分配id后id立刻被占用状态
        IDMap.Add(id, true);
        return id;
    }

    /// <summary>
    /// 清除不被使用的 ID
    /// </summary>
    private static void ClearID() {
        // foreach只读，另外新建列表存储删除值
        foreach (var kvp in IDMap) {
            // 在被使用中
            if (kvp.Value) continue;
            // 检查是否在结束列表中存在
            if (IDEndList.Contains(kvp.Key)) continue;
            // 可以删除id，填入暂存表
            Temp.Add(kvp.Key);
        }
        // id字典中删除
        foreach (var id in Temp)
            IDMap.Remove(id);
        // 清空暂存表
        Temp.Clear();
    }
    #endregion

}