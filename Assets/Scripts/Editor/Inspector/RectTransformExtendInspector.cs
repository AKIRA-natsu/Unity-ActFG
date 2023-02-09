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
        var panelType = $"{target.name}Panel".GetConfigTypeByAssembley();
        if (panelType != null) {
            EditorGUILayout.Space();
            if (!Application.isPlaying) {
                if (GUILayout.Button("Update Props")) {
                    GenerateUIGUI.UpdateUI((target as RectTransform).gameObject);
                }
            } else {
                var component = UIManager.Instance.Get(panelType);
                if (GUILayout.Button("Active")) {
                    if (component.Active) {
                        component.Hide();
                    } else {
                        component.Show();
                    }
                }
            }
        }

        var componentType = $"{target.name}Component".GetConfigTypeByAssembley();
        if (componentType != null) {
            EditorGUILayout.Space();
            if (!Application.isPlaying) {
                if (GUILayout.Button("Update Props")) {
                    GenerateUIPropGUI.UpdateUIProp((target as RectTransform).gameObject);
                }
            } else {
                // TODO: 暂时拿不到脚本
            }
        }
    }
}