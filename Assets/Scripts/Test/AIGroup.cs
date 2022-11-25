using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AKIRA.Manager;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AIGroup : MonoBehaviour {
    [SerializeField]
    private GroupTag groupTag;
    [SerializeField]
    private AIBase member;
    // 队伍字典
    private Dictionary<int, List<AIBase>> AITeamMap = new Dictionary<int, List<AIBase>>();

    private void Awake() {
        AIGroupManager.Instance.RegistGroup(groupTag, this);
    }

    /// <summary>
    /// 获得同一个队伍
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public ReadOnlyCollection<AIBase> GetTeam(int ID) {
        return AITeamMap[ID].AsReadOnly();
    }

    /// <summary>
    /// 生成一个队伍
    /// </summary>
    /// <param name="count"></param>
    /// <param name="parent"></param>
    public void CreateMember(int count, AITeam team) {
        int ID = AITeamMap.Count;
        AITeamMap.Add(ID, new List<AIBase>());
        this.Repeat(_ => {
            AITeamMap[ID].Add(ObjectPool.Instance.Instantiate(member).Init(ID, groupTag, team));
        }, count);
    }
}