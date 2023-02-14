using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自身动画基类
/// </summary>
public abstract class SelfAnim : MonoBehaviour, IUpdate {
    /// <summary>
    /// 运动正负朝向
    /// </summary>
    public enum Ward {
        Forward,
        Backward,
    }

    [HideInInspector]
    public bool Enable = true;

    public abstract void GameUpdate();
}

/// <summary>
/// 动画
/// 学习Deform面板
/// </summary>
public class SelfAnimationGroup : MonoBehaviour, IUpdate {
    // 自身动画组
    private const string AnimationGroup = "SelfAnimation";

    [SerializeField, HideInInspector]
    private List<SelfAnim> datas = new();
    /// <summary>
    /// 动画组
    /// </summary>
    /// <value></value>
    public List<SelfAnim> Datas {
        get => datas;
        set => datas = value;
    }

    private void OnEnable() {
        this.Regist(AnimationGroup);
    }

    private void OnDisable() {
        this.Remove(AnimationGroup);
    }

    public void GameUpdate() {
        foreach (var data in datas) {
            if (data.Enable)
                data.GameUpdate();
        }
    }

}