using UnityEngine;

[CreateAssetMenu(fileName = "AiDefaultConfig", menuName = "Framework/Ai/AiDefaultConfig", order = 0)]
public class AiDefaultConfig : ScriptableObject {
    /// <summary>
    /// 默认路径
    /// </summary>
    public const string DefaultPath = "Config/AiDefaultConfig";

    /// <summary>
    /// 走路速度
    /// </summary>
    public float walkSpeed = 1.2f;
    /// <summary>
    /// 奔跑速度
    /// </summary>
    public float runSpeed = 3f;

    /// <summary>
    /// 巡逻等待时间
    /// </summary>
    public float patrolWaitTime = 2f;
    /// <summary>
    /// 巡逻顺序/随机
    /// </summary>
    public bool patrolInOrder = true;

    /// <summary>
    /// 跟随/跟随更新时间
    /// </summary>
    public float chaseWaitTime = 0.5f;
    /// <summary>
    /// 跟随/追逐距离
    /// </summary>
    public float chaseDistance = 5f;

    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attckDistance = 2f;
}