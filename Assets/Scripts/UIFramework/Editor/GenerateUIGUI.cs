using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using AKIRA.UIFramework;

/// <summary>
/// 自动生成 UI 脚本 (EditorGUI)
/// </summary>
public class GenerateUIGUI : EditorWindow {
    [MenuItem("Tools/Framework/UI/CreateUI(Select Gameobjects)")]
    internal static void CreateUI() {
        var objs = Selection.gameObjects;
        if (objs == null || objs.Length == 0)
            $"未选择物体".Colorful(Color.yellow).Log();
        for (int i = 0; i < objs.Length; i++) {
            var obj = objs[i];
            CreateUI(obj.name, WinType.Normal, obj);
        }
    }

    [MenuItem("Tools/Framework/UI/CreateUI(GUI)")]
    public static void Open() {
        var gui = GetWindow<GenerateUIGUI>();
        gui.titleContent = new GUIContent("自动生成UI");
    }

    private WinEnum chooseWinEnum = WinEnum.None;
    private string winName = "";
    private WinType chooseWinType = WinType.None;

    internal static UIRule rule = null;
    private static List<UINode> nodes = new List<UINode>();
    private static List<string> btns = new List<string>();

    private void OnGUI() {
        GameObject obj = null;
        if (Selection.gameObjects.Length > 0) obj = Selection.gameObjects[0];

        EditorGUI.BeginChangeCheck();
        // title
        EditorGUILayout.LabelField("自动生成 UI (GUI)");
        EditorGUILayout.Space();

        rule = UIRule.DefaultPath.Load<UIRule>();

        if (rule == null) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField($"路径下没有规则文件 => {UIRule.DefaultPath}");
            EditorGUI.EndDisabledGroup();

            // if (GUILayout.Button("规则文件位置")) {
            //     var rulePath = Application.dataPath;
            //     rulePath = EditorUtility.OpenFilePanelWithFilters("选择文件位置", rulePath, new string[] {"Asset", "asset"});

            //     // 加上 Assets
            //     rulePath = rulePath.Replace(Application.dataPath + "/Resources/", "").Split('.')[0];
            //     rule = rulePath.Load<UIRule>();
            // }
            return;
        } else {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("已读取规则文件");
            EditorGUI.EndDisabledGroup();
        }


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
        if (chooseWinEnum != WinEnum.None && !String.IsNullOrEmpty(winName) && chooseWinType != WinType.None) {
            if (GUILayout.Button("生成 UI Prefab & Script")) {
                if (obj == null) {
                    $"没有选择物体".Colorful(Color.red).Log();
                    return;
                }
                if (chooseWinEnum == WinEnum.None || chooseWinType == WinType.None) {
                    $"没有选择UI标签或类型".Colorful(Color.red).Log();
                    return;
                }
                CreateUI(chooseWinEnum.ToString(), chooseWinType, obj);
            }
        }
    }

    /// <summary>
    /// 生成UI脚本并保存ui prefab
    /// </summary>
    internal static void CreateUI(string @enum, WinType @type, GameObject obj) {
        var name = obj.name;
        var panelPath = $"Assets/Scripts/UI/{name}Panel.cs";
        var propPath = $"Assets/Scripts/UI/{name}PanelProp.cs";
        var objPath = Application.dataPath + $"/Resources/UI/{name}.prefab";

        // 检查是否存在预制体
        if (File.Exists(objPath)) {
            $"已经存在预制体{obj}\n进行删除，路径为{objPath}".Colorful(Color.yellow).Log();
            File.Delete(objPath);
        }
        PrefabUtility.SaveAsPrefabAsset(obj, objPath, out bool result);
        if (!result) {
            $"保存预制体{obj}失败".Colorful(Color.red).Error();
            return;
        }
        $"保存预制体{obj}成功\n路径为{objPath}".Colorful(Color.cyan).Log();

        objPath = objPath.Replace(".prefab", "");
        // 去掉 "UI/" 4
        objPath = objPath.Substring(objPath.Length - name.Length - 3);

        // =============================================================================================================================

        if (File.Exists(propPath)) {
            $"已经存在prop文件\n进行删除，路劲为{propPath}".Colorful(Color.yellow).Log();
            File.Delete(propPath);
        }
        string propContent =
        #region code

$@"using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AKIRA.UIFramework {{
    public class {name}PanelProp : UIComponent {{";
        propContent += $"\n{LinkControlContent(obj.transform)}";
        propContent +=
$@"    }}
}}";
        #endregion

        File.WriteAllText(propPath, propContent);
        $"生成prop.cs完毕\n路劲为{propPath}".Colorful(Color.cyan).Log();
        AssetDatabase.Refresh();

        // =============================================================================================================================

        if (File.Exists(panelPath)) {
            $"已经存在panel文件\n进行删除，路劲为{panelPath}".Colorful(Color.yellow).Log();
            File.Delete(panelPath);
        }
        string panelContent =
        #region code

$@"using UnityEngine;

namespace AKIRA.UIFramework {{
    [Win(WinEnum.{@enum}, ""{objPath}"", WinType.{@type})]
    public class {name}Panel : {name}PanelProp {{
        public override void Awake(WinType type) {{
            base.Awake(type);";
        panelContent += $"\n{LinkBtnListen()}";
        panelContent +=
$@"        }}
    }}
}}";
        #endregion

        File.WriteAllText(panelPath, panelContent);
        $"生成panel.cs完毕\n路劲为{panelPath}".Colorful(Color.cyan).Log();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获得UI个节点组件
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns></returns>
    private static StringBuilder LinkControlContent(Transform _transform) {
        if (nodes.Count != 0) nodes.Clear();
        if (btns.Count != 0) btns.Clear();
        if (rule == null) rule = UIRule.DefaultPath.Load<UIRule>();
        TraverseUI(_transform.transform, "");
        StringBuilder content = new StringBuilder();
        // 最后一个是transform根节点
        for (int i = 0; i < nodes.Count - 1; i++) {
            var node = nodes[i];
            // 删去路径根节点  /_transform.name/
            node.path = node.path.Remove(0, _transform.name.Length + 2);
            if (rule.TryGetControlName(node.name, out string controlName)) {
                if (rule.CheckMatchableControl(node.name)) {
                    // content.Append($"        [UIControl(\"{node.path}\", true)]\n        protected {controlName} {node.path.Replace('/', '_').Replace("@", "")};\n");
                    content.Append($"        [UIControl(\"{node.path}\", true)]\n        protected {controlName} {node.name};\n");
                } else {
                    // content.Append($"        [UIControl(\"{node.path}\")]\n        protected {controlName} {node.path.Replace('/', '_').Replace("@", "")};\n");
                    content.Append($"        [UIControl(\"{node.path}\")]\n        protected {controlName} {node.name};\n");
                }
                if (controlName.Equals("Button"))
                    // btns.Add(node.path.Replace('/', '_').Replace("@", ""));
                    btns.Add(node.name);
            }
        }
        return content;
    }

    /// <summary>
    /// 添加按钮监听事件
    /// </summary>
    /// <returns></returns>
    private static StringBuilder LinkBtnListen() {
        StringBuilder content = new StringBuilder();
        foreach (var btn in btns)
            content.Append($"            this.{btn}.onClick.AddListener(() => {{}});\n");
        return content;
    }

    /// <summary>
    /// 遍历UI节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="path"></param>
    private static void TraverseUI(Transform parent, string path) {
        path += $"/{parent.name}";
        if (parent.childCount != 0)
            for (int i = 0; i < parent.childCount; i++)
                TraverseUI(parent.GetChild(i), path);

        nodes.Add(new UINode(parent.name, path));
    }
}