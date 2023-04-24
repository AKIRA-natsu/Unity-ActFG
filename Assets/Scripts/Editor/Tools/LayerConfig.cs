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

    static LayerConfig() { }

    /// <summary>
    /// 更新Layer
    /// </summary>
    /// <param name="path"></param>
    // [MenuItem("Tools/Framework/LayerConfig/Update Layer")]
    [MenuItem("Tools/Framework/Update Layer")]
    private static void UpdateLayer() {
        var path = Path.Combine(Application.dataPath, "Scripts/Other/Layer.cs");
        $"Layer Update: Path => {path}".Log();
        // // 检查文件是否还存在
        // if (!File.Exists(path)) {
        //     DeletePathData();
        //     "文件不存在".Colorful(Color.red).Log();
        //     return;
        // }

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

}