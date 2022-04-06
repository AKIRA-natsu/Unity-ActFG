using System.Reflection;
using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AKIRA.ToolEditor {
    /// <summary>
    /// 自动生成 Mono 脚本 (GUI)
    /// </summary>
    public class GenerateScriptGUI : EditorWindow {
        [MenuItem("Tools/Script/CreateScript")]
        public static void Open() {
            var gui = GetWindow<GenerateScriptGUI>();
            gui.titleContent = new GUIContent("自动生成脚本");
        }

        private readonly string DLLName = "Assembly-CSharp";

        private string path = "";
        private string @namespace = "ActFG.";
        private bool isChild = true;
        private string parent = "";
        private string scriptName = "";

        private MethodInfo[] methods;
        private string[] methodsName = new string[1] { "None" };
        private int index = 0;

        private void OnGUI() {
            EditorGUI.BeginChangeCheck();
            // title
            EditorGUILayout.LabelField("自动生成脚本 (GUI)");
            EditorGUILayout.Space();

            // paramters
            if (GUILayout.Button("存放路劲")) {
                path = EditorUtility.OpenFolderPanel("选择存放路劲", Application.dataPath, " ");
                // 加上 Assets
                path = path.Replace(Application.dataPath + "/", "");
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(path);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("脚本命名空间");
            @namespace = EditorGUILayout.TextArea(@namespace);
            EditorGUILayout.LabelField("脚本名称");
            scriptName = EditorGUILayout.TextArea(scriptName);
            EditorGUILayout.LabelField("是否继承");
            isChild = EditorGUILayout.Toggle(isChild);
            EditorGUI.BeginDisabledGroup(!isChild);
            EditorGUILayout.LabelField("继承自");
            parent = EditorGUILayout.TextArea(parent);
            if (GUILayout.Button("获得方法")) {
                var type = GetConfigTypeByAssembley(parent);
                if (type == null) {
                    $"{parent} 在{DLLName}中不存在".Warn();
                    return;
                }
                methods = type.GetMethods();
                methodsName = new string[methods.Length];
                for (int i = 0; i < methods.Length; i++) {
                    methodsName[i] = methods[i].Name;
                }
            }
            EditorGUILayout.LabelField("继承方法");
            index = EditorGUILayout.MaskField(index, methodsName);
            EditorGUI.EndDisabledGroup();

            // button
            EditorGUILayout.Space();
            if (GUILayout.Button("生成脚本")) {
                if (string.IsNullOrEmpty(path)) {
                    $"没有选择路劲".Colorful(Color.yellow).Log();
                    return;
                }
                CreateScripts();
            }
        }

        /// <summary>
        /// 不同程序集
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public Type GetConfigTypeByAssembley(string className) {
            var types = Assembly.Load(DLLName).GetTypes();
            foreach (var type in types) {
                if (type.Name.Contains(className)) {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateScripts() {
            var filePath = "Assets/" + path + "/" + scriptName + ".cs";
            if (File.Exists(filePath)) {
                $"{filePath} has existed".Error();
                return;
            }

            // 泛型继承
            if (parent.Contains("Singleton")) {
                parent = $"{parent}<{scriptName}>";
            }

            string content = 
            #region code
$@"using System;
using UnityEngine;

namespace {@namespace} {{
    public class {scriptName} {(isChild ? $": {parent} " : "")}{{
        ";
        
//         // 继承方法
        for (int i = 0; i < methodsName.Length; i++) {
            if ((index & 1 << i) != 0) {
                if (methods[i].IsPrivate) {
                    content += 
$@"
        private override {methods[i].ReturnType} {methods[i].Name} {{}}
";
                } else if (methods[i].IsPublic) {
                    content += 
$@"
        public override {methods[i].ReturnType} {methods[i].Name} {{}}
";
                } else {
                    content += 
$@"
        protect override {methods[i].ReturnType} {methods[i].Name} {{}}
";
                }
            }
        }

        content += $@"
    }}
}}
";
            #endregion

            File.WriteAllText(filePath, content);
            $"{scriptName} script生成 => {path}".Colorful(Color.yellow).Log();
            AssetDatabase.Refresh();
        }
    }
}