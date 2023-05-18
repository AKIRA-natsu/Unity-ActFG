using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>清空信号</para>
/// <para>来源：https://www.bilibili.com/video/av21513489/?p=14&vd_source=b6cbace78cacac5f94717102502c11d0</para>
/// </summary>
public class ClearSingals : StateMachineBehaviour {
    // 开始时清理的动画
    public string[] clearAtEnterAnims;
    // 结束时清理的动画
    public string[] clearAtExitAnims;

    // 
    private int[] clearAtEnters;
    // 
    private int[] clearAtExits;

    private void Awake() {
        clearAtEnters = new int[clearAtEnterAnims.Length];
        clearAtExits = new int[clearAtExitAnims.Length];
        for (int i = 0; i < clearAtEnters.Length; i++)
            clearAtEnters[i] = Animator.StringToHash(clearAtEnterAnims[i]);
        for (int i = 0; i < clearAtExits.Length; i++)
            clearAtExits[i] = Animator.StringToHash(clearAtExitAnims[i]);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        foreach (var singal in clearAtEnters)
            animator.ResetTrigger(singal);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        foreach (var singal in clearAtExits)
            animator.ResetTrigger(singal);
    }
}
