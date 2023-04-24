using UnityEngine;
using UnityEditor;
using AKIRA.Behaviour.AI;

[CustomEditor(typeof(CharacterNavigationController), true)]
public class CharacterNavigationControllerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        var controller = target as CharacterNavigationController;

        EditorGUILayout.BeginHorizontal();
        if (!Application.isPlaying) {
            if (GUILayout.Button("Add FSM")) {
                controller.AddFSM();
            }

            if (GUILayout.Button("Remove FSM")) {
                controller.RemoveFSM();
            }
        } else {
            if (GUILayout.Button("Wake")) {
                controller.Wake();
            }

            if (GUILayout.Button("Recycle")) {
                controller.Recycle();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}