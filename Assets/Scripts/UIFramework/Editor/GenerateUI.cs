using System;
using System.IO;
using UnityEngine;
using ActFG.UIFramework;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ActFG.Util.Tools;

namespace ActFG.ToolEditor {
    /// <summary>
    /// 自动生成 UI 脚本
    /// </summary>
    public class GenerateUI : Editor {
        [MenuItem("Tools/UI/CreateUI(Select Gameobjects)")]
        internal static void CreateUI() {
            var objs = Selection.gameObjects;
            for (int i = 0; i < objs.Length; i++) {
                var obj = objs[i];
                var name = obj.name;
                var filePath = $"Assets/Scripts/UI/{name}Panel.cs";
                var objPath = Application.dataPath + $"/Resources/UI/{name}.prefab";
                if (!File.Exists(objPath)) {
                    var result = false;
                    PrefabUtility.SaveAsPrefabAsset(objs[i], objPath, out result);
                    if (!result) {
                        $"生成预制体失败".StringColor(Color.red).Error();
                        return;
                    }
                }
                objPath = objPath.Replace(".prefab", "");
                // 去掉 "UI/" 4
                objPath = objPath.Substring(objPath.Length - name.Length - 3);

                if (!File.Exists(filePath)) {
                    string content = 
                        #region code

$@"using ActFG.Attribute;
using UnityEngine.UI;

namespace ActFG.UIFramework {{
    [Win(WinEnum.{name}, ""{objPath}"", WinType.Normal)]
    public class {name}Panel : UIComponent {{

        public {name}Panel() : base() {{}}

        public override void Awake() {{
            base.Awake();
        }}
    }}
}}";
                        #endregion
                    
                    File.WriteAllText(filePath, content);
                    $"生成完毕\n路劲为{filePath}".StringColor(Color.yellow).Log();
                    AssetDatabase.Refresh();
                } else {
                    $"已经存在该文件".StringColor(Color.yellow).Log();
                }
            }
        }

        internal static void CreateUI(WinEnum @enum, WinType @type, GameObject obj) {
            var name = obj.name;
            var filePath = $"Assets/Scripts/UI/{name}Panel.cs";
            var objPath = Application.dataPath + $"/Resources/UI/{name}.prefab";
            if (!File.Exists(objPath)) {
                var result = false;
                PrefabUtility.SaveAsPrefabAsset(obj, objPath, out result);
                if (!result) {
                    $"生成预制体失败".StringColor(Color.red).Error();
                    return;
                }
            }
            objPath = objPath.Replace(".prefab", "");
            // 去掉 "UI/" 4
            objPath = objPath.Substring(objPath.Length - name.Length - 3);

            if (!File.Exists(filePath)) {
                string content = 
                    #region code

$@"using ActFG.Attribute;
using UnityEngine.UI;

namespace ActFG.UIFramework {{
    [Win(WinEnum.{@enum}, ""{objPath}"", WinType.{@type})]
    public class {name}Panel : UIComponent {{

        public {name}Panel() : base() {{}}

        public override void Awake() {{
            base.Awake();
        }}
    }}
}}";
                    #endregion
                    
                File.WriteAllText(filePath, content);
                $"生成完毕\n路劲为{filePath}".StringColor(Color.yellow).Log();
                AssetDatabase.Refresh();
            } else {
                $"已经存在该文件".StringColor(Color.yellow).Log();
            }
        }
    }
}