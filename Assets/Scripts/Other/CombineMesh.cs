using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合并mesh
/// order -1
/// </summary>
[DefaultExecutionOrder(-1)]
public class CombineMesh : MonoBehaviour {
    // 子节点所有mesh
    private MeshRenderer[] renderers;
    
    // 相同材质球记录表
    private Dictionary<Material, List<MeshRenderer>> materialMap = new Dictionary<Material, List<MeshRenderer>>();

    // 是否合成添加MeshCollider
    public bool addCollider = false;

    private void Awake() {
        renderers = this.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers) {
            var material = renderer.sharedMaterial;
            if (materialMap.ContainsKey(material)) {
                materialMap[material].Add(renderer);
            } else {
                materialMap.Add(material, new List<MeshRenderer>() { renderer });
            }
        }

        foreach (var kvp in materialMap)
            MergeMesh(kvp.Key, kvp.Value);
    }

    /// <summary>
    /// 合并网格
    /// </summary>
    private void MergeMesh(Material material, List<MeshRenderer> renderers) {
        if (renderers.Count == 1)
            return;

        var meshFilters = new MeshFilter[renderers.Count];
        for (int i = 0; i < renderers.Count; i++)
            meshFilters[i] = renderers[i].GetComponent<MeshFilter>();

        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++) {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(combineInstances);

        // 创建新节点在父节点下
        GameObject go = new GameObject(material.name, typeof(MeshFilter), typeof(MeshRenderer));
        go.SetParent(this.gameObject);

        go.GetComponent<MeshFilter>().sharedMesh = newMesh;

        // 隐藏所有子节点
        for (int i = 0; i < renderers.Count; i++)
            renderers[i].gameObject.SetActive(false);
            
        // 添加材质
        go.gameObject.GetComponent<MeshRenderer>().material = material;

        if (addCollider) {
            // meshCollider
            go.AddComponent<MeshCollider>().sharedMesh = newMesh;
        }
    }
}