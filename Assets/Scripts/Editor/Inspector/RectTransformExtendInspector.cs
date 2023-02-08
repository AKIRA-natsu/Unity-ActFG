using System;
using UnityEngine;
using UnityEditor;
using AKIRA.UIFramework;

/// <summary>
/// RectTransform 扩展更新UI组件
/// </summary>
[CustomEditor(typeof(RectTransform))]
public class RectTransformExtendInspector : DecoratorEditor {
    public RectTransformExtendInspector(): base("RectTransformEditor"){}

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var type = $"{target.name}Panel".GetConfigTypeByAssembley();
        if (type == null) {
            return;
        } else {
            EditorGUILayout.Space();
            if (!Application.isPlaying) {
                if (GUILayout.Button("Update Props")) {
                    GenerateUIGUI.UpdateUI((target as RectTransform).gameObject);
                }
            } else {
                var component = UIManager.Instance.Get(type);
                if (GUILayout.Button("Active")) {
                    if (component.Active) {
                        component.Hide();
                    } else {
                        component.Show();
                    }
                }
            }
        }
    }
}