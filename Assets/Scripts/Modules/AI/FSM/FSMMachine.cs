using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.Behaviour.AI {
    /// <summary>
    /// 状态机
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent]
    public class FSMMachine : MonoBehaviour {
        /// <summary>
        /// 包含的状态机
        /// </summary>
        [SerializeField]
        private List<AIState> containStates = new();

        /// <summary>
        /// 状态列表
        /// </summary>
        /// <returns></returns>
        private Dictionary<AIState, IState> StateMap = new();
        /// <summary>
        /// 当前状态
        /// </summary>
        public AIState curState { get; private set; } = AIState.None;
        /// <summary>
        /// 当前状态表现
        /// </summary>
        private IState stateBehaviour;

        [SerializeField]
        private AIDataConfig config;
        /// <summary>
        /// AI数据
        /// </summary>
        public AIDataConfig Config => config;
        /// <summary>
        /// 路径标签
        /// </summary>
        [HideInInspector]
        public string wayTag = default;

        /// <summary>
        /// AI
        /// </summary>
        private NavMeshAgent agent;
        /// <summary>
        /// 是否到达目的地
        /// </summary>
        public bool Reach => agent.remainingDistance <= Config.reachDestination;

        private void Awake() {
            foreach (var state in containStates) {
                var behaviour = $"{state}State".GetConfigTypeByAssembley()?.CreateInstance<IState>();
                if (behaviour != null) {
                    $"add {state}".Log();
                    StateMap.Add(state, behaviour);
                }
            }

            if (config == null) {
                config = AIDataConfig.GetDefaultConfig();
            }

            agent = this.GetComponent<NavMeshAgent>();
            agent.stoppingDistance = config.reachDestination;
        }

        private void Start() {
            // 按顺序进入第一个存在表现得状态
            foreach(var state in containStates) {
                if (StateMap.ContainsKey(state)) {
                    TransitionState(state);
                    break;
                }
            }
        }

        /// <summary>
        /// 恢复状态机
        /// </summary>
        public void Resume() {}

        /// <summary>
        /// 停止状态机
        /// </summary>
        public void Stop() {}

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="targetState"></param>
        public void TransitionState(AIState targetState) {
            if (curState == targetState) {
                return;
            }
            
            if (!StateMap.ContainsKey(targetState)) {
                $"{this} Dont Contain State Behaviour => {targetState}".Colorful(Color.yellow).Log();
                return;
            }

            stateBehaviour?.Exit();
            stateBehaviour = StateMap[targetState];
            curState = stateBehaviour.Enter();
        }

        public void GameUpdate(out object data) {
            stateBehaviour?.GameUpdate(this);
            data = SetData();
        }

        /// <summary>
        /// 设置相对应得数据，并返回IAnima所需要得数据
        /// </summary>
        /// <returns></returns>
        private object SetData() {
            switch (curState) {
                case AIState.None:
                    return default;
                case AIState.Idle:
                    return default;
                case AIState.Wait:
                    return 0f;
                case AIState.Partol:
                    agent.speed = config.walkSpeed;
                    return 0.5f;
                case AIState.Move:
                    return default;
                case AIState.Jump:
                    return default;
                case AIState.Climb:
                    return default;
                case AIState.ReadyAttack:
                    return default;
                case AIState.Attack:
                    return default;
                case AIState.Die:
                    return default;
                default:
                    return default;
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            if (Config != null) {
                Gizmos.color = System.Drawing.Color.Orange.ToUnityColor();
                Gizmos.DrawWireSphere(this.transform.position, Config.partolRadius);
            }
        }
        #endif

        /// <summary>
        /// 设置目标点
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool SetDestination(Vector3 targetPosition) {
            return agent.SetDestination(targetPosition);
        }
    }
}