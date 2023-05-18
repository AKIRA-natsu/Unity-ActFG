using System;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.Data {
    /// <summary>
    /// Quad Tree Data
    /// </summary>
    public interface IQuadTreeData : IDisposable {
        Vector3 position { get; }
        QuadTree node { get; set; }
    }

    /// <summary>
    /// Quad Tree
    /// </summary>
    public class QuadTree {
        // quadrant
        private Vector2 min;
        private Vector2 max;
        // max depth
        private int maxDepth;
        // datas max count
        private int maxCount;

        public QuadTree[] children = new QuadTree[4];
        public List<IQuadTreeData> datas = new();

        // is the leaf of the tree
        public bool isLeaf = false;

        // Constructors
        public QuadTree(Vector2 min, Vector2 max, int maxDepth, int maxCount) {
            this.min = min;
            this.max = max;
            this.maxDepth = maxDepth;
            this.maxCount = maxCount;
        }

        public QuadTree(Vector2 min, Vector2 max, int maxDepth) : this(min, max, maxDepth, 4) {}

        /// <summary>
        /// Build QuadTree
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="depth"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public QuadTree RecursiveBuild(Vector2 min, Vector2 max, int depth, params IQuadTreeData[] datas) {
            // cur bounds have no datas
            // return null
            if (datas == null || datas.Length == 0) {
                return null;
            }

            // datas length reach maxCount or reach maxDeaph
            // create node as leaf and return
            if (datas.Length < maxCount || depth == maxDepth) {
                QuadTree leaf = depth == 0 ? this : new QuadTree(min, max, maxDepth, maxCount);
                for (int i = 0; i < datas.Length; i++) {
                    var data = datas[i];
                    if (IsContain(data, min, max)) {
                        leaf.datas.Add(data);
                        data.node = leaf;
                    }
                }
                leaf.isLeaf = true;
                return leaf;
            }

            // get 4 quadrants
            GetQuadrants(min, max, out Vector2[] subMin, out Vector2[] subMax);
            // init list
            List<List<IQuadTreeData>> newDatas = new List<List<IQuadTreeData>>();
            for (int i = 0; i < 4; i++) {
                newDatas.Add(new List<IQuadTreeData>());
            }

            for (int i = 0; i < datas.Length; i++) {
                var data = datas[i];
                for (int j = 0; j < 4; j++) {
                    if (!IsContain(data, subMin[j], subMax[j]))
                        continue;
                    newDatas[j].Add(data);
                    break;
                }
            }

            QuadTree node = depth == 0 ? this : new QuadTree(min, max, maxDepth, maxCount);
            for (int i = 0; i < 4; i++) {
                node.children[i] = RecursiveBuild(subMin[i], subMax[i], depth + 1, newDatas[i].ToArray());
            }

            return node;
        }

        /// <summary>
        /// <para>Destory QuadTree</para>
        /// <para>Foreach data call IQuadTreeData.Dispose()</para>
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public QuadTree RecursiveDestory(QuadTree tree) {
            if (tree == null)
                return null;
            
            // delete children node
            for (int i = 0; i < 4; i++) {
                tree.children[i] = RecursiveDestory(tree.children[i]);
            }

            // call IDispose
            foreach (var data in tree.datas) {
                data.Dispose();
            }
            // delete node
            return null;
        }

        /// <summary>
        /// Find Data To Destory
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool RecursiveDestory(IQuadTreeData data) {
            if (this.datas.Contains(data)) {
                data.Dispose();
                datas.Remove(data);
                return datas.Count == 0;
            }

            for (int i = 0; i < 4; i++) {
                var children = this.children[i];
                if (children == null)
                    continue;
                if (children.RecursiveDestory(data)) {
                    if (children.datas.Count == 0) {
                        this.children[i] = null;
                        break;
                    }
                }
            }

            if (children[0] == null && children[1] == null && children[2] == null && children[3] == null) {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Insert QuadTree Node
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="depth"></param>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool RecursiveInsert(Vector2 min, Vector2 max, int depth, QuadTree node, IQuadTreeData data) {
            // Dont Contain in bounds
            if (!IsContain(data, min, max)) {
                return false;
            }

            GetQuadrants(min, max, out Vector2[] subMin, out Vector2[] subMax);
            
            // reach max depth
            if (depth == maxDepth) {
                datas.Add(data);
                data.node = node;
                return true;
            }

            // Split self
            if (node.isLeaf) {
                node.datas.Add(data);
                data.node = node;
                if (node.datas.Count >= node.maxCount) {
                    for (int i = 0; i < 4; i++) {
                        node.children[i] = new QuadTree(subMin[i], subMax[i], maxDepth, maxCount);
                        node.children[i].isLeaf = true;
                    }
                    node.isLeaf = false;

                    for (int i = 0; i < node.datas.Count; i++) {
                        var tempData = node.datas[i];
                        for (int j = 0; j < 4; j++) {
                            if (!IsContain(tempData, subMin[j], subMax[j]))
                                continue;
                            node.children[j].datas.Add(tempData);
                            tempData.node = node.children[j];
                        }
                    }
                    node.datas.Clear();
                }
                return true;
            }

            for (int i = 0; i < 4; i++) {
                if (IsContain(data, subMin[i], subMax[i])) {
                    if (node.children[i] == null) {
                        node.children[i] = new QuadTree(subMin[i], subMax[i], maxDepth, maxCount);
                        node.children[i].isLeaf = true;
                    }
                        
                    return RecursiveInsert(subMin[i], subMax[i], depth + 1, node.children[i], data);
                }
            }

            return false;
        }

        /// <summary>
        /// Find Data To Destory
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool RecursiveRemove(IQuadTreeData data) {
            if (this.datas.Contains(data)) {
                // Compare to RecursiveDestory: Dont Call Dispose
                // data.Dispose();
                datas.Remove(data);
                return datas.Count == 0;
            }

            for (int i = 0; i < 4; i++) {
                var children = this.children[i];
                if (children == null)
                    continue;
                if (children.RecursiveRemove(data)) {
                    if (children.datas.Count == 0) {
                        this.children[i] = null;
                        break;
                    }
                }
            }

            if (children[0] == null && children[1] == null && children[2] == null && children[3] == null) {
                return true;
            }
            
            return false;
        }


        [Obsolete("not test yet", true)]
        public bool RecursiveRemove(Vector2 min, Vector2 max, int depth, QuadTree node, IQuadTreeData data) {
            if (node == null)
                return false;
            
            if (!IsContain(data, min, max))
                return false;
            
            if (depth == maxDepth || node.isLeaf) {
                if (datas.Contains(data)) {
                    datas.Remove(data);
                }
                // if datas is Empty, told parent to delete it
                if (datas.Count == 0)
                    return true;
                else
                    return false;
            }

            // ! isLeaf
            for (int i = 0; i < 4; i++) {
                var children = node.children[i];
                if (RecursiveRemove(children.min, children.max, depth + 1, children, data)) {
                    node.children[i] = null;
                    break;
                }
            }

            // children empty, delete self
            if (node.children[0] == null && node.children[1] == null &&
                node.children[2] == null && node.children[3] == null)
                return true;

            return false;
        }

        public static void RecursiveTraverse(Vector2 min, Vector2 max, QuadTree node, ref List<Vector2> lines) {
            Vector2 center = (min + max) * 0.5f;
            float length = Mathf.Max(max.x - min.x, max.y - min.y);
            Vector2[] corners = new Vector2[4];
            corners[0] = min;
            corners[1] = min + new Vector2(length, 0f);
            corners[2] = max;
            corners[3] = min + new Vector2(0f, length);

            lines.Add(corners[0]);
            lines.Add(corners[1]);

            lines.Add(corners[1]);
            lines.Add(corners[2]);

            lines.Add(corners[2]);
            lines.Add(corners[3]);

            lines.Add(corners[3]);
            lines.Add(corners[0]);

            if (node == null || node.isLeaf)
                return;
            
            GetQuadrants(min, max, out Vector2[] subMin, out Vector2[] subMax);
            for (int i = 0; i < 4; i++) {
                RecursiveTraverse(subMin[i], subMax[i], node.children[i], ref lines);
            }
        }

        /// <summary>
        /// Quad Tree Update Node
        /// </summary>
        /// <param name="root"></param>
        /// <param name="data"></param>
        public bool RecursiveUpdate(QuadTree root, IQuadTreeData data) {
            // out of root bounds
            if (OutBounds(data, root.min, root.max))
                return false;

            // Still in quadrant
            if (IsContain(data, min, max))
                return false;

            root.RecursiveRemove(data);
            root.RecursiveInsert(root.min, root.max, 0, root, data);
            return true;
        }

        /// <summary>
        /// Get 4 Quadrants and center
        /// </summary>
        /// <param name="subMin"></param>
        /// <param name="subMax"></param>
        private static void GetQuadrants(Vector2 min, Vector2 max, out Vector2[] subMin, out Vector2[] subMax) {
            // subdivied into 4 sub nodes
            Vector2 center = (min + max) * 0.5f;
            float length = Mathf.Max(max.x - min.x, max.y - min.y);

            // -------------
            // |  3  |  2  |
            // -------------
            // |  0  |  1  |
            // -------------
            subMin = new Vector2[4];
            subMax = new Vector2[4];

            subMin[0] = min;
            subMax[0] = center;
            subMin[1] = center - new Vector2(0f, length / 2f);
            subMax[1] = center + new Vector2(length / 2f, 0f);
            subMin[2] = center;
            subMax[2] = max;
            subMin[3] = min + new Vector2(0f, length / 2f);
            subMax[3] = center + new Vector2(0f, length / 2f);
        }

        /// <summary>
        /// Check data is in bounds
        /// </summary>
        /// <param name="quadTreeData"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool IsContain(IQuadTreeData quadTreeData, Vector2 min, Vector2 max) {
            var position = quadTreeData.position;
            return position.x >= min.x && position.x <= max.x
                && position.z >= min.y && position.z <= max.y;
        }

        /// <summary>
        /// Check data is out of bounds
        /// </summary>
        /// <param name="quadTreeData"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool OutBounds(IQuadTreeData quadTreeData, Vector2 min, Vector2 max) {
            var position = quadTreeData.position;
            return position.x < min.x || position.x > max.x
                || position.z < min.y || position.z > max.y;
        }
    }
}