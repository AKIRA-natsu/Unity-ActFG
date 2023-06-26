using System;
using AKIRA.Behaviour.Unlock;
using Node = AKIRA.Behaviour.Unlock.Node;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace AKIRA.Editor.Unlock {
    public class NodeView : UnityEditor.Experimental.GraphView.Node {
        public Action<NodeView> onNodeSelected;
        public Node node;
        public Port input;
        public Port output;

        public NodeView(Node node) {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts() {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if (input != null) {
                input.portName = "";
                // input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts() {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            if (output != null) {
                output.portName = "";
                // output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }

        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Unlock Tree(Set Position)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected() {
            base.OnSelected();
            if (onNodeSelected != null) {
                onNodeSelected.Invoke(this);
            }
        }
    }
}