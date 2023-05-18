using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AKIRA.Data;
using UnityEngine;

namespace AKIRA.Test {
    /// <summary>
    /// 测试四叉树
    /// </summary>
    public class CubeSpawner : MonoBehaviour {
        public Vector2 size;
        public int count;

        private List<GameObject> cubes = new();
        private List<Vector3> lines = new();

        public QuadTree tree;

        private void Start() {
            CreateCubes();
        }

        [ContextMenu("Create Cubes")]
        private void CreateCubes() {
            cubes.ForEach(cube => {
                if (Application.isPlaying)
                    GameObject.Destroy(cube);
                else
                    GameObject.DestroyImmediate(cube);
            });
            cubes.Clear();

            for (int i = 0; i < count; i++) {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(this.transform);
                cube.transform.localPosition = new Vector3(Random.Range(-size.x, size.x), 0, Random.Range(-size.y, size.y));
                cube.AddComponent(typeof(CubeData));
                cubes.Add(cube);
            }

            IQuadTreeData[] datas = new IQuadTreeData[count];
            for (int i = 0; i < count; i++) {
                datas[i] = cubes[i].GetComponent<IQuadTreeData>();
            }
            tree = new QuadTree(-size, size, 10, 4);
            tree.RecursiveBuild(-size, size, 0, datas);
            DrawLines();
        }
        
        private void OnDrawGizmos() {
            if (lines.Count == 0)
                return;
            
            for (int i = 0; i < lines.Count; i+=2) {
                Gizmos.DrawLine(lines[i], lines[i + 1]);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.C)) {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(this.transform);
                cube.transform.localPosition = new Vector3(Random.Range(-size.x, size.x), 0, Random.Range(-size.y, size.y));
                cube.AddComponent<CubeData>();
                cubes.Add(cube);
                tree.RecursiveInsert(-size, size, 0, tree, cube.GetComponent<IQuadTreeData>());
                DrawLines();
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                var index = Random.Range(0, cubes.Count);
                tree.RecursiveDestory(cubes[index].GetComponent<IQuadTreeData>());
                cubes.RemoveAt(index);
                DrawLines();
            }
        }

        public void DrawLines() {
            lines.Clear();
            List<Vector2> result = new List<Vector2>();
            QuadTree.RecursiveTraverse(-size, size, tree, ref result);

            foreach (var line in result) {
                lines.Add(new Vector3(line.x, 0, line.y));
            }
        }
    }

    public class CubeData : MonoBehaviour, IQuadTreeData {
        public Vector3 position => this.transform.position;

        public QuadTree node { get; set; }
        public CubeSpawner spawner;

        private void Start() {
            spawner = this.GetComponentInParent<CubeSpawner>();
        }

        public void Dispose() {
            GameObject.Destroy(this.gameObject);
        }

        private void Update() {
            if (node.RecursiveUpdate(spawner.tree, this)) {
                spawner.DrawLines();
            }
        }
    }
}
