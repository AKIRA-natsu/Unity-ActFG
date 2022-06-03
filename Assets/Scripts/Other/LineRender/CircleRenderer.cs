using UnityEngine;

/// <summary>
/// <para>LineRenderer圆圈</para>
/// <para>来源: https://www.youtube.com/watch?v=DdAfwHYNFOE</para>
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour {
    private LineRenderer circleRenderer;

    private void Awake() {
        circleRenderer = this.GetComponent<LineRenderer>();
    }

    private void DrawCircle(int steps, float radius) {
        circleRenderer.positionCount = steps;
        for (int currentStep = 0; currentStep < steps; currentStep++) {
            float currentProgress = (float)currentStep / steps;
            float currentRadian = currentProgress * 2 * Mathf.PI;

            float xScale = Mathf.Cos(currentRadian);
            float yScale = Mathf.Sin(currentRadian);

            float x = radius * xScale;
            float y = radius * yScale;

            Vector3 currentPosition = new Vector3(x, y, 0);
            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}