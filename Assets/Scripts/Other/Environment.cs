using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using AKIRA.Manager;
using AKIRA.Data;

namespace AKIRA.Behaviour.Prepare {
    /// <summary>
    /// 场景环境
    /// </summary>
    [Source("Source/Base/[Environment]", GameData.Source.Base, 1)]
    public class Environment : MonoBehaviour {
        // 后处理
        private Volume volume;

        // 是否开启后处理
        [SerializeField]
        private bool enableVolume;

        private void Awake() {
            volume = this.GetComponentInChildren<Volume>();
        }

        private void Start() {
            volume?.gameObject.SetActive(enableVolume);
        }

        private void OnValidate() {
            if (!Application.isPlaying)
                return;
            
            volume?.gameObject.SetActive(enableVolume);
        }
    }
}