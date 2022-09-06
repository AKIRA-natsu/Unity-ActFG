using UnityEngine;

/// <summary>
/// 存放基类
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[SelectionBase]
public abstract class StoreBase : MonoBehaviour, IUpdate {
    // Layer
    public LayerMask mask;
    // 具体值
    private int[] layerValues;
    // 自身容器
    public ContainerObject selfContainer;
    
    // 上一次更新时间
    private float lastUpdateTime;
    // 拿取/放置间隔
    protected float interval;
    // 每次拿取/放置数量
    protected int preCount;

    // 目标容器
    protected ContainerObject tarContainer;

    private void Awake() {
        layerValues = mask.GetLayerValue();
    }

    private void OnTriggerEnter(Collider other) {
        other.gameObject.layer.Log();
        if (!CheckLayer(other.gameObject.layer))
            return;
        
        // 获得目标容器
        if (other.GetComponentInChildren<ContainerController>())
            tarContainer = other.GetComponentInChildren<ContainerController>().GetContainer(selfContainer.stackObjectType);
        else
            tarContainer = other.GetComponentInChildren<ContainerObject>();
        // 容器为空
        if (tarContainer == null)
            return;

        CalculateValue(out interval, out preCount);
        lastUpdateTime = Time.time;
        UpdateManager.Instance.Regist(this);
    }

    private void OnTriggerExit(Collider other) {
        if (!CheckLayer(other.gameObject.layer))
            return;

        UpdateManager.Instance.Remove(this);
        tarContainer = null;
    }

    /// <summary>
    /// 检查
    /// </summary>
    /// <param name="colliderLayer"></param>
    /// <returns></returns>
    private bool CheckLayer(int colliderLayer) {
        foreach (var value in layerValues) {
            if (colliderLayer == value)
                return true;
        }
        return false;
    }

    public void GameUpdate() {
        if (Time.time - lastUpdateTime <= interval)
            return;
        
        lastUpdateTime = Time.time;
        Store();
    }


    /// <summary>
    /// 更新获得/放置
    /// </summary>
    /// <param name="container"></param>
    protected abstract void Store();
    /// <summary>
    /// 计算拿取
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="preCount"></param>
    protected abstract void CalculateValue(out float interval, out int preCount);

}