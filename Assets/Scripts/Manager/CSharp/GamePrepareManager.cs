using System;
using AKIRA.Manager;
using Cysharp.Threading.Tasks;

/// <summary>
/// 游戏准备中心
/// </summary>
public class GamePrepareManager : Singleton<GamePrepareManager> {
    /// <summary>
    /// 进入游戏事件
    /// </summary>
    private Func<UniTask> onGameEnter;
    /// <summary>
    /// 退出游戏事件
    /// </summary>
    private Func<UniTask> onGameExit;

    private GamePrepareManager() {}

    /// <summary>
    /// 注册进入游戏事件
    /// </summary>
    /// <param name="onGameEnter"></param>
    public void RegistOnGameEnter(Func<UniTask> onGameEnter) {
        this.onGameEnter += onGameEnter;
    }
    /// <summary>
    /// 移除进入游戏事件
    /// </summary>
    /// <param name="onGameEnter"></param>
    public void RemoveOnGameEnter(Func<UniTask> onGameEnter) {
        this.onGameEnter -= onGameExit;
    }
    /// <summary>
    /// 注册退出游戏事件
    /// </summary>
    /// <param name="onGameExit"></param>
    public void RegistOnGameExit(Func<UniTask> onGameExit) {
        this.onGameExit += onGameExit;
    }
    /// <summary>
    /// 移除退出游戏事件
    /// </summary>
    /// <param name="onGameExit"></param>
    public void RemoveOnGameExit(Func<UniTask> onGameExit) {
        this.onGameExit -= onGameExit;
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public async void EnterGame() {
        if (onGameEnter == null)
            return;
        await onGameEnter.Invoke();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public async void ExitGame() {
        if (onGameExit == null)
            return;
        await onGameExit.Invoke();
    }
}