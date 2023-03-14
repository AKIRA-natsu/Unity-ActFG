using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace AKIRA.Manager {
    /// <summary>
    /// https://www.youtube.com/watch?v=SOK3Ias5Nk0
    /// </summary>
    [ExecuteAlways]
    public class BendingManager : MonoSingleton<BendingManager> {
        private const string BENDING_FEATURE = "ENABLE_BENDING";

        protected override void Awake() {
            base.Awake();
            // disable shader in edit mode
            if (Application.isPlaying)
                Shader.EnableKeyword(BENDING_FEATURE);
            else
                Shader.DisableKeyword(BENDING_FEATURE);
        }

        private void OnEnable() {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        protected override void OnDisable() {
            base.OnDisable();
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private static void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam) {
            cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99f) * cam.worldToCameraMatrix;
        }

        private static void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam) {
            cam.ResetCullingMatrix();
        }
    }
}
