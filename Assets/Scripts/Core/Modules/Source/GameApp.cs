using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA {
    /// <summary>
    /// 游戏开始入口
    /// </summary>
    public class GameApp : MonoBehaviour {
        private void Awake() {
            EventManager.Instance.AddEventListener(GameData.Event.OnAppSourceEnd, _ => this.gameObject.Destory());
            SourceSystem.Instance.Load();
        }

#if UNITY_EDITOR
        [ContextMenu("Test")]
        private void Test() {
            SourceSystem.Instance.Test();
        }
#endif
    }
}