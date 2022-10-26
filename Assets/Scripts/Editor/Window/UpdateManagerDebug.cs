using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UpdateManagerDebug : EditorWindow {

    private UpdateManager manager;
    // 折叠
    private static Dictionary<UpdateMode, bool> foldOuts = new Dictionary<UpdateMode, bool>();
    // ScrollView滑动
    private Vector2 scrollView;

    [MenuItem("Tools/Framework/UpdateManager/Debug")]
    private static void ShowWindow() {
        var window = GetWindow<UpdateManagerDebug>();
        window.titleContent = new GUIContent("UpdateManagerDebug");
        window.Show();
    }

    private static void InitFoldOut() {
        foldOuts.Clear();
        foreach (UpdateMode mode in System.Enum.GetValues(typeof(UpdateMode)))
            foldOuts.Add(mode, false);
    }

    private void OnGUI() {
        if (!Application.isPlaying) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("游戏未运行");
            GUILayout.EndHorizontal();
            return;
        }

        if (manager == null)
            manager = UpdateManager.Instance;
        if (foldOuts.Count == 0)
            InitFoldOut();

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        var map = manager.inspectorMap;
        foreach (var kvp in map) {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{kvp.Key}更新列表个数：{kvp.Value.Count}");
            GUILayout.EndHorizontal();
            foldOuts[kvp.Key] = EditorGUILayout.Foldout(foldOuts[kvp.Key], $"{kvp.Key}列表");
            
            // 折叠显示
            if (!foldOuts[kvp.Key])
                continue;
            
            EditorGUILayout.BeginVertical("frameBox");
            for (int i = 0; i < kvp.Value.Count; i++) {
                var update = kvp.Value[i];
                EditorGUILayout.BeginVertical("frameBox");
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Update名称", update.ToString());
                EditorGUILayout.EndHorizontal();

                // EditorGUILayout.BeginHorizontal("box");
                // if (GUILayout.Button(update.ToString())) {
                //     var type = update.GetType();
                //     var gos = GameObject.FindObjectsOfType(type);
                //     foreach (var go in gos)
                //         go.Log();
                // }
                // EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();

        // Editor OnInspectorGUI方法中
        // 强制每帧刷新一次
        // 来源：https://cxywk.com/gamedev/q/QZdC6snb
        if (EditorApplication.isPlaying)
            Repaint();
    }
}