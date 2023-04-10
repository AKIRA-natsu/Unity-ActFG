using UnityEngine;
using UnityEditor;
using AKIRA.Manager;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {
    // 存储state
    private GameState state;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        EditorGUI.BeginChangeCheck();
        state = (GameState)EditorGUILayout.EnumPopup("Game State", GameManager.State);
        if (EditorGUI.EndChangeCheck()) {
            if (GameManager.IsStateEqual(state))
                return;
            $"GameManager Inspacetor Log: 编辑器下修改状态{state}".Colorful(System.Drawing.Color.RoyalBlue).Log();
            GameManager.Instance.Switch(state);
        }

        EditorGUILayout.HelpBox(GetMessage(), MessageType.Info);

        EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    /// help box message
    /// </summary>
    /// <returns></returns>
    private string GetMessage() {
        if (Application.isPlaying) {
            return "修改状态将调用GameManager.Switch方法";
        }  else {
            return "非游戏运行中无法修改";
        }
    }
}