using System.Collections.Generic;
using System.Linq;
using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Behaviour.Unlock {
    [Source("Source/Base/[UnlockObjects]", GameData.Source.Scene)]
    public class UnlockTreeRunner : MonoBehaviour, IUpdate {
        // 解锁树
        public UnlockTree tree;
        // 锁住节点
        private List<Node> lockNodes = new List<Node>();
        // 解锁节点
        private List<Node> unlockNodes = new List<Node>();

        private void Start() {
            if (tree == null || tree.node == null)
                return;
            lockNodes.Add(tree.node);
            EventManager.Instance.AddEventListener(GameData.Event.OnGameStart, OnGameStart);
        }

        /// <summary>
        /// 游戏开始，开始更新判断解锁
        /// </summary>
        /// <param name="data"></param>
        private void OnGameStart(object data) {
            $"进入游戏，开始解锁".Log();
            this.Regist();
            EventManager.Instance.RemoveEventListener(GameData.Event.OnGameStart, OnGameStart);
        }

        public void GameUpdate() {
            for (int i = 0; i < lockNodes.Count; i++) {
                var node = lockNodes[i];
                var state = node.Update();
                if (state == Node.State.Unlock) {
                    unlockNodes.Add(node);
                    lockNodes.RemoveAt(i--);
                }
            }

            if (lockNodes.Count == 0) {
                for (int i = 0; i < unlockNodes.Count; i++)
                    lockNodes.AddRange(unlockNodes[i].children);
                lockNodes = lockNodes.Where((x, i) => lockNodes.FindIndex(z => z.guid == x.guid) == i).ToList();
                unlockNodes.Clear();
            }

            if (lockNodes.Count == 0 && unlockNodes.Count == 0) {
                this.Remove();
            }
        }
    }
}