using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AKIRA;
using AKIRA.Manager;
using AKIRA.Data;

/// <summary>
/// TODO: Editor下忽略一些Source(Toggle)
/// </summary>
[CustomEditor(typeof(GameApp))]
public class GameAppInspector : Editor {
    private Dictionary<int, List<SourceAttribute>> map = new Dictionary<int, List<SourceAttribute>>();
    private Vector2 view;

    private void OnEnable() {
        map = SourceSystem.Instance.GetSources();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        view = EditorGUILayout.BeginScrollView(view);
        foreach (var kvp in map) {
            EditorGUILayout.BeginVertical("framebox");
            EditorGUILayout.LabelField($"SourceOrder: {kvp.Key.ToString()}");
            var sources = kvp.Value;
            foreach (var source in sources) {
                DrawSourceElement(source);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Test Source")) {
            "测试入口，测试资源加载（非真正加载，已经按照顺序加载）".Log();
            foreach (var values in map.Values) {
                foreach (var source in values) {
                    source.ToString().Log(GameData.Log.Source);
                }
            }
            $"测试结束，调用EventManager触发 {GameData.Event.OnAppSourceEnd}".Log();
        }
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 
    /// </summary>
    private void DrawSourceElement(SourceAttribute source) {
        EditorGUILayout.BeginHorizontal("box");
        var name = source.path.Split("/").Last();
        EditorGUILayout.LabelField($"{source.parentName} : {name}");
        EditorGUILayout.EndHorizontal();
    }
}