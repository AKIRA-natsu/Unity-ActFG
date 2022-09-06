using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <para>Agent OffMeshLink 移动处理</para>
/// <para>https://bbs.huaweicloud.com/blogs/303788</para>
/// </summary>
public abstract class AgentLocomotion : AIBehaviour {
    // OffMeshLink 开始点
    private Vector3 linkStart;
    // OffMeshLink 结束点
    private Vector3 linkEnd;
    // OffMeshLink 旋转
    private Quaternion linkRotate;
    // 开始寻路
    private bool begin = false;

    // 协程函数名称
    private string locoState = "Locomotion_Stand";

    [HideInInspector]
    public float walkSpeed = 1.5f;
    [HideInInspector]
    public float runSpeed = 4f;
    [HideInInspector]
    public float speedThreshold = 0.1f;

    protected override void Awake() {
        base.Awake();
        agent.autoTraverseOffMeshLink = false;
    }

    public override void Wake() {
        // 协程处理动画状态机
        StartCoroutine(AniamationStateMachine());
    }

    public override void Recycle() {
        // 非死亡的回收
        if (fsm.CurState != AIState.Dead) {
            StopCoroutine(locoState);
            StopCoroutine(AniamationStateMachine());
        }
        fsm.SwitchState(AIState.Idle);
        locoState = "Locomotion_Stand";
    }

    #region IEnumerator 协程 处理动画
    /// <summary>
    /// 协程、while循环处理动画状态机
    /// </summary>
    /// <returns></returns>
    private IEnumerator AniamationStateMachine() {
        while (fsm.CurState != AIState.Dead) {
            if (fsm.@debug)
                $"{this}: 当前执行{locoState}函数".Log();
            yield return StartCoroutine(locoState);
        }
    }

    /// <summary>
    /// 站立待机
    /// </summary>
    /// <returns></returns>
    private IEnumerator Locomotion_Stand() {
        do {
            UpdateAnimationBlend();
            yield return null;
        } while (agent.remainingDistance == 0);
        // 未达到目标点，转到移动
        locoState = "Locomotion_Move";
        yield return null;
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <returns></returns>
    private IEnumerator Locomotion_Move() {
        do {
            UpdateAnimationBlend();
            yield return null;
            // 角色处于OffMeshLink，更具不同地点选择不同状态
            if (agent.isOnOffMeshLink) {
                Walk(0);
                locoState = SelectLinkAnimation();
                yield break;
            }
        } while (agent.remainingDistance != 0);
        // 到达目标点，转到待机
        locoState = "Locomotion_Stand";
        yield return null;
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    /// <returns></returns>
    private IEnumerator Locomotion_Jump() {
        fsm.SwitchState(AIState.Jump);
        Vector3 startPos = transform.position;

        agent.isStopped = true;
        Jump();
        transform.rotation = linkRotate;

        agent.ActivateCurrentOffMeshLink(false);

        // FIXME: 锁死按照跳跃动画0.45f
        float time = 0.45f;
        float lerpTime = 0f;
        do {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Jump")) {
                lerpTime += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(startPos, linkEnd, lerpTime / time);
                newPos.y += 0.4f * Mathf.Sin(Mathf.PI * (lerpTime / time));
                transform.position = newPos;
            }
            yield return null;
        } while (lerpTime <= time);

        agent.ActivateCurrentOffMeshLink(true);
        transform.position = linkEnd;
        // 恢复到Idle
        agent.CompleteOffMeshLink();
        // FIXME: 跳跃动画问题缓1s
        yield return new WaitForSeconds(1.5f);
        agent.isStopped = false;
        locoState = "Locomotion_Stand";
        yield return null;
    }

    /// <summary>
    /// 攀爬
    /// </summary>
    /// <returns></returns>
    private IEnumerator Locomotion_Climb() {
        fsm.SwitchState(AIState.Climb);
        // 攀爬中心位置
        Vector3 linkCenter = (linkStart + linkEnd) * 0.5f;
        // float animSpeed;
        // 判断在上面还是下面
        if (transform.position.y > linkCenter.y) {
        //     // 在上面向下爬
        //     animSpeed = -1f;
            // FIXME: 没有向下，用跳跃代替
            locoState = "Locomotion_Jump";
            yield break;
        } else {
        //     // 在下面向上爬
        //     animSpeed = 1f;
        }

        agent.isStopped = true;

        // 设置位置
        transform.position = linkStart;
        transform.rotation = linkRotate;
        // 播放动画
        Climb(true);
        agent.ActivateCurrentOffMeshLink(false);

        // 攀爬
        bool climbing = true;
        var delay = new WaitForSeconds(0.01f);
        Vector3 climbToTopPos = default;

        do {
            // FIXME: 转向莫名转到不对的地方 问题
            transform.rotation = linkRotate;
            if (Mathf.Abs(linkEnd.y - transform.position.y) <= agent.height) {
                Climb(false);
                ClimbToTop(true);
                var info = animator.GetCurrentAnimatorStateInfo(0);
                if (info.IsName("Climbing To Top")) {
                    transform.position = Vector3.Lerp(climbToTopPos, linkEnd, info.normalizedTime);
                    if (info.normalizedTime >= 1) {
                        climbing = false;
                    }
                } else {
                    yield return delay;
                }
            } else {
                transform.position = Vector3.Lerp(transform.position, linkEnd, Time.deltaTime);
                climbToTopPos = transform.position;
            }
            yield return delay;
        } while (climbing);

        ClimbToTop(false);
        agent.ActivateCurrentOffMeshLink(true);
        // 恢复Idle
        transform.position = linkEnd;
        agent.CompleteOffMeshLink();
        agent.isStopped = false;

        // 设置下一个状态
        locoState = "Locomotion_Stand";
        yield return null;

    }
    #endregion

    /// <summary>
    /// 选择连接动画
    /// </summary>
    private string SelectLinkAnimation() {
        // 获得当前OffMeshLink数据
        OffMeshLinkData link = agent.currentOffMeshLinkData;
        // 计算角色当前实在link的开始点还是结束点 OffMeshLink双向
        float distS = (transform.position - link.startPos).magnitude;
        float distE = (transform.position - link.endPos).magnitude;

        if (distS < distE) {
            linkStart = link.startPos;
            linkEnd = link.endPos;
        } else {
            linkStart = link.endPos;
            linkEnd = link.startPos;
        }

        // 方向
        Vector3 alignDir = linkEnd - linkStart;
        // 忽略y轴
        alignDir.y = 0;
        // 计算旋转角度
        linkRotate = Quaternion.LookRotation(alignDir);

        // 判断OffMeshLink手动生成（楼梯）还是自动生成（跳跃）
        if (link.linkType == OffMeshLinkType.LinkTypeManual) {
            // 爬楼梯
            return "Locomotion_Climb";
        } else {
            // 跳跃
            return "Locomotion_Jump";
        }
    }

    /// <summary>
    /// 更新动画融合
    /// </summary>
    private void UpdateAnimationBlend() {
        Vector3 velocityXZ = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float speed = velocityXZ.magnitude;
        if (speed > (walkSpeed + runSpeed)) {
            fsm.SwitchState(AIState.Run);
            Walk(1f);
        } else if (speed > speedThreshold) {
            fsm.SwitchState(AIState.Walk);
            Walk(0.5f);
        } else {
            fsm.SwitchState(AIState.Idle);
            Walk(0f);
        }
    }
}