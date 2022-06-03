using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>LineRenderer波浪曲线</para>
/// <para>来源: https://www.youtube.com/watch?v=6C1NPy321Nk</para>
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SineWave : MonoBehaviour
{
    // 线条
    private LineRenderer line;
    // 点个数
    public int points;
    // 振幅
    public float amplitude = 1;
    // 频率
    public float frequency = 1;
    // x轴 范围限制
    public Vector2 xLimits = new Vector2(0, 1);
    // 速度
    public float movementSpeed = 1;

    private void Awake() {
        line = this.GetComponent<LineRenderer>();
    }

    /// <summary>
    /// 绘制
    /// </summary>
    private void Draw() {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        line.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++) {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed));
            line.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    private void Update() {
        Draw();
    }
}
