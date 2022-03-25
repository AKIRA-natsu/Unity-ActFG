using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ActFG.UIFramework;

namespace ActFG.ToolEditor {
    /// <summary>
    /// 自动生成 UI 脚本 (EditorGUI)
    /// </summary>
    public class GenerateUIGUI : EditorWindow {
        [MenuItem("Tools/UI/CreateUI(GUI)")]
        public static void Open() {
            var gui = GetWindow<GenerateUIGUI>();
            gui.titleContent = new GUIContent("自动生成UI");
        }

        private WinEnum chooseWinEnum = WinEnum.None;
        private string winName = "";
        private WinType chooseWinType = WinType.None;

        private void OnGUI() {
            GameObject obj = null;
            if (Selection.gameObjects.Length > 0) obj = Selection.gameObjects[0];

            EditorGUI.BeginChangeCheck();
            // title
            EditorGUILayout.LabelField("自动生成 UI (GUI)");
            EditorGUILayout.Space();

            // paramters
            EditorGUILayout.LabelField("UI 标签");
            chooseWinEnum = (WinEnum)EditorGUILayout.EnumPopup(chooseWinEnum);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("UI 名称");
            if (obj != null) winName = obj.name;
            EditorGUILayout.TextArea(winName);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.LabelField("UI 类型");
            chooseWinType = (WinType)EditorGUILayout.EnumPopup(chooseWinType);
            EditorGUI.EndChangeCheck();

            EditorGUILayout.Space();
            // button
            if (GUILayout.Button("生成 UI Prefab & Script")) {
                if (obj == null) {
                    $"没有选择物体".Colorful(Color.red).Log();
                    return;
                }
                if (chooseWinEnum == WinEnum.None || chooseWinType == WinType.None) {
                    $"没有选择UI标签或类型".Colorful(Color.red).Log();
                    return;
                }
                GenerateUI.CreateUI(chooseWinEnum.ToString(), chooseWinType, obj);
            }
        }
    }
}