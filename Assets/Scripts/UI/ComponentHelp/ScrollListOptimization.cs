using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AKIRA.UIFramework {
    /// <summary>
    /// 列表排序方式
    /// </summary>
    public enum ListDirection {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 优化列表用的格子接口
    /// </summary>
    internal interface IScrollItem : IPool {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="index"></param>
        void UpdateContent(int index);
    }

    /// <summary>
    /// <para>注意：</para>
    /// <para>Mask为ScrollView父节点</para>
    /// <para>ReorderItemsByPos为滑条OnValueChange触发函数</para>
    /// <para>注意锚点！！！！！！</para>
    /// </summary>
    public class ScrollListOptimization : MonoBehaviour {
        // ScrollView/Viewport/Content
        public RectTransform Container;
        // 遮罩 ScrollView父节点
        public Mask Mask;
        // 格子预制体 继承IScrollItem
        [ExtraInfo("Prefab继承 IScrollItem 接口")]
        public RectTransform Prefab;
        // 生成数量
        public int Num = 1;
        // 间隔
        public float Spacing;
        // 排序方式
        public ListDirection Direction = ListDirection.Horizontal;

        private RectTransform maskRT;
        // 显示的数量
        private int numVisible;
        // 左右两边多出来的显示
        private int numBuffer = 2;
        private float containerHalfSize;
        private float prefabSize;

        private Dictionary<int, int[]> itemDict = new Dictionary<int, int[]>();
        private List<RectTransform> listItemRect = new List<RectTransform>();
        private List<IScrollItem> listItems = new List<IScrollItem>();
        private int numItems = 0;
        // 开始位置
        private Vector3 startPos;
        private Vector3 offsetVec;

        // Use this for initialization
        void Start()
        {
            Container.anchoredPosition3D = new Vector3(0, 0, 0);

            maskRT = Mask.GetComponent<RectTransform>();

            Vector2 prefabScale = Prefab.rect.size;
            prefabSize = (Direction == ListDirection.Horizontal ? prefabScale.x : prefabScale.y) + Spacing;

            Container.sizeDelta = Direction == ListDirection.Horizontal ? (new Vector2(prefabSize * Num, prefabScale.y)) : (new Vector2(prefabScale.x, prefabSize * Num));
            containerHalfSize = Direction == ListDirection.Horizontal ? (Container.rect.size.x * 0.5f) : (Container.rect.size.y * 0.5f);

            numVisible = Mathf.CeilToInt((Direction == ListDirection.Horizontal ? maskRT.rect.size.x : maskRT.rect.size.y) / prefabSize);

            offsetVec = Direction == ListDirection.Horizontal ? Vector3.right : Vector3.down;
            startPos = Container.anchoredPosition3D - (offsetVec * containerHalfSize) + (offsetVec * ((Direction == ListDirection.Horizontal ? prefabScale.x : prefabScale.y) * 0.5f));
            numItems = Mathf.Min(Num, numVisible + numBuffer);
            for (int i = 0; i < numItems; i++)
            {
                GameObject obj = (GameObject)Instantiate(Prefab.gameObject, Container.transform);
                RectTransform t = obj.GetComponent<RectTransform>();
                t.anchoredPosition3D = startPos + (offsetVec * i * prefabSize);
                listItemRect.Add(t);
                itemDict.Add(t.GetInstanceID(), new int[] { i, i });
                obj.SetActive(true);

                IScrollItem li = obj.GetComponentInChildren<IScrollItem>();
                listItems.Add(li);
                li.UpdateContent(i);
            }
            Prefab.gameObject.SetActive(false);
            Container.anchoredPosition3D += offsetVec * (containerHalfSize - ((Direction == ListDirection.Horizontal ? maskRT.rect.size.x : maskRT.rect.size.y) * 0.5f));
        }

        public void ReorderItemsByPos(float normPos)
        {
            if (Direction == ListDirection.Vertical) normPos = 1f - normPos;
            int numOutOfView = Mathf.CeilToInt(normPos * (Num - numVisible));   //number of elements beyond the left boundary (or top)
            int firstIndex = Mathf.Max(0, numOutOfView - numBuffer);   //index of first element beyond the left boundary (or top)
            int originalIndex = firstIndex % numItems;

            int newIndex = firstIndex;
            for (int i = originalIndex; i < numItems; i++)
            {
                moveItemByIndex(listItemRect[i], newIndex);
                listItems[i].UpdateContent(newIndex);
                newIndex++;
            }
            for (int i = 0; i < originalIndex; i++)
            {
                moveItemByIndex(listItemRect[i], newIndex);
                listItems[i].UpdateContent(newIndex);
                newIndex++;
            }
        }

        private void moveItemByIndex(RectTransform item, int index)
        {
            int id = item.GetInstanceID();
            itemDict[id][0] = index;
            item.anchoredPosition3D = startPos + (offsetVec * index * prefabSize);
        }
    }
}