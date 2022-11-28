using AKIRA.AI.Animation;
using AKIRA.AI.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.AI {
    /// <summary>
    /// 人形 基类
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class HumanAgentBase : AIBase {
        // AI
        public NavMeshAgent navMeshAgent { get; private set; }
        /// <summary>
        /// 到达终点
        /// </summary>
        public bool Reach => !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

        /// <summary>
        ///  状态机
        /// </summary>
        /// <value></value>
        public AiStateMachine machine { get; private set; }
        /// <summary>
        /// 动画控制器
        /// </summary>
        /// <value></value>
        public HumanAnimation aiAnimation { get; private set; }

        [SerializeField]
        protected AiState initializeState = AiState.Idle;

        /// <summary>
        /// 一些统一的设置
        /// </summary>
        public AiDefaultConfig config;

        protected virtual void Awake() {
            Render = this.transform.Find("Render");
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            machine = new AiStateMachine(this);
            aiAnimation = new HumanAnimation(Render.GetComponentInChildren<Animator>());

            // 如果不设置就设置默认
            if (config == null)
                config = AiDefaultConfig.DefaultPath.Load<AiDefaultConfig>();
        }

        public override void GameUpdate()
            => machine.GameUpdate();
    }
}