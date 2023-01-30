using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AKIRA.Manager;

/// <summary>
/// Guide Editor
/// </summary>
public class GuideWindow : EditorWindow {
    // XML文件
    private XML xml;

    // 指引列表转换
    private List<GuideInfo> infos = new List<GuideInfo>();
    // ScrollView滑动
    private Vector2 scrollView;

    [MenuItem("Tools/Framework/GuideWindow")]
    private static void ShowWindow() {
        var window = GetWindow<GuideWindow>();
        window.titleContent = new GUIContent("GuideWindow");
        window.Show();
    }

    private void OnGUI() {
        // 第一次打开窗口读取指引数据
        if (xml == null) {
            xml = new XML(GuideManager.GuideDataPath, false);
            // 清掉上一次打开的列表
            infos.Clear();
            if (xml.Exist()) {
                xml.Read((x) => {
                    var nodes= x.SelectSingleNode("Data").ChildNodes;
                    foreach (XmlElement node in nodes) {
                        infos.Add(new GuideInfo() {
                            ID = node.GetAttribute(GuideInfoName.ID).TryParseInt(),
                            completeType = (GuideCompleteType)node.GetAttribute(GuideInfoName.GuideCompleteType).TryParseInt(),
                            isShowBg = node.GetAttribute(GuideInfoName.IsShowBg).TryParseInt() == 1,
                            dialog = node.GetAttribute(GuideInfoName.Dialog),
                            dialogDirection = (GuideDialogDirection)node.GetAttribute(GuideInfoName.DialogDirection).TryParseInt(),
                            arrowTarget = GameObject.Find(node.GetAttribute(GuideInfoName.ArrowTargetPath)),
                            reachDistance = node.GetAttribute(GuideInfoName.ReachDistance).TryParseFloat(),
                            controlByIGuide = node.GetAttribute(GuideInfoName.ControlByIGuide).TryParseInt() == 1,
                        });
                    }
                });
            } else {
                xml.Create((x) => {
                    var root = x.CreateElement("Data");
                    x.AppendChild(root);
                });
                AssetDatabase.Refresh();
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label($"Path: {GuideManager.GuideDataPath}");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create Guide")) {
            infos.Add(new GuideInfo() {
                ID = infos.Count,
            });
        }

        if (GUILayout.Button("Save File"))
            SaveFile();
        
        if (GUILayout.Button("Delete File"))
            DeleteFile();

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        for (int i = 0; i < infos.Count; i++) {
            var info = infos[i];

            // 设置排颜色（info类决定）
            var message = info.GetInfoGUIMessage();
            GUI.color = message.Color;
            GUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("frameBox");
            if (!String.IsNullOrEmpty(message.WarnMessage))
                EditorGUILayout.HelpBox(message.WarnMessage, MessageType.Info);
            EditorGUI.BeginDisabledGroup(true);
            info.ID = EditorGUILayout.IntField(GuideInfoName.ID, i);
            EditorGUI.EndDisabledGroup();
            info.completeType = (GuideCompleteType)EditorGUILayout.EnumPopup(GuideInfoName.GuideCompleteType, info.completeType);

            if (info.completeType != GuideCompleteType.None)
                info.arrowTarget = (GameObject)EditorGUILayout.ObjectField(GuideInfoName.ArrowTargetPath, info.arrowTarget, typeof(GameObject), allowSceneObjects : true);

            // 检测是否有IGuide接口
            info.controlByIGuide = info.arrowTarget != null && info.arrowTarget.TryGetComponent<IGuide>(out _);

            if (info.completeType == GuideCompleteType.TDWorld) {
                if (!info.controlByIGuide)
                    info.reachDistance = EditorGUILayout.FloatField(GuideInfoName.ReachDistance, info.reachDistance);
            }

            if (info.completeType == GuideCompleteType.UIWorld) {
                info.isShowBg = EditorGUILayout.Toggle(GuideInfoName.IsShowBg, info.isShowBg);
                info.dialogDirection = (GuideDialogDirection)EditorGUILayout.EnumPopup(GuideInfoName.DialogDirection, info.dialogDirection);
                if (info.dialogDirection != GuideDialogDirection.None)
                    info.dialog = EditorGUILayout.TextField(GuideInfoName.Dialog, info.dialog);
            }

            
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Remove"))
                infos.RemoveAt(i--);
            if (GUILayout.Button("Up") && i != 0)
                SwitchIndex(i, i - 1);
            if (GUILayout.Button("Down") && i != infos.Count - 1)
                SwitchIndex(i, i + 1);
            GUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    private void SaveFile() {
        var tempMap = new Dictionary<int, GuideInfo>();
        for (int i = 0; i < infos.Count; i++) {
            var info = infos[i];
            if (!info.IsVailiable) {
                infos.RemoveAt(i--);
                continue;
            }
            tempMap.Add(info.ID, info);
        }

        // 删除后的重新新建xml文件
        if (xml == null) {
            xml = new XML(GuideManager.GuideDataPath, false);
            xml.Create((x) => {
                var root = x.CreateElement("Data");
                x.AppendChild(root);
            });
        }

        xml.Update((x) => {
            XmlNode data = x.SelectSingleNode("Data");
            var nodes = data.ChildNodes;
            for (int i = 0; i < nodes.Count; i++) {
                XmlElement node = nodes[i] as XmlElement;
                var id = node.GetAttribute(GuideInfoName.ID).TryParseInt();
                if (tempMap.ContainsKey(id)) {
                    // 更新节点
                    UpdateNode(node, tempMap[id]);
                    // 暂存移除
                    tempMap.Remove(id);
                } else {
                    // 删除也放在更新
                    // map不存在节点移除
                    data.RemoveChild(node);
                    i--;
                }
            }

            // 添加新增节点
            foreach (var info in tempMap.Values) {
                XmlElement node = x.CreateElement($"Guide{info.ID}");
                UpdateNode(node, info);
                data.AppendChild(node);
            }
        });
        "Guide Information => 保存xml".Colorful(Color.green).Log();
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    private void DeleteFile() {
        if (!xml.Exist())
            return;
        xml.Delete();
        infos.Clear();
        xml = null;
        xml = new XML(GuideManager.GuideDataPath, false);
    }

    /// <summary>
    /// 交换键值
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private void SwitchIndex(int a, int b) {
        var temp = infos[a];
        infos[a] = infos[b];
        infos[b] = temp;
    }

    /// <summary>
    /// 更新节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="info"></param>
    private void UpdateNode(XmlElement node, GuideInfo info) {
        node.SetAttribute(GuideInfoName.ID, info.ID.ToString());
        node.SetAttribute(GuideInfoName.GuideCompleteType, ((int)info.completeType).ToString());
        node.SetAttribute(GuideInfoName.IsShowBg, (info.isShowBg ? 1 : 0).ToString());
        node.SetAttribute(GuideInfoName.Dialog, info.dialog);
        node.SetAttribute(GuideInfoName.DialogDirection, ((int)info.dialogDirection).ToString());
        node.SetAttribute(GuideInfoName.ArrowTargetPath, info.arrowTarget.GetPath());
        node.SetAttribute(GuideInfoName.ReachDistance, info.reachDistance.ToString());
        node.SetAttribute(GuideInfoName.ControlByIGuide, (info.controlByIGuide ? 1 : 0).ToString());
    }
}
