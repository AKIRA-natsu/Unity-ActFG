using System.Collections;
using AKIRA.Manager;
using UnityEngine;

public class AITeam : MonoBehaviour, IResource, IUpdate {
    public int order => 0;
    // 生成数量
    public int count;
    // 影响位置
    // 本地坐标
    public Vector3 affectPosition { get; private set; } = Vector3.zero;
    // 范围
    public float radius;
    // 是否自动更新
    private bool @update = false;
    /// <summary>
    /// 注册/移除更新
    /// </summary>
    /// <value></value>
    public bool @Update {
        get => @update;
        set {
            // 相同
            if (value == @update)
                return;

            if (value)
                this.Regist();
            else
                this.Remove();
            @update = value;
        }
    }

    // 面板启动更新
    [SerializeField]
    private bool inspectorUpdate = false;

    private void Awake() {
        ResourceCollection.Instance.Regist(this, order);
    }

    public IEnumerator Load() {
        AIGroupManager.GetGroup(GroupTag.Fish).CreateMember(count, this);
        yield return null;
        @Update = inspectorUpdate;
    }

    private void OnDisable() {
        @Update = false;
    }

    // Update is called once per frame
    public void GameUpdate() {
        if (Random.Range(0, 10000) < 50)
            affectPosition = Random.insideUnitSphere * radius;
	}

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(affectPosition + this.transform.position, 1f);
    }
}