using UnityEngine;

public abstract class AIBase : MonoBehaviour, IPool, IUpdate {
    // 队伍标识
    public int ID { get; protected set; }
    // 队伍类型标签
    public GroupTag groupTag { get; protected set; }
    // 所在队伍
    public AITeam team { get; protected set; }

    /// <summary>
    /// 初始化ID，标签
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="tag"></param>
    public AIBase Init(int ID, GroupTag tag, AITeam team) {
        this.ID = ID;
        this.groupTag = tag;
        this.team = team;
        this.SetParent(team);
        this.transform.localPosition = team.affectPosition + Random.insideUnitSphere * team.radius;
        return this;
    }

    public void Wake() {}
    public void Recycle() {}
    public abstract void GameUpdate();
}