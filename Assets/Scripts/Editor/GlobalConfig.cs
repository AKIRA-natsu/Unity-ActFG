using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 层级配置
/// 改手动更新
/// </summary>
// [InitializeOnLoad]
public class LayerConfig {
    /// <summary>
    /// 存储键值
    /// </summary>
    internal const string LayerConfigKey = "LayerConfigPath";

    static LayerConfig() { }

    /// <summary>
    /// 更新Layer
    /// </summary>
    /// <param name="path"></param>
    [MenuItem("Tools/Framework/LayerConfig/Update Layer")]
    private static void UpdateLayer() {
        var path = EditorPrefsHelp.GetString(LayerConfigKey);
        if (String.IsNullOrEmpty(path))
            return;
        // 检查文件是否还存在
        if (!File.Exists(path)) {
            DeletePathData();
            "文件不存在".Colorful(Color.red).Log();
            return;
        }

        var content = @"/// <summary>
/// <para>层级</para>
/// <para>Create&Update By GlobalConfig</para>
/// </summary>
public static class Layer {
            ";
        for (int i = 0; i < 32; i++) {
            var name = LayerMask.LayerToName(i).Replace(" ", "");
            if (String.IsNullOrEmpty(name))
                continue;
            content += @$"
    /// <summary>
    /// {name}
    /// </summary>
    public static readonly int {name} = {i};
                ";
            }
        content += @"
}

            ";

        File.WriteAllText(path, content);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Framework/LayerConfig/Show Layer Save Path")]
    private static void ShowLayerSavePath() {
        var path = EditorPrefsHelp.GetString(LayerConfigKey);
        path.GetString().Colorful(Color.green).Log();
    }

    [MenuItem("Tools/Framework/LayerConfig/Select & Save Path Data")]
    private static void SelectPathData() {
        var path = EditorUtility.OpenFilePanel("Select Layer.cs File", Application.dataPath, "cs");
        // 确保后缀
        if (path.EndsWith(".cs")) {
            $"保存LayerConfig路径位置 => {path}".Colorful(Color.cyan).Log();
            EditorPrefsHelp.Set(LayerConfigKey, path);
            UpdateLayer();
        }
    }

    [MenuItem("Tools/Framework/LayerConfig/Delete Path Data")]
    private static void DeletePathData() {
        $"删除LayerConfig保存路径".Colorful(Color.cyan).Log();
        EditorPrefsHelp.Delete(LayerConfigKey);
    }

}