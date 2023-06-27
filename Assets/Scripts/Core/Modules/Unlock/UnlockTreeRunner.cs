using System.Collections.Generic;
using System.Linq;
using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA.Behaviour.Unlock {
    [Source("Source/Base/[UnlockObjects]", GameData.Source.Base)]
    public class UnlockTreeRunner : MonoBehaviour {
        // 解锁树
        public UnlockTree tree;
        // 锁住节点
        private List<Node> lockNodes = new List<Node>();
        // 解锁节点
        private List<Node> unlockNodes = new List<Node>();

        private void Start() {
            lockNodes.Add(tree.node);
        }

        private void Update() {
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
        }
    }
}