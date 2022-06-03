//注意，图片尺寸过大可能会运行卡顿或者直接数组越界
using System.Collections.Generic;
using UnityEngine;

public struct Pos
{
    public int i;
    public int j;
    public Pos(int i, int j) : this()
    {
        this.i = i;
        this.j = j;
    }

    public Pos(float i, float j) : this()
    {
        this.i = Mathf.RoundToInt(i);
        this.j = Mathf.RoundToInt(j);
    }
}

/// <summary>
/// https://blog.csdn.net/LittleWhiteLv/article/details/107008660
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class Split : MonoBehaviour
{

    [Range(1, 20)]
    public int radiateNum = 5;      //裂痕的数量
    [Range(0, 45)]
    public int changeAngle = 15;    //随机裂痕的弯曲程度

    private Color[] color;
    private int w, h;

    private void Start()
    {
        Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;
        if (!texture.isReadable)
        {
            Debug.LogError("图片不可读！请勾选图片属性中的Read/Write Enabled选项.");
            return;
        }
        color = texture.GetPixels();
        w = texture.width;
        h = texture.height;
    }

    private void OnMouseDown()
    {
        Vector3 clickPos = new Vector3(w, h) * 0.5f;
        Vector3 pos1 = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 100;
        pos1.x /= transform.lossyScale.x;
        pos1.y /= transform.lossyScale.y;
        Vector3 pos2 = new Vector3();
        float angle = -transform.eulerAngles.z * Mathf.Deg2Rad;
        pos2.x = pos1.x * Mathf.Cos(angle) - pos1.y * Mathf.Sin(angle);
        pos2.y = pos1.x * Mathf.Sin(angle) + pos1.y * Mathf.Cos(angle);
        clickPos += pos2;
        CreateSprites((int)clickPos.x, (int)clickPos.y);
    }

    //创建分裂图
    private void CreateSprites(int center_j, int center_i)
    {
        //随机分裂角度
        float[] angle = new float[radiateNum];
        int num = 0;
        bool retry; //随机角度接近时要重新生成
        float delt;
        while (num != radiateNum)
        {
            angle[num] = Random.Range(0, 2 * Mathf.PI);
            retry = false;
            for (int i = 0; i < num; i++)
            {
                delt = Mathf.Abs(angle[i] - angle[num]);
                if (delt < Mathf.PI / radiateNum / 2 || delt > 2 * Mathf.PI - Mathf.PI / radiateNum / 2)
                {
                    retry = true;
                    break;
                }
            }
            if (!retry)
            {
                num++;
            }
        }

        //绘制随机裂痕
        bool[] line = new bool[w * h];
        line[center_i * w + center_j] = true;
        int pixelL = 20;        //分段长度
        for (int i = 0; i < radiateNum; i++)
        {
            Vector2 curVec = new Vector2(center_i, center_j);
            Pos curPos;
            Vector2 step = new Vector2(Mathf.Cos(angle[i]), Mathf.Sin(angle[i])) * 0.98f;
            int stepNum = 1;
            while (true)
            {
                if (stepNum % pixelL == 0)
                {
                    angle[i] += Random.Range(-changeAngle * Mathf.Deg2Rad, changeAngle * Mathf.Deg2Rad);
                    step = new Vector2(Mathf.Cos(angle[i]), Mathf.Sin(angle[i])) * 0.98f;
                }
                curVec += step;
                curPos = new Pos(curVec.x, curVec.y);
                if (curPos.i >= 0 && curPos.j >= 0 && curPos.i < h && curPos.j < w)
                {
                    line[curPos.i * w + curPos.j] = true;
                }
                else
                {
                    break;
                }
                stepNum++;
            }
        }

        //向四周扩展从而获取每块图像
        GameObject g1 = new GameObject("sprites");
        int splitNum = 0;
        for (int n = 0; n < w * h; n++)
        {
            if (!line[n])
            {
                splitNum++;
                List<Pos> list = new List<Pos>();
                List<Pos> list1 = new List<Pos>();
                list.Add(new Pos(n / w, n % w));
                list1.Add(new Pos(n / w, n % w));
                line[n] = true;
                int left, right, down, up;
                left = right = n % w;
                down = up = n / w;
                while (list.Count != 0)
                {
                    Pos p = list[0];
                    list.RemoveAt(0);
                    if (p.i > 0 && !line[(p.i - 1) * w + p.j])
                    {
                        list.Add(new Pos(p.i - 1, p.j));
                        list1.Add(new Pos(p.i - 1, p.j));
                        line[(p.i - 1) * w + p.j] = true;
                        if (p.i - 1 < down)
                        {
                            down = p.i - 1;
                        }
                    }
                    if (p.j > 0 && !line[p.i * w + p.j - 1])
                    {
                        list.Add(new Pos(p.i, p.j - 1));
                        list1.Add(new Pos(p.i, p.j - 1));
                        line[p.i * w + p.j - 1] = true;
                        if (p.j - 1 < left)
                        {
                            left = p.j - 1;
                        }
                    }
                    if (p.i < h - 1 && !line[(p.i + 1) * w + p.j])
                    {
                        list.Add(new Pos(p.i + 1, p.j));
                        list1.Add(new Pos(p.i + 1, p.j));
                        line[(p.i + 1) * w + p.j] = true;
                        if (p.i + 1 > up)
                        {
                            up = p.i + 1;
                        }
                    }
                    if (p.j < w - 1 && !line[p.i * w + p.j + 1])
                    {
                        list.Add(new Pos(p.i, p.j + 1));
                        list1.Add(new Pos(p.i, p.j + 1));
                        line[p.i * w + p.j + 1] = true;
                        if (p.j + 1 > right)
                        {
                            right = p.j + 1;
                        }
                    }
                }

                //创建sprite
                PhysicsMaterial2D m;
                m = new PhysicsMaterial2D("m");
                m.bounciness = 0.3f;    //物体边缘的弹性和摩擦系数
                m.friction = 0.6f;
                int w1 = right - left + 1;
                int h1 = up - down + 1;
                int pos_x = (left + right - w) / 2;
                int pos_y = (down + up - h) / 2;
                Vector3 pos1 = new Vector3(pos_x, pos_y, 0) / 100;
                Vector3 pos2 = new Vector3();
                float angle1 = transform.eulerAngles.z * Mathf.Deg2Rad;
                pos1.x *= transform.lossyScale.x;
                pos1.y *= transform.lossyScale.y;
                pos2.x = pos1.x * Mathf.Cos(angle1) - pos1.y * Mathf.Sin(angle1);
                pos2.y = pos1.x * Mathf.Sin(angle1) + pos1.y * Mathf.Cos(angle1);
                if (list1.Count > 20)
                { //大于20像素才会创建
                    Texture2D t1 = new Texture2D(w1, h1);
                    Color[] c1 = new Color[w1 * h1];
                    foreach (Pos p in list1)
                    {
                        c1[(p.i - down) * w1 + p.j - left] = color[p.i * w + p.j];
                    }
                    t1.SetPixels(c1);
                    t1.Apply();
                    GameObject g = new GameObject("sprite");
                    g.transform.SetParent(g1.transform);
                    g.transform.position = transform.position + pos2;
                    g.transform.eulerAngles = new Vector3(0, 0, angle1 * Mathf.Rad2Deg);
                    g.transform.localScale = transform.lossyScale;
                    g.AddComponent<SpriteRenderer>().sprite = Sprite.Create(t1, new Rect(0, 0, w1, h1), new Vector2(0.5f, 0.5f));
                    g.AddComponent<PolygonCollider2D>().sharedMaterial = m;
                    g.AddComponent<Rigidbody2D>();
                    float mass;
                    if (GetComponent<Rigidbody2D>())
                    {
                        g.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
                        mass = list1.Count / (float)(w * h) * GetComponent<Rigidbody2D>().mass * 2;
                    }
                    else
                    {
                        mass = list1.Count / (float)(w * h) * 20;    //重新分配每块的质量
                    }
                    g.GetComponent<Rigidbody2D>().mass = mass;
                    g.AddComponent<Split>().radiateNum = radiateNum;
                }
            }
        }
        //销毁原物体
        Destroy(gameObject);
    }

}