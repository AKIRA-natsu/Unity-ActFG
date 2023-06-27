using AKIRA.Data;
using AKIRA.Manager;
using UnityEngine;

namespace AKIRA {
    /// <summary>
    /// 游戏开始入口
    /// </summary>
    public class GameApp : MonoBehaviour {
        private void Awake() {
            // EventManager.Instance.AddEventListener(GameData.Event.OnAppSourceEnd, _ => this.gameObject.Destory());
            SourceSystem.Instance.Load();
        }

        private void OnApplicationFocus(bool focusStatus) {
            EventManager.Instance.TriggerEvent(GameData.Event.OnAppFocus, focusStatus);
        }
    }
}