using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画
/// 学习Deform面板
/// </summary>
public class SelfAnimation : MonoBehaviour {
    [SerializeField, HideInInspector]
    private List<SelfAnim> datas = new();
    public List<SelfAnim> Datas {
        get => datas;
        set => datas = value;
    }
}