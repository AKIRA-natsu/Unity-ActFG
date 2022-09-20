using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace ActFG.Behaviour {
    public enum EmenyType {
        None,
        /// <summary>
        /// 史莱姆
        /// </summary>
        Suramu,
    }

    public enum EmenyDifficult {
        None,
        Easy,
        Normal,
        Hard,
    }

    public enum EmenyState {
        Idle,
        Chase,
        Attack,
        Dead,
    }

    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public abstract class EmenyBehaviourBase : MonoBehaviour {
        public class EmenyData {
            public EmenyType emenyType;
            public EmenyDifficult emenyDifficult;

            public EmenyData(EmenyType type = EmenyType.None, EmenyDifficult difficult = EmenyDifficult.None) {
                this.emenyType = type;
                this.emenyDifficult = difficult;
            }
        }

        /// <summary>
        /// 怪物数据
        /// </summary>
        public EmenyData data;
        
        /// <summary>
        /// 怪物状态
        /// </summary>
        public EmenyState state;

        protected NavMeshAgent agent;

        /// <summary>
        /// 出生点
        /// </summary>
        protected Vector3 OriginPosition;

        /// <summary>
        /// 视图半径
        /// </summary>
        [SerializeField, Header("视图半径")]
        protected float Radius;

        /// <summary>
        /// 移动速度
        /// </summary>
        [SerializeField, Header("移动速度")]
        protected float Speed;

        /// <summary>
        /// 血量
        /// </summary>
        [SerializeField, Header("血量")]
        public float HP;

        protected virtual void Awake() {
            data = new EmenyData();
            agent = this.gameObject.GetComponent<NavMeshAgent>();
            OriginPosition = this.transform.position;
            Speed = agent.speed;
        }

        /// <summary>
        ///  获得怪物状态
        /// </summary>
        /// <returns></returns>
        public abstract EmenyState GetEmenyState();
        /// <summary>
        /// 找到玩家
        /// </summary>
        /// <returns></returns>
        public abstract bool FoundPlayer();

        // TODO: 还有一堆事情要弄
        
    }
}