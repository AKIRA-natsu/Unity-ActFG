using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

/// <summary>
/// 自动生成
/// </summary>
[SelectionBase]
public class ContainerAutoGenerate : MonoBehaviour, IUpdate {
    // 放置容器物品
    public ContainerObject container;
    
    // 更新间隔时间
    public float interval;
    // 上一次更新时间
    private float lastUpdateTime;

    private bool @update = true;
    /// <summary>
    /// 是否更新
    /// </summary>
    /// <value></value>
    public bool @Update {
        get => @update;
        set {
            if (@update == value)
                return;
            
            @update = value;
            this.enabled = value;
        }
    }

    // 提供面板手动决定更新
    [SerializeField]
    private bool updateInspector = true;

    private void OnEnable() {
        UpdateManager.Instance.Regist(this);
    }

    private void OnDisable() {
        if (!UpdateManager.isApplicationOut)
            UpdateManager.Instance.Remove(this);
    }

    private void OnValidate() {
        if (!Application.isPlaying)
            return;

        @Update = updateInspector;
    }

    public void GameUpdate() {
        if (Time.time - lastUpdateTime <= interval)
            return;
        
        lastUpdateTime = Time.time;

        if (container.ReachMaxRoom)
            return;
        container.AddRoom(this.transform.position);
    }
}
