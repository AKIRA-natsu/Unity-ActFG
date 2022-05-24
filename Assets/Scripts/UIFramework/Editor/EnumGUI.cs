using System.Globalization;
using System.Linq;
using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using AKIRA.UIFramework;

namespace AKIRA.ToolEditor {
    /// <summary>
    /// 添加 UI 标签 (EditorGUI)
    /// </summary>
    public class EnumGUI : EditorWindow {
        [MenuItem("Tools/UI/Add&Remove Enum(GUI)")]
        public static void Open() {
            var gui = GetWindow<EnumGUI>();
            gui.titleContent = new GUIContent("Enum GUI");
        }

        // 文件位置写死
        private string filePath;

        private string enumName = "";
        private string enumRemark = "";

        private int remove = 0;
        private string removeName = "";

        private void OnGUI() {
            // 添加部分
            // title
            EditorGUILayout.LabelField("添加&删除 Enum(GUI)");
            EditorGUILayout.Space();

            if (GUILayout.Button("Enum文件位置")) {
                if (string.IsNullOrEmpty(filePath))
                    filePath = Application.dataPath;
                filePath = EditorUtility.OpenFilePanelWithFilters("选择文件位置", filePath, new string[] {"CSharp", "cs"});

                // 加上 Assets
                filePath = filePath.Replace(Application.dataPath + "/", "");
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(filePath);
            EditorGUI.EndDisabledGroup();

            if (!string.IsNullOrEmpty(filePath)) {
                // 显示所有已经有的标签
                var scriptName = filePath.Substring(filePath.LastIndexOf('/') + 1).Replace(".cs", "");
                var type = DLLExtend.GetConfigTypeByAssembley(scriptName);

                if (type.IsEnum) {
                    EditorGUILayout.LabelField("现有标签");
                    var EnumNames = Enum.GetNames(type);
                    for (int i = 0; i < EnumNames.Length; i++) {
                        var name = EnumNames[i];
                        var field = type.GetField(name);
                        var remarks = field.GetCustomAttributes(typeof(RemarkAttribute), false) as RemarkAttribute[];
                        // 水平绘制
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(name, GUILayout.Width(150));
                        foreach (var remark in remarks)
                            EditorGUILayout.LabelField(remark.Remark, GUILayout.Width(70));
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.Space();

                    // paramters
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField("标签名称");
                    enumName = EditorGUILayout.TextArea(enumName);

                    EditorGUILayout.LabelField("标签备注");
                    enumRemark = EditorGUILayout.TextArea(enumRemark);
                    EditorGUI.EndChangeCheck();

                    EditorGUILayout.Space();
                    // button
                    if (GUILayout.Button("添加 UI 标签")) {
                        if (string.IsNullOrEmpty(enumName)) {
                            $"没有输入标签名称".Colorful(Color.red).Log();
                            return;
                        }
                        AddEnum();
                        AssetDatabase.Refresh();
                    }

                    // 删除部分
                    EditorGUILayout.Space(40);
                    EditorGUI.BeginChangeCheck();
                    remove = EditorGUILayout.Popup(remove, EnumNames);
                    EditorGUI.EndChangeCheck();
                    if (GUILayout.Button("删除 UI 标签")) {
                        removeName = EnumNames[remove];
                        RemoveEnum();
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        private void AddEnum() {
            var path = Path.Combine(Application.dataPath, filePath).Replace('\\', '/');
            var code = File.ReadAllLines(path).ToList();
            var AddCode = 
$@"        [Remark(""{enumRemark}"")]
        {enumName},";
            code.Insert(code.Count - 2, AddCode);
            File.WriteAllLines(path, code.ToArray());
            $"{path}\n添加{enumName}成功".Colorful(Color.cyan).Log();
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        private void RemoveEnum() {
            var path = Path.Combine(Application.dataPath, filePath).Replace('\\', '/');
            var code = File.ReadAllLines(path).ToList();
            var index = code.Count;
            for (int i = code.Count - 1; i >= 0; i--) {
                if (code[i].Contains(removeName)) {
                    index = i;
                    break;
                }
            }
            // 第一次删除往前移了
            code.RemoveAt(index - 1);
            code.RemoveAt(index - 1);
            File.WriteAllLines(path, code.ToArray());
            $"删除{removeName}成功".Colorful(Color.cyan).Log();
            remove = 0;
            removeName = "";
        }
    }
}