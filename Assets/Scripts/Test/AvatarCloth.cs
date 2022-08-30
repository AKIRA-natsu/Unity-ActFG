using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Avatar换装</para>
/// <para>https://blog.uwa4d.com/archives/avartar.html</para>
/// <para>https://github.com/zouchunyi/UnityAvater</para>
/// <para>TODO: 测试</para>
/// </summary>
public class AvatarCloth : MonoBehaviour {
    private const int COMBINE_TEXTURE_MAX = 512;
    private const string COMBINE_DIFFUSE_TEXTURE = "_MainTex";

    private List<Transform> transforms = new List<Transform>();
    private List<Material> materials = new List<Material>();
    private List<CombineInstance> combineInstances = new List<CombineInstance>();
    private List<Transform> bones = new List<Transform>();

    private List<Vector2[]> olduv = null;
    private Material newMaterial = null;
    private Texture2D newDiffuseTex = null;

    public Shader shader;

    /// <summary>
    /// 合并物体
    /// </summary>
    /// <param name="skeleton"></param>
    /// <param name="meshes"></param>
    /// <param name="combine"></param>
    public void CombineGameObject(GameObject skeleton, SkinnedMeshRenderer[] meshes, bool combine = false) {
        transforms.Clear();
        transforms.AddRange(skeleton.GetComponentsInChildren<Transform>(true));
        materials.Clear();
        combineInstances.Clear();
        bones.Clear();

        olduv = default;
        newMaterial = default;
        newDiffuseTex = default;
        
        for (int i = 0; i < meshes.Length; i++) {
            SkinnedMeshRenderer smr = meshes[i];
            materials.AddRange(smr.materials);
            // meshes
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++) {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add(ci);
            }
            // bones
            for (int j = 0; j < smr.bones.Length; j++) {
                int tBase = 0;
                for (tBase = 0; tBase < transforms.Count; tBase++) {
                    if (smr.bones[j].name.Equals(transforms[tBase])) {
                        bones.Add(transforms[tBase]);
                        break;
                    }
                }
            }
        }

        // merge materials
        if (combine) {
            newMaterial = new Material(shader);
            olduv = new List<Vector2[]>();
            // merge textures
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < materials.Count; i++)
                textures.Add(materials[i].mainTexture as Texture2D);
            
            newDiffuseTex = new Texture2D(COMBINE_TEXTURE_MAX, COMBINE_TEXTURE_MAX, TextureFormat.RGBA32, true);
            Rect[] uvs = newDiffuseTex.PackTextures(textures.ToArray(), 0);
            newMaterial.mainTexture = newDiffuseTex;

            // reset uv
            Vector2[] uva, uvb;
            for (int j = 0; j < combineInstances.Count; j++) {
                uva = (Vector2[])(combineInstances[j].mesh.uv);
                uvb = new Vector2[uva.Length];
                for (int k = 0; k < uva.Length; k++)
                    uvb[k] = new Vector2((uva[k].x * uvs[j].width) + uvs[j].x, (uva[k].y * uvs[j].height) + uvs[j].y);
                olduv.Add(combineInstances[j].mesh.uv);
                combineInstances[j].mesh.uv = uvb;
            }
        }

        // create a new skinned mesh renderer
        SkinnedMeshRenderer oldSkinned = skeleton.GetComponent<SkinnedMeshRenderer>();
        if (oldSkinned != null)
            GameObject.DestroyImmediate(oldSkinned);
        SkinnedMeshRenderer r = skeleton.AddComponent<SkinnedMeshRenderer>();
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), combine, false);
        r.bones = bones.ToArray();
        if (combine) {
            r.material = newMaterial;
            for (int i = 0; i < combineInstances.Count; i++)
                combineInstances[i].mesh.uv = olduv[i];
        } else {
            r.materials = materials.ToArray();
        }
    }
}
