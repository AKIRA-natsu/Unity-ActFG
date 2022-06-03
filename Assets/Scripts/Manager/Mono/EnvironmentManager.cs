using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace AKIRA.Manager {
    public class EnvironmentManager : MonoSingleton<EnvironmentManager> {
        public PostProcessVolume volume;
    }
}
