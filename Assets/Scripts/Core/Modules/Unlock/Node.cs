using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Behaviour.Unlock {
    public class Node : ScriptableObject {
        public enum State {
            Lock,
            Check,
            Unlock,
        }

        // lock target
        [HideInInspector]
        public ILock ilock;
        // node state
        [HideInInspector]
        public State state = State.Lock;
        /// <summary>
        /// id
        /// </summary>
        [HideInInspector]
        public string guid = default;
        /// <summary>
        /// view position
        /// </summary>
        [HideInInspector]
        public Vector2 position = default;

        /// <summary>
        /// Lock target path
        /// </summary>
        [HideInInspector]
        public string path;
        /// <summary>
        /// children nodes
        /// </summary>
        public List<Node> children = new List<Node>();

        private static Transform lockRoot;
        /// <summary>
        /// 解锁的父节点
        /// </summary>
        public static Transform LockRoot { 
            get {
                if (lockRoot == null)
                    #if UNITY_EDITOR
                    if (Application.isPlaying)
                        lockRoot = GameObject.FindObjectOfType<UnlockTreeRunner>().transform;
                    else
                        lockRoot = GameObject.Find("[UnlockObjects]").transform;
                    #else
                        lockRoot = GameObject.FindObjectOfType<UnlockTreeRunner>().transform;
                    #endif
                return lockRoot;
            }
        }

        public State Update() {
            if (String.IsNullOrEmpty(path)) {
                return State.Unlock;
            }

            // first time to get ilock interface
            if (ilock == null) {
                ilock = LockRoot.Find(path)?.GetComponent<ILock>();
            }

            // null target, skip unlock check
            if (ilock == null) {
                return State.Unlock;
            }

            // check condition
            switch (state) {
                case State.Lock:
                    state = ilock.CheckCondition() ? State.Check : State.Lock;
                break;
                case State.Check:
                    state = ilock.UnlockCondition() ? State.Unlock : State.Check;
                    if (state == State.Unlock)
                        CompleteNode();
                break;
                case State.Unlock:
                    // 直接解锁的
                    CompleteNode();
                break;
            }

            return state;
        }

        /// <summary>
        /// 解锁节点
        /// </summary>
        private void CompleteNode() {
            ilock.CompleteLock();
            //  $"解锁节点： {ilock}".Log();
        }
    
        public virtual Node Clone()  {
            return Instantiate(this);
        }
    }
}