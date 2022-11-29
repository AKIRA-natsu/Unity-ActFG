using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AKIRA.Behaviour {
    public class EmenyBehaviour : EmenyBehaviourBase {
        // 移动目标点
        private Vector3 TargetPosition;
        // 到达移动点等待时间
        private const float WaitTime = 3f;
        [SerializeField, Header("等待时间")]
        private float _time;

        private GameObject Player;

        protected override void Awake()
        {
            base.Awake();
            this.state = EmenyState.Idle;
            TargetPosition = this.OriginPosition;

            Player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update() {
            StartCoroutine(Move());
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, Radius);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.OriginPosition, Radius);
        }
        
        public override EmenyState GetEmenyState()
        {
            if (this.HP <= 0)
                return EmenyState.Dead;
            if (FoundPlayer() && this.state == EmenyState.Idle)
                return EmenyState.Chase;
                // TODO: Attack
            // if (xxx) return EmenyState.Attack;

            return EmenyState.Idle;
        }

        public override bool FoundPlayer()
        {
            if (Vector3.Distance(this.transform.position, Player.transform.position) <= this.Radius)
                return true;
            return false;
        }

        /// <summary>
        /// 移动
        /// FIXME: 移动上下高度问题
        /// </summary>
        /// <returns></returns>
        private IEnumerator Move() {
            // TODO: 缺少很多东西
            // TODO: 移动等待时间

            if (FoundPlayer()) {
                agent.destination = Player.transform.position;
                yield return null;
            } else {
                if (Vector3.Distance(this.transform.position, this.OriginPosition) >= this.Radius) {
                    TargetPosition = this.OriginPosition;
                    agent.destination = this.OriginPosition;
                }
            }

            // 如果没有走到直接返回
            while (Vector3.Distance(this.transform.position, TargetPosition) >= this.agent.stoppingDistance) {
                yield return null;
            }

            NavWayPoint();
            Debug.Log("移动".Colorful(Color.yellow) + " => " + TargetPosition);
            if (Vector3.Distance(TargetPosition, this.transform.position) > 1.0f) {
                agent.destination = TargetPosition;
            }

            yield return null;
        }


        /// <summary>
        /// 获得随机巡逻点
        /// </summary>
        public void NavWayPoint() {
            var randomX = Random.Range(-Radius, Radius);
            var randomZ = Random.Range(-Radius, Radius);
            TargetPosition = new Vector3(this.OriginPosition.x + randomX, this.transform.position.y, this.OriginPosition.z + randomZ);

            NavMeshHit hit;
            TargetPosition = NavMesh.SamplePosition(TargetPosition, out hit, Radius, 1) ? TargetPosition : this.transform.position;
        }

    }
}
