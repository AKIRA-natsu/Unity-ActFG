using System.Collections;
using System.Collections.Generic;
using AKIRA.Manager;
using UnityEngine;

public class CrackControl : MonoBehaviour, IPool
{
    public Crack crackPrefab;
    public float openValue;
    public float speed;
    public float range;

    private static string crackPath = "Prefab/CrackControl";
    private List<Crack> cracks = new List<Crack>();
    private List<CrackControl> crackControls = new List<CrackControl>();

    public void Open(bool isSlideCrack) {
        StartCoroutine(CrackOpen(isSlideCrack));
    }

    public void Close() {
        StartCoroutine(CrackClose());
    }

    /// <summary>
    /// 开启裂纹
    /// </summary>
    /// <param name="isSlideCrack"></param>
    /// <returns></returns>
    private IEnumerator CrackOpen(bool isSlideCrack) {
        int range = Mathf.RoundToInt(this.range);
        Vector3 startPoint = transform.position;

        for (int i = 0; i < range; i += crackPrefab.length) {
            Crack crack = ObjectPool.Instance.Instantiate(crackPrefab, this.transform);
            crack.transform.position = startPoint;
            crack.transform.forward = this.transform.forward;
            startPoint += transform.forward * crackPrefab.length;

            for (int j = 0; j < crack.blendShapeCount; j++) {
                crack.SetBlendShape(j, 0);
            }

            cracks.Add(crack);
        }

        // 开启
        for (int i = 0; i < cracks.Count; i++) {
            for (int j = 1; j < cracks[i].blendShapeCount; j++) {
                if (i == cracks.Count - 1 && j == cracks[i].blendShapeCount - 1) {
                    break;
                }

                if (!isSlideCrack)
                    InstantiateSideCrack(i, j);

                float lerp = 0;
                while (lerp < 1) {
                    cracks[i].SetBlendShape(j, openValue * lerp);

                    if (j == cracks[i].blendShapeCount - 1 && i < cracks.Count - 1) {
                        cracks[i + 1].SetBlendShape(0, openValue * lerp);
                    }

                    lerp += Time.deltaTime * speed;
                    yield return null;
                }
                cracks[i].SetBlendShape(j, openValue);

                if (j == cracks[i].blendShapeCount - 1 && i < cracks.Count - 1) {
                    cracks[i + 1].SetBlendShape(0, openValue);
                }
            }
        }

        yield return null;
    }

    /// <summary>
    /// 创建边缘裂纹
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    private void InstantiateSideCrack(int i, int j)
    {
        float chance = Random.Range(0f, 1f);
        if (chance < 0.5f) {
            Transform point = cracks[i].corners[j];
            CrackControl crackControl = ObjectPool.Instance.Instantiate<CrackControl>(crackPath, this.transform);
            crackControl.transform.position = point.position;
            crackControl.transform.forward = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * point.forward;
            crackControl.openValue = Random.Range(openValue / 4f, openValue / 2f);
            crackControl.range  = Random.Range(2, 4);
            crackControl.Open(true);
            crackControls.Add(crackControl);
        }
    }

    /// <summary>
    /// 集体关闭并回收
    /// </summary>
    /// <returns></returns>
    private IEnumerator CrackClose() {
        for (int i = 0; i < crackControls.Count; i++) {
            crackControls[i].Close();
        }

        float lerp = 0;
        while (lerp < 1) {
            for (int i = 0; i < cracks.Count; i++) {
                for (int j = 0; j < cracks[i].blendShapeCount; j++) {
                    cracks[i].SetBlendShape(j, cracks[i].GetBlendShape(j) * (1 - lerp));
                }
            }
            lerp += Time.deltaTime;
            yield return null;
        }

        yield return null;
        for (int i = 0; i < cracks.Count; i++)
            ObjectPool.Instance.Destory(cracks[i]);

        cracks.Clear();

        for (int i = 0; i < crackControls.Count; i++) {
            ObjectPool.Instance.Destory(crackControls[i]);
        }
        crackControls.Clear();
    }

    public void Wake() {}

    public void Recycle() {
        for (int i = 0; i < cracks.Count; i++)
            ObjectPool.Instance.Destory(cracks[i]);
        cracks.Clear();


        for (int i = 0; i < crackControls.Count; i++)
            ObjectPool.Instance.Destory(crackControls[i]);
        crackControls.Clear();
    }
}
