using System;
using UnityEngine;
using UnityEditor;
using AKIRA.Behaviour.Unlock;

namespace AKIRA.Editor.Unlock {
    [CustomEditor(typeof(Node))]
    public class NodeInspector : UnityEditor.Editor {
        private Transform unlockTarget;

        public override void OnInspectorGUI() {
            // base.OnInspectorGUI();
            
            var node = target as Node;

            GUI.enabled = !Application.isPlaying;
            
            // lock state
            EditorGUILayout.EnumPopup("Node State", node.state);

            // path
            EditorGUILayout.LabelField("Lock Target Path", node.path);

            GUI.enabled = true;
            
            // get lock object in scene
            if (String.IsNullOrEmpty(node.path)) {
                unlockTarget = null;
            } else {
                unlockTarget = Node.LockRoot.Find(node.path);
            }
            
            unlockTarget = (Transform)EditorGUILayout.ObjectField("Unlock Target", unlockTarget, typeof(Transform), true);

            if (unlockTarget != null && unlockTarget.GetComponent<ILock>() != null) {
                node.path = unlockTarget.GetPath().Replace($"{Node.LockRoot.name}/", "");
            }
        }
    }
}