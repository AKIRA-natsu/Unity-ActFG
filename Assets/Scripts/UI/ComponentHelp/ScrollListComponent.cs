using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public interface IScrollItem {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="index"></param>
        void UpdateContent(int index);
    }

    /// <summary>
    /// <para>Scroll View的优化</para>
    /// <para>挂载在Scroll View/Viewport/Content下</para>
    /// <para>显示不正常检查亿下下锚点 (0.5f 0.5f 0.5f 0.5f)</para>
    /// <para>来源：https://github.com/boonyifei/ScrollList</para>
    /// </summary>
    public class ScrollListComponent : MonoBehaviour {
        // 对象
        [SerializeField]
        private RectTransform prefab;
        // 排序方式
        [SerializeField]
        private ListDirection direction = ListDirection.Horizontal;
        private Vector2 prefabScale;
        // 生成数量
        private int createCount = 1;
        // 间隔
        [SerializeField]
        private Vector2Int space;
        // 列数
        [SerializeField, Min(1)]
        private int lineCount = 1;

        // ScrollView 父节点 RectTransform
        private RectTransform parentRT;
        // 显示的数量
        private int numVisible;
        // 左右两边多出来的显示
        private int numBuffer = 2;
        private float containerHalfSize;
        private float prefabSize;

        // 记录物品字典
        private Dictionary<RectTransform, IScrollItem> ItemMap = new();
        private int numItems = 0;
        // 开始位置
        private Vector3 startPos;
        // 排列间距差值
        private Vector3 dirOffsetVec;
        // line间距差值
        private Vector3 lineOffsetVec;

        /// <summary>
        /// 初始化，确保预制体Prefab上挂载IScrollView脚本
        /// </summary>
        /// <param name="number"></param>
        public void Initialization(int number) {
            Initialization(number, null);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="number">总数</param>
        /// <param name="type">继承IScrollItem</param>
        public void Initialization(int number, Type type) {
            numBuffer = numBuffer * lineCount;
            this.createCount = number;
            var container = this.GetComponent<RectTransform>();

            // ScrollView的父节点RectTransform
            parentRT = this.transform.parent.parent.parent.GetComponent<RectTransform>();

            //Three permutations per row, the total number of rows that need to be arranged
            int n = (createCount % lineCount) != 0 ? Mathf.CeilToInt(createCount / lineCount) + 1 : Mathf.CeilToInt(createCount / lineCount);
            (container.sizeDelta, containerHalfSize) = GetContainerValue(n);
            dirOffsetVec = direction == ListDirection.Horizontal ? Vector3.right : Vector3.down;
            lineOffsetVec = direction == ListDirection.Horizontal ? Vector3.down : Vector3.right;
            startPos = container.anchoredPosition3D - (dirOffsetVec * containerHalfSize) + (dirOffsetVec * ((direction == ListDirection.Horizontal ? prefabScale.x : prefabScale.y) * 0.5f))
                - lineOffsetVec * (((direction == ListDirection.Horizontal ? prefabScale.y : prefabScale.x) * 0.5f) * (lineCount - 1));
            numItems = Mathf.Min(createCount, numVisible + numBuffer);
            for (int i = 0; i < numItems; i++) {
                RectTransform trans = Instantiate(prefab, container.transform);
                MoveItemByIndex(trans, i);

                IScrollItem item = type == null ? trans.GetComponent<IScrollItem>() : trans.gameObject.AddComponent(type).GetComponent<IScrollItem>();
                item.UpdateContent(i);
                ItemMap.Add(trans, item);
            }
            GameObject.Destroy(prefab.gameObject);
            container.anchoredPosition3D += dirOffsetVec * (containerHalfSize - ((direction == ListDirection.Horizontal ? parentRT.rect.size.x : parentRT.rect.size.y) * 0.5f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Three permutations per row, the total number of rows that need to be arranged</param>
        /// <returns>prefabScale, Container.sizeDelta,containerHalfSize</returns>
        private (Vector2 ContainersizeDelta, float containerHalfSize) GetContainerValue(int n) {
            prefabScale = prefab.rect.size;
            float containerWeigth;
            Vector2 ContainersizeDelta;
            float containerHalfSize;
            if (direction == ListDirection.Horizontal) {
                prefabSize = prefabScale.x + space.x;
                containerWeigth = (prefabScale.y + space.y) * lineCount - space.y;
                ContainersizeDelta = new Vector2(prefabSize * n, containerWeigth);
                containerHalfSize = ContainersizeDelta.x * 0.5f;
                numVisible = Mathf.CeilToInt((parentRT.rect.size.x) / prefabSize) * lineCount;
            } else {
                prefabSize = prefabScale.y + space.y;
                containerWeigth = (prefabScale.x + space.x) * lineCount - space.x;
                ContainersizeDelta = new Vector2(containerWeigth, prefabSize * n);
                containerHalfSize = ContainersizeDelta.y * 0.5f;
                numVisible = Mathf.CeilToInt((parentRT.rect.size.y) / prefabSize) * lineCount;
            }
            return (ContainersizeDelta, containerHalfSize);
        }

        /// <summary>
        /// 滑条注册事件
        /// </summary>
        /// <param name="normPos"></param>
        public void ReorderItemsByPos(float normPos) {
            if (direction == ListDirection.Vertical) normPos = 1f - normPos;
            int numOutOfView = Mathf.CeilToInt(normPos * (createCount - numVisible));   //number of elements beyond the left boundary (or top)
            int firstIndex = Mathf.Max(0, numOutOfView - numBuffer);   //index of first element beyond the left boundary (or top)
            int originalIndex = (firstIndex) % numItems;
            int newIndex = firstIndex;
            for (int i = originalIndex; i < numItems; i++) {
                MoveItemByIndex(ItemMap.Keys.ElementAt(i), newIndex);
                ItemMap.Values.ElementAt(i).UpdateContent(newIndex);
                newIndex++;
            }

            for (int i = 0; i < originalIndex; i++) {
                // 防止越界
                if (newIndex >= createCount)
                    break;
                MoveItemByIndex(ItemMap.Keys.ElementAt(i), newIndex);
                ItemMap.Values.ElementAt(i).UpdateContent(newIndex);
                newIndex++;
            }
        }

        private void MoveItemByIndex(RectTransform item, int index) {
            int tempIndex = index / lineCount;
            int tempRemainder = index % lineCount;

            var prefabSizeF = direction == ListDirection.Horizontal ? prefabScale.y + space.y : prefabScale.x + space.x;
            item.anchoredPosition3D = startPos + (dirOffsetVec * tempIndex) * prefabSize + (lineOffsetVec * tempRemainder) * prefabSizeF;
        }
    }
}