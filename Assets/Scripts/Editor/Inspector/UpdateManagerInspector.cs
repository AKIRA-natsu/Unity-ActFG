using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UpdateManager))]
public class UpdateManagerInspector : Editor {
    // 折叠
    private static Dictionary<UpdateMode, bool> foldOuts = new Dictionary<UpdateMode, bool>();
    
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        
        if (!Application.isPlaying) {
            EditorGUILayout.HelpBox("Game Not Start", MessageType.Info);
        } else {
            ShowDebugWindow(target as UpdateManager);
        }
    }

    private static void InitFoldOut() {
        foldOuts.Clear();
        foreach (UpdateMode mode in System.Enum.GetValues(typeof(UpdateMode)))
            foldOuts.Add(mode, false);
    }

    private void ShowDebugWindow(UpdateManager manager) {
        if (foldOuts.Count == 0)
            InitFoldOut();

        var map = manager.inspectorMap;
        foreach (var kvp in map) {
            GUI.enabled = false;  
            EditorGUILayout.LabelField($"{kvp.Key}更新列表个数：{kvp.Value.Count}");  
            GUI.enabled = true;

            EditorGUILayout.BeginVertical("frameBox");
            foldOuts[kvp.Key] = EditorGUILayout.Foldout(foldOuts[kvp.Key], $"{kvp.Key}列表");
            
            // 折叠显示
            if (!foldOuts[kvp.Key]) {
                // continue;
            } else {
                EditorGUILayout.BeginVertical("frameBox");
                for (int i = 0; i < kvp.Value.Count; i++) {
                    var update = kvp.Value[i];
                    EditorGUILayout.BeginVertical("frameBox");
                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.LabelField("Update名称", update.ToString());
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        // Editor OnInspectorGUI方法中
        // 强制每帧刷新一次
        // 来源：https://cxywk.com/gamedev/q/QZdC6snb
        // if (EditorApplication.isPlaying)
            Repaint();
    }
}