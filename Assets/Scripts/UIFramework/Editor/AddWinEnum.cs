using System.Linq;
using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ActFG.UIFramework;
using ActFG.Util.Tools;
using ActFG.Attribute;

namespace ActFG.ToolEditor {
    /// <summary>
    /// 添加 UI 标签 (EditorGUI)
    /// </summary>
    public class AddWinEnum : EditorWindow {
        [MenuItem("Tools/UI/AddUIEnum(GUI)")]
        public static void Open() {
            var gui = GetWindow<AddWinEnum>();
            gui.titleContent = new GUIContent("添加UI标签");
        }

        private string filePath = "";
        private string winName = "";
        private string winRemark = "";

        private void OnGUI() {
            GameObject obj = null;
            if (Selection.gameObjects.Length > 0) obj = Selection.gameObjects[0];

            EditorGUI.BeginChangeCheck();
            // title
            EditorGUILayout.LabelField("添加 UI 标签(GUI)");
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("现有标签");
            var EnumNames = Enum.GetNames(typeof(WinEnum));
            for (int i = 0; i < EnumNames.Length; i++) {
                var name = EnumNames[i];
                var field = typeof(WinEnum).GetField(name);
                var remark = field.GetCustomAttributes(typeof(RemarkAttribute), false)[0] as RemarkAttribute;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name, GUILayout.Width(150));
                EditorGUILayout.LabelField(remark.remark);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();

            // paramters
            EditorGUILayout.LabelField("标签名称");
            winName = EditorGUILayout.TextArea(winName);

            EditorGUILayout.LabelField("标签备注");
            winRemark = EditorGUILayout.TextArea(winRemark);
            EditorGUI.EndChangeCheck();

            EditorGUILayout.Space();
            // button
            if (GUILayout.Button("添加 UI 标签")) {
                if (string.IsNullOrEmpty(winName)) {
                    $"没有输入标签名称".StringColor(Color.red).Log();
                    return;
                }
                AddEnum();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        private void AddEnum() {
            filePath = Application.dataPath + "/Scripts/UIFramework/Base/WinEnum.cs";
            if (!File.Exists(filePath)) {
                $"{filePath}\n文件位置不对".StringColor(Color.red).Log();
                return;
            }
            var code = File.ReadAllLines(filePath).ToList();
            var AddCode = 
$@"        [Remark(""{winRemark}"")]
        {winName},";
            code.Insert(code.Count - 2, AddCode);
            File.WriteAllLines(filePath, code.ToArray());
            $"添加{winName}成功".StringColor(Color.cyan).Log();
        }
    }
}