using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIFSM {
    // 状态
    private AIState state = AIState.Idle;
    // 当前状态
    public AIState CurState { get => state; }

    // 状态事件
    private Dictionary<AIState, Action> stateActionMap = new Dictionary<AIState, Action>();
    // 状态事件 一次性
    private Dictionary<AIState, Action> onceActionMap = new Dictionary<AIState, Action>();

    // 打日志
    public bool @debug = false;

    public AIFSM() {
        // 初始化事件字典
        foreach (AIState state in Enum.GetValues(typeof(AIState))) {
            stateActionMap.Add(state, null);
            onceActionMap.Add(state, null);
        }
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="targetState"></param>
    public void SwitchState(AIState targetState) {
        if (state == targetState)
            return;
        
        if (@debug)
            $"{state}切换到{targetState}".Colorful(Color.magenta).Log();

        state = targetState;
        stateActionMap[state]?.Invoke();
        onceActionMap[state]?.Invoke();
        onceActionMap[state] = null;
    }

    /// <summary>
    /// 注册状态事件
    /// </summary>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <param name="once"></param>
    public void RegistStateAction(AIState state, Action action, bool once = false) {
        if (once)
            onceActionMap[state] += action;
        else
            stateActionMap[state] += action;
    }
}