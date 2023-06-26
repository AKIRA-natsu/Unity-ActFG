using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AKIRA.Behaviour.Unlock {
    /// <summary>
    /// 解锁树
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockTree", menuName = "Framework/UnlockTree", order = 0)]
    public class UnlockTree : ScriptableObject {
        public Node node;
        public List<Node> nodes = new List<Node>();

        public Node.State Update() {
            return node.Update();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Create Node
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(Vector2 position) {
            Node node = ScriptableObject.CreateInstance<Node>();
            node.name = "Node";
            node.guid = GUID.Generate().ToString();
            node.position = position;

            Undo.RecordObject(this, "Unlock Tree(Create node)");
            nodes.Add(node);

            if (!Application.isPlaying) {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Unlock Tree(Create node)");

            AssetDatabase.SaveAssets();

            return node;
        }

        /// <summary>
        /// Delete Node
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(Node node) {
            Undo.RecordObject(this, "Unlock Tree(Delete node)");
            nodes.Remove(node);
            
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void AddChild(Node parent, Node child) {
            Undo.RecordObject(parent, "Unlock Tree(Add Child)");
            parent.children.Add(child);
            EditorUtility.SetDirty(parent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void RemoveChild(Node parent, Node child) {
            Undo.RecordObject(parent, "Unlock Tree(Remove Child)");
            parent.children.Remove(child);
            EditorUtility.SetDirty(parent);
        }
#endif

        public void Traverse(Node node, System.Action<Node> visiter) {
            if (node) {
                visiter.Invoke(node);
                var children = node.children;
                children.ForEach(node => Traverse(node, visiter));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UnlockTree Clone() {
            UnlockTree tree = Instantiate(this);
            tree.node = tree.node.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.node, tree.nodes.Add);
            return tree;
        }
    }
}