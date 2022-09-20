using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIBase), true)]
[CanEditMultipleObjects]
public class AIBaseInspector : Editor {
    // 面板存储状态
    private AIState inspectorState = AIState.Idle;
    // 重写
    private bool @override = false;
    // 重写
    private bool drawPath = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        AIBase ai = target as AIBase;

        // -----------------------------AI State-------------------------------
        EditorGUILayout.LabelField("AI State");
        EditorGUILayout.BeginHorizontal();
        @override = EditorGUILayout.Toggle("Override", @override);
        EditorGUILayout.EndHorizontal();
        
        if (!@override) {
            ai.fsm.debug = false;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup("CurState", ai.fsm.CurState);
            EditorGUI.EndDisabledGroup();
        } else {
            ai.fsm.debug = true;
            EditorGUILayout.BeginHorizontal();
            inspectorState = (AIState)EditorGUILayout.EnumPopup("CurState(SwitchState)", inspectorState);
            EditorGUILayout.EndHorizontal();
            ai.fsm.SwitchState(inspectorState);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Called SwitchState Function\nAlso Debug State", MessageType.Info);
            EditorGUILayout.EndHorizontal();
        }

        // -----------------------------AI Path-------------------------------
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("AI Path");
        EditorGUILayout.BeginHorizontal();
        drawPath = EditorGUILayout.Toggle("Draw Path", drawPath);
        EditorGUILayout.EndHorizontal();
        
        if (drawPath) {
            ai.CreatePath();
            EditorGUILayout.BeginHorizontal();
            ai.path.color = EditorGUILayout.GradientField("Path Color", ai.path.color);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            ai.path.width = EditorGUILayout.FloatField("Path Width", ai.path.width);
            EditorGUILayout.EndHorizontal();
            ai.path.RefreshPath();
        } else {
            ai.DestoryPath();
        }

        if (GUI.changed) {
            EditorUtility.SetDirty(ai);
        }

        // Editor OnInspectorGUI方法中
        // 强制每帧刷新一次
        // 来源：https://cxywk.com/gamedev/q/QZdC6snb
        if (EditorApplication.isPlaying)
            Repaint();
    }
}