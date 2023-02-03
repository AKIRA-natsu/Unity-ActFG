#if UNITY_EDITOR
using UnityEngine;

public class TestUniTime : MonoBehaviour {
    public bool repeat = true;

    private void Start() {
        var delay = this.UniDelay(DelayLog, 3f).UniCompleted(EndLog, 1f).UniCompleted(EndLog, 2f);
        "Start".Log();
    }

    [ContextMenu("Editor Repeat")]
    public void EditorRepeat() {
        var delay = this.UniRepeat(Repeat, 7, 1f).UniCompleted(EndLog, 3f);
    }

    [ContextMenu("Editor Repeat2")]
    public void EditorRepeat2() {
        var delay = this.UniRepeat("循环".Log, () => repeat, 1f).UniCompleted(EndLog, 3f);
    }

    public void DelayLog() {
        "延迟3s".Log();
    }

    public void EndLog() {
        "结束".Log();
    }

    public void Repeat(int time) {
        time.Log();
    }
}
#endif