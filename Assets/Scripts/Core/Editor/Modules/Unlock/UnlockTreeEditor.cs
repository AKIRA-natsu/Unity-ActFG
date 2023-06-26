using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using AKIRA.Behaviour.Unlock;
using UnityEditor.Callbacks;
using System.Linq;
using AKIRA.Data;

namespace AKIRA.Editor.Unlock {
    public class UnlockTreeEditor : EditorWindow {
        private UnlockTreeView treeView;
        private InspectorView inspectorView;
        private ToolbarButton resetAllBtn;

        private SerializedObject treeObject;

        [MenuItem("Tools/Framework/UnlockTreeEditor")]
        public static void OpenWindow()
        {
            UnlockTreeEditor wnd = GetWindow<UnlockTreeEditor>();
            wnd.titleContent = new GUIContent("UnlockTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line) {
            if (Selection.activeObject is UnlockTree) {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            var name = typeof(UnlockTreeEditor).Name;
            var location = name.GetScriptLocation().Replace(Application.dataPath, "Assets");

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(location.Replace($"{name}.cs", $"UIBuilder/{name}.uxml"));
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(location.Replace($"{name}.cs", $"UIBuilder/{name}.uss"));
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<UnlockTreeView>();
            inspectorView = root.Q<InspectorView>();
            resetAllBtn = root.Q<ToolbarButton>("ResetAllBtn");
            resetAllBtn.clicked += ResetAllNodeState;

            treeView.onNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }

        private void OnEnable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change) {
            switch (change) {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        private void OnSelectionChange() {
            UnlockTree tree = Selection.activeObject as UnlockTree;
            if (!tree) {
                if (Selection.activeGameObject) {
                    var runner = Selection.activeGameObject.GetComponent<UnlockTreeRunner>();
                    if (runner && runner.tree) {
                        tree = runner.tree;
                    }
                }
            }

            if (Application.isPlaying) {
                if (tree) {
                    treeView.PopulateView(tree);
                }
            } else {
                if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())) {
                    treeView.PopulateView(tree);
                }
            }

            if (tree != null) {
                treeObject = new SerializedObject(tree);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        private void OnNodeSelectionChanged(NodeView view) {
            inspectorView.UpdateSelection(view);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetAllNodeState() {
            if (Application.isPlaying)
                return;
            $"UnlockEditorTree {this}: 重置所有节点的状态".Log(GameData.Log.Editor);
            var header = treeObject.FindProperty("node").objectReferenceValue as Node;
            ResetNodeState(header);
        }

        /// <summary>
        /// 重置节点状态
        /// </summary>
        /// <param name="node"></param>
        private void ResetNodeState(Node node) {
            node.state = Node.State.Lock;
            if (node.children != null && node.children.Count != 0)
                for (int i = 0; i < node.children.Count; i++)
                    ResetNodeState(node.children[i]);
        }
    }
}