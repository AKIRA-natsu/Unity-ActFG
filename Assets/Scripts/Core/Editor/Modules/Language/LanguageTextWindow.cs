using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using AKIRA.Data;
using AKIRA.Manager;

/// <summary>
/// 文本类更新脚本
/// </summary>
public class LanguageTextWindow : EditorWindow {
    // 脚本内容
    private string path;
    // 是否选择了语言
    private string[] names;
    private bool[] chooses;
    // scroll view
    private Vector2 view;

    [MenuItem("Tools/Framework/Language/LanguageTextWindow")]
    private static void ShowWindow() {
        var window = GetWindow<LanguageTextWindow>();
        window.titleContent = new GUIContent("LanguageTextWindow");
        window.Show();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitLanguageMap() {
        path = "LanguageText".GetScriptLocation();
        if (String.IsNullOrEmpty(path)) {
            path = "LanguageManager".GetScriptLocation().Replace("LanguageManager", "LanguageText");
            $"创建LanguageText脚本在LanguageManager同级文件夹: {path}".Log(GameData.Log.Editor);
            using (var stream = File.Create(path)) {
                stream.Close();
                AssetDatabase.Refresh();
            }
        }
        var lines = File.ReadAllLines(path);
        names = Enum.GetNames(typeof(SystemLanguage));
        chooses = new bool[names.Length];
        // 初始化字典
        for (int i = 0; i < names.Length; i++) {
            chooses[i] = false;
            foreach (var line in lines) {
                if (line.Trim().Replace("public string ", "").Replace(";", "").Equals(names[i])) {
                    chooses[i] = true;
                    break;
                }
            }
        }
    }

    private void OnGUI() {
        if (String.IsNullOrEmpty(path) || names == null || names.Length == 0 || chooses == null || chooses.Length == 0)
            InitLanguageMap();
        view = EditorGUILayout.BeginScrollView(view);
        EditorGUILayout.BeginVertical("framebox");
        var count = names.Length;
        for (int i = 0; i < count; i++) {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(names[i]);
            chooses[i] = EditorGUILayout.Toggle(chooses[i]);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Write Script")) {
            WriteScript();
        }
    }

    /// <summary>
    /// 写入文本
    /// </summary>
    private void WriteScript() {
        var content = @"using UnityEngine;
        
/// <summary>
/// <para>文本</para>
/// <para>Create&Update By LanguageTextWindow</para>
/// </summary>
[System.Serializable]
public class LanguageText {
    public string textID;
            ";

        // 添加方法
        var function = @"
    /// <summary>
    /// 通过language返回对应的语言文本
    /// </summary>
    public string GetLanguageTextValue(SystemLanguage language) {
        switch (language) {";

        for (int i = 0; i < chooses.Length; i++) {
            if (!chooses[i])
                continue;
            content += @$"
    public string {names[i]};
            ";
            
            function += @$"
            case SystemLanguage.{names[i]}:
            return {names[i]};
            ";
        }

        function += @"
        }
        return default;
    }";

        content += function;
        content += @"
}";

        File.WriteAllText(path, content);

        // 更改配置文件
        var config = GameData.Path.LanguageConfig.Load<LanguageConfig>();
        config.UpdateFontConfig(chooses);
        Selection.activeObject = config;

        AssetDatabase.Refresh();
    }
}