using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AgentLocomotion), true)]
[CanEditMultipleObjects]
public class AgentLocomotionInspector : AIBaseInspector {
    private bool agentFold = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        AgentLocomotion locomotion = target as AgentLocomotion;

        EditorGUILayout.Space();
        // -----------------------------AI Agent-------------------------------
        EditorGUILayout.LabelField("AI Agent");
        EditorGUILayout.BeginHorizontal();
        locomotion.walkSpeed = EditorGUILayout.FloatField("WalkSpeed", locomotion.walkSpeed);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        locomotion.runSpeed = EditorGUILayout.FloatField("RunSpeed", locomotion.runSpeed);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        locomotion.speedThreshold = EditorGUILayout.FloatField("SpeedThreshold", locomotion.speedThreshold);
        EditorGUILayout.EndHorizontal();
    }
}