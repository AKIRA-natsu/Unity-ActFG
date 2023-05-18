using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using AKIRA.Data;

public class XMLWindow : EditorWindow {
    // 
    private Vector2 scrollPosition;
    // XML过滤文件名称
    private const string BuildHelpName = "XmlBuildHelp.xml";
    // 过滤保存
    public static XML xml {get; private set; } = new XML(BuildHelpName);
    // 过滤字典
    private Dictionary<string, bool> FilterMap;

    [MenuItem("Tools/Framework/Persistence/XmlBuildHelp")]
    private static void ShowWindow() {
        var window = GetWindow<XMLWindow>();
        window.titleContent = new GUIContent("XMLWindow");
        window.Show();

        var path = Application.streamingAssetsPath;
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }

    private void OnGUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("打包过滤StreamingAssets下xml文件（Auto Save）");
        EditorGUILayout.EndHorizontal();
        if (Directory.Exists(XmlBuildHelp.TempFolder)) {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("打包报错中断恢复~Temp按钮");
            if (GUILayout.Button("Resume ~TempXml Files")) {
                XmlBuildHelp.ResumeFiles();
            }
            EditorGUILayout.EndVertical();
            return;
        }
        // 读取Xml保存数据
        if (FilterMap == null || !FilterMap.ContainsKey(BuildHelpName)) {
            FilterMap = new();
            if (xml.Exist()) {
                xml.Read(x => {
                    var data = x.SelectSingleNode("BuildData");
                    var nodes = data.ChildNodes;
                    foreach (XmlElement node in nodes)
                        FilterMap.Add(node.Name.Replace("_", " "), Convert.ToBoolean(node.GetAttribute("Filter")));
                });
            } else {
                xml.Create(x => {
                    var data = x.CreateElement("BuildData");
                    var node = x.CreateElement(BuildHelpName);
                    node.SetAttribute("Filter", true.ToString());
                    data.AppendChild(node);
                    x.AppendChild(data);
                });
                FilterMap.Add(BuildHelpName, true);
                AssetDatabase.Refresh();
            }
        }

        // 只找xml文件
        DirectoryInfo direction = new DirectoryInfo(Application.streamingAssetsPath);
        var files = GetFileNames(direction.GetFiles("*.xml", SearchOption.AllDirectories));

        if (files.Count == 0) {
            return;
        } else {
            EditorGUILayout.BeginVertical("box");
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            List<string> deleteNodes = new();
            for (int i = 0; i < FilterMap.Count; i++) {
                var kvp = FilterMap.ElementAt(i);
                var name = kvp.Key;
                if (!files.Contains(name)) {
                    deleteNodes.Add(name);
                    FilterMap.Remove(name);
                    i--;
                } else {
                    DrawGUI(name);
                    files.Remove(name);
                }
            }

            // 添加节点
            for (int i = 0; i < files.Count; i++) {
                var file = files[i];
                FilterMap.Add(file, file.Equals(BuildHelpName));
                DrawGUI(file);
            }
            
            // 不为0添加/删除xml保存节点
            if (deleteNodes.Count != 0 || files.Count != 0) {
                xml.Update(x => {
                    var data = x.SelectSingleNode("BuildData");
                    var nodes = data.ChildNodes;
                    for (int i = 0; i < nodes.Count; i++) {
                        var name = nodes[i].Name.Replace("_", " ");
                        if(deleteNodes.Contains(name) && !name.Equals(BuildHelpName)) {
                            data.RemoveChild(nodes[i--]);
                        }
                        if (files.Contains(name)) {
                            files.Remove(name);
                        }
                    }
                    for (int i = 0; i < files.Count; i++) {
                        var node = x.CreateElement($"{files[i].Split(".")[0].Replace(" ", "_")}.xml");
                        node.SetAttribute("Filter", FilterMap[files[i]].ToString());
                        data.AppendChild(node);
                    }
                });
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // 删除存档
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("删除勾选Xml文件及存档")) {
                PlayerPrefsHelp.Delete();
                foreach (var kvp in FilterMap) {
                    if (kvp.Key.Equals(BuildHelpName))
                        continue;
                    if (!kvp.Value)
                        continue;
                    File.Delete(Path.Combine(Application.streamingAssetsPath, kvp.Key.Replace("_", " ")));
                }

                $"清除存档文档 及 PlayerPrefs存档结束".Log(GameData.Log.Editor);
                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndVertical();
        }
    }

    /// <summary>
    /// 获得文件所有名称
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    private List<string> GetFileNames(in FileInfo[] files) {
        List<string> result = new();
        foreach (var file in files)
            result.Add(file.Name);
        return result;
    }

    /// <summary>
    /// 绘制列表元素
    /// </summary>
    private void DrawGUI(string key) {
        EditorGUILayout.BeginHorizontal("framebox");
        EditorGUI.BeginDisabledGroup(key.Equals(BuildHelpName));
        EditorGUI.BeginChangeCheck();
        FilterMap[key] = EditorGUILayout.Toggle(FilterMap[key]);
        if (EditorGUI.EndChangeCheck()) {
            xml.Update(x => {
                var data = x.SelectSingleNode("BuildData");
                var nodes = data.ChildNodes;
                foreach (XmlElement node in nodes) {
                    if (node.Name.Replace("_", " ").Equals(key)) {
                        node.SetAttribute("Filter", FilterMap[key].ToString());
                        break;
                    }
                }
            });
        }
        EditorGUILayout.LabelField(key);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
    }
}