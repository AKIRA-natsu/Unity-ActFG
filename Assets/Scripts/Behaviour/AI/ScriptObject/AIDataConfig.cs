using UnityEngine;

/// <summary>
/// AI数据
/// </summary>
[CreateAssetMenu(fileName = "AIDataConfig", menuName = "Framework/AIDataConfig", order = 0)]
public class AIDataConfig : ScriptableObject {
    #region Get Default Config
    /// <summary>
    /// 默认路径
    /// </summary>
    public const string DefaultPath = "Config/AIDataConfig";

    /// <summary>
    /// 获得默认配置
    /// </summary>
    /// <returns></returns>
    public static AIDataConfig GetDefaultConfig() {
        return DefaultPath.Load<AIDataConfig>();
    }
    #endregion

    /// <summary>
    /// 移动速度
    /// </summary>
    public float walkSpeed;

    /// <summary>
    /// 跑步速度
    /// </summary>
    public float runSpeed;

    
    /// <summary>
    /// 等待时间
    /// </summary>
    public float waitTime;

    /// <summary>
    /// 到达距离
    /// </summary>
    public float reachDestination;

    /// <summary>
    /// 是否随机巡逻
    /// </summary>
    public bool partolRandom;
    /// <summary>
    /// 随机巡逻半径
    /// </summary>
    public float partolRadius;

}