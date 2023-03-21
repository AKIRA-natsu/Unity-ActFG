using System.Linq;
using UnityEngine;
using UnityEditor;
using AKIRA.Behaviour.AI;

[CustomEditor(typeof(FSMMachine))]
public class FSMMachineInspector : Editor {
    public override void OnInspectorGUI() {
        var machine = target as FSMMachine;

        if (!Application.isPlaying) {
            base.OnInspectorGUI();
        } else {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Config", machine.Config, typeof(AIDataConfig), allowSceneObjects : false);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }


        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        EditorGUILayout.EnumPopup("State", machine.curState);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        // 如果是沿着路走的，设置路标签
        if (machine.Config != null && !machine.Config.partolRandom) {
            EditorGUILayout.BeginHorizontal();
            var paths = WayPath.GetPaths();
            if (paths.Count == 0) {
                EditorGUILayout.LabelField("No WayPath Found");
            } else {
                var keys = paths.Keys.ToArray();
                int index = 0;
                for (; index < keys.Length;) {
                    if (keys[index].Equals(machine.wayTag)) {
                        break;
                    }
                    index++;
                }
                if (index >= keys.Length)
                    index = 0;

                index = EditorGUILayout.Popup("Partol Way Path", index, paths.Keys.ToArray());
                machine.wayTag = keys[index];
            }
            EditorGUILayout.EndHorizontal();
        }
        
    }
}