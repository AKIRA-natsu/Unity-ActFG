using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using AKIRA.Data;

public class LanguageConvertXmlWindow : EditorWindow {
    // 文件夹
    private string folderPath;
    // 输出路径
    private string outputPath;

    [MenuItem("Tools/Framework/Language/LanguageConvertXmlWindow")]
    private static void ShowWindow() {
        var window = GetWindow<LanguageConvertXmlWindow>();
        window.titleContent = new GUIContent("LanguageConvertXmlWindow");
        window.Show();
    }

    private void OnGUI() {
        if (String.IsNullOrEmpty(folderPath))
            folderPath = $"{Application.dataPath}/Resources/Texts";
        if (String.IsNullOrEmpty(outputPath))
            outputPath = Application.streamingAssetsPath;

        EditorGUILayout.BeginVertical("framebox");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Folder", GUILayout.Width(50f));
        EditorGUILayout.TextField(folderPath);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Choose Fold")) {
            folderPath = EditorUtility.OpenFolderPanel("Select Texts Folder", "", "");
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("OutPut", GUILayout.Width(50f));
        EditorGUILayout.TextField(outputPath);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Choose output")) {
            outputPath = EditorUtility.OpenFolderPanel("Select OutPut Folder", "", "");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Convert To Xml")) {
            ConvertToXml();
        }

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 转换到XML
    /// </summary>
    private void ConvertToXml() {
        var paths = Directory.GetFiles(folderPath);
        var fields = typeof(LanguageText).GetFields();
        foreach (var path in paths) {
            if (path.Contains(".meta") || !path.Contains(".xlsx"))
                continue;
            var xmlFile = Path.Combine(Application.streamingAssetsPath, path.Split(@"\")[1].Replace(".xlsx", ".xml"));
            var datas = path.ExcelConvertToClass<LanguageText>(GameData.DLL.AKIRA_Runtime);
            var result = XML.XmlSerialize<List<LanguageText>>(datas);
            File.WriteAllText(xmlFile, result);
        }
        AssetDatabase.Refresh();
    }
}