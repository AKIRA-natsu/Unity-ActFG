using UnityEngine;
using AKIRA.Manager;

/// <summary>
/// 路线
/// </summary>
public class PathDebug : ReferenceBase {
    // 路线物体
    private GameObject pathLine;
    // 线条
    private LineRenderer line;
    // 路线颜色
    public Gradient color = new Gradient();
    // 路线长度
    public float width;

    // 线条默认材质球名称
    private static string LineShaderName = "Legacy Shaders/Particles/Alpha Blended Premultiply";

    public PathDebug() {}

    /// <summary>
    /// 初始化
    /// 在Wake后
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public PathDebug Init(GameObject parent) {
        pathLine.SetParent(parent);
        return this;
    }

    public override void Wake() {
        base.Wake();
        pathLine = new GameObject("PathLine", typeof(LineRenderer));
        line = pathLine.GetComponent<LineRenderer>();
        line.sharedMaterial = new Material(Shader.Find(LineShaderName));

        color.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.cyan, 0), };
        color.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), };
        width = 1f;

        RefreshPath();
    }

    public override void Recycle() {
        base.Recycle();
        GameObject.DestroyImmediate(pathLine);
    }

    /// <summary>
    /// 更新线条参数
    /// </summary>
    public void RefreshPath() {
        line.colorGradient = color;
        line.startWidth = width;
        line.endWidth = width;
    }

    /// <summary>
    /// 设置路线
    /// </summary>
    /// <param name="path"></param>
    /// <param name="offsetY">y轴差值</param>
    public void SetPath(Vector3[] path, float offsetY) {
        for (int i = 0; i < path.Length; i++)
            path[i] = path[i] + Vector3.up * offsetY;
        line.positionCount = path.Length;
        for (int i = 0; i < path.Length; i++)
            line.SetPosition(i, path[i]);
    }
}