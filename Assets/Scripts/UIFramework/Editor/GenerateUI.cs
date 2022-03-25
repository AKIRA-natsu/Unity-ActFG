using System;
using System.IO;
using UnityEngine;
using ActFG.UIFramework;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ActFG.ToolEditor {
    /// <summary>
    /// 自动生成 UI 脚本
    /// </summary>
    public class GenerateUI : Editor {

        private readonly string UIRulePath = "Resources/Rule/UIComponentRule.txt";

        [MenuItem("Tools/UI/CreateUI(Select Gameobjects)")]
        internal static void CreateUI() {
            var objs = Selection.gameObjects;
            for (int i = 0; i < objs.Length; i++) {
                var obj = objs[i];
                CreateUI(obj.name, WinType.Normal, obj);
//                 var name = obj.name;
//                 var filePath = $"Assets/Scripts/UI/{name}Panel.cs";
//                 var objPath = Application.dataPath + $"/Resources/UI/{name}.prefab";
//                 if (!File.Exists(objPath)) {
//                     var result = false;
//                     PrefabUtility.SaveAsPrefabAsset(objs[i], objPath, out result);
//                     if (!result) {
//                         $"生成预制体失败".Colorful(Color.red).Error();
//                         return;
//                     }
//                 }
//                 objPath = objPath.Replace(".prefab", "");
//                 // 去掉 "UI/" 4
//                 objPath = objPath.Substring(objPath.Length - name.Length - 3);

//                 if (!File.Exists(filePath)) {
//                     string content = 
//                         #region code

// $@"using ActFG.Attribute;
// using UnityEngine.UI;

// namespace ActFG.UIFramework {{
//     [Win(WinEnum.{name}, ""{objPath}"", WinType.Normal)]
//     public class {name}Panel : UIComponent {{

//         public {name}Panel() : base() {{}}

//         public override void Awake() {{
//             base.Awake();
//         }}
//     }}
// }}";
//                         #endregion
                    
//                     File.WriteAllText(filePath, content);
//                     $"生成完毕\n路劲为{filePath}".Colorful(Color.cyan).Log();
//                     AssetDatabase.Refresh();
//                 } else {
//                     $"已经存在该文件".Colorful(Color.cyan).Log();
//                 }
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
            if (!File.Exists(objPath)) {
                var result = false;
                PrefabUtility.SaveAsPrefabAsset(obj, objPath, out result);
                if (!result) {
                    $"保存预制体{obj}失败".Colorful(Color.red).Error();
                    return;
                }
            }
            objPath = objPath.Replace(".prefab", "");
            // 去掉 "UI/" 4
            objPath = objPath.Substring(objPath.Length - name.Length - 3);

            if (!File.Exists(panelPath)) {
                string content = 
                    #region code

$@"using ActFG.Attribute;

namespace ActFG.UIFramework {{
    [Win(WinEnum.{@enum}, ""{objPath}"", WinType.{@type})]
    public class {name}Panel : {name}PanelProp {{
        public override void Awake() {{
            base.Awake();
        }}
    }}
}}";
                    #endregion
                    
                File.WriteAllText(panelPath, content);
                $"生成panel.cs完毕\n路劲为{panelPath}".Colorful(Color.cyan).Log();
                AssetDatabase.Refresh();
            } else {
                $"已经存在panel文件".Colorful(Color.yellow).Log();
            }

// =============================================================================================================================

            if (!File.Exists(propPath)) {
                string content = 
                    #region code

$@"using ActFG.Attribute;
using UnityEngine.UI;

namespace ActFG.UIFramework {{
    public class {name}PanelProp : UIComponent {{



    }}
}}";
                    #endregion
                    
                File.WriteAllText(propPath, content);
                $"生成prop.cs完毕\n路劲为{propPath}".Colorful(Color.cyan).Log();
                AssetDatabase.Refresh();
            } else {
                $"已经存在prop文件".Colorful(Color.yellow).Log();
            }
        }
    }
}