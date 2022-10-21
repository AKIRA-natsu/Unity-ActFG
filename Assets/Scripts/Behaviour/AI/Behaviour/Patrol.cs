using UnityEngine;

[RequireComponent(typeof(AIBase))]
public class Patrol : MonoBehaviour, IUpdate {
    // 巡逻点父节点
    public Transform partrolTransParent;
    // 等待时间
    public float waitTime = 1f;
    // 随机巡逻点
    public bool inOrder = true;
    // 是否巡逻
    private bool patroling = true;
    /// <summary>
    /// 更新巡逻
    /// </summary>
    /// <value></value>
    public bool Patroling {
        get => patroling;
        set {
            // 重复不改变
            if (patroling == value)
                return;

            patroling = value;
            this.enabled = value;
        }
    }

    [SerializeField]
    private bool patrolingInspector = true;

    // AI
    private AIBase ai;
    // 巡逻点
    private Vector3[] positions;
    // 当前巡逻目标点键值
    private int curIndex = -1;

    // 等待时间
    private float wait = 0f;

    private void Awake() {
        ai = this.GetComponent<AIBase>();
        var trans = partrolTransParent.GetChildrenArray<Transform>();
        positions = new Vector3[trans.Length];
        for (int i = 0; i < trans.Length; i++)
            positions[i] = trans[i].position;
        
        patrolingInspector = Patroling;
    }

    private void OnEnable() {
        UpdateManager.Instance.Regist(this);
    }

    private void OnDisable() {
        if (!UpdateManager.IsApplicationOut)
            UpdateManager.Instance.Remove(this);
    }

    private void OnValidate() {
        if (Application.isPlaying)
            Patroling = patrolingInspector;
    }

    public void GameUpdate() {
        if (!ai.Reach)
            return;
        
        wait += Time.deltaTime;
        if (wait <= waitTime)
            return;

        wait = 0;
        ai.SetDestination(GetNextPatrolPosition());
    }

    /// <summary>
    /// 获得下一个巡逻点
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNextPatrolPosition() {
        if (inOrder) {
            if (++curIndex >= positions.Length)
                curIndex = 0;
        } else {
            var index = Random.Range(0, positions.Length);
            if (index == curIndex)
                return GetNextPatrolPosition();
            curIndex = index;
        }

        return positions[curIndex];
    }
}