using AKIRA.Manager;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// <para>提示文字</para>
/// <para>材质球 transparent，放在场景中，非UI</para>
/// </summary>
[RequireComponent(typeof(TextMeshPro))]
public class TipUI : MonoBehaviour, IPool
{
    // 提示文字
    private TextMeshPro text;

    private void Awake() {
        text = this.GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// 播放文字
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color">文字颜色</param>
    public void Play(string text, Color color)
    {
        color.a = 0;
        this.text.color = color;

        this.text.text = text;
        this.transform.localScale = Vector3.one * 0.3f;
        this.transform.DOScale(1f, 0.5f);
        this.text.DOFade(1f, 0.5f).OnComplete(() =>
        {
            this.text.DOFade(0f, 0.3f).OnComplete(() =>
            {
                ObjectPool.Instance.Destory(this);
            });
        });
    }

    public void Wake() {}
    public void Recycle() { }
}