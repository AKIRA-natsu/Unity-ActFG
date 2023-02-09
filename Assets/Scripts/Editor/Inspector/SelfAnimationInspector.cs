using Beans.Unity.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// <para>ReorderableList：https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/</para>
/// </summary>
[CustomEditor(typeof(SelfAnimation))]
public class SelfAnimationInspector : Editor {
    private ReorderableList animations;
    private Editor selectedComponentInspectorEditor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ShowSelfAnimWindow(target as SelfAnimation);
    }

    private void OnEnable() {
        animations = new ReorderableList(serializedObject, serializedObject.FindProperty("datas"));
        animations.drawElementCallback = DrawAnimationElement;
        animations.drawHeaderCallback = DrawAnimationHeader;
    }

    private void OnDisable() {
        animations = null;
    }

    /// <summary>
    /// 元素绘制
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isActive"></param>
    /// <param name="isFocused"></param>
    private void DrawAnimationElement(Rect rect, int index, bool isActive, bool isFocused) {
        rect.y += 2;
        var element = animations.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);

        if (index == animations.index) {
            var animation = element.objectReferenceValue;

            if (animation == null)
                return;
            
            Editor.CreateCachedEditor(animation, null, ref selectedComponentInspectorEditor);
            var foldoutName = $"{ObjectNames.NicifyVariableName (animation.GetType ().Name)} Properties";
            using (var foldout = new EditorGUILayoutx.FoldoutContainerScope (animations.serializedProperty, foldoutName, "Box", EditorStyles.foldout)) {
                if (foldout.isOpen) {
                    selectedComponentInspectorEditor.OnInspectorGUI ();
                }
            }
        }
    }

    /// <summary>
    /// 列表名称
    /// </summary>
    /// <param name="rect"></param>
    private void DrawAnimationHeader(Rect rect) {
        EditorGUI.LabelField(rect, "Animations");
    }

    /// <summary>
    /// 动画面板
    /// </summary>
    /// <param name="animation"></param>
    private void ShowSelfAnimWindow(SelfAnimation animation) {
        if (animations.Equals(null)) 
            return;

        EditorGUILayout.BeginVertical("framebox");
        serializedObject.Update();
        animations.DoLayoutList();

        var newAnimations = EditorGUILayoutx.DragAndDropArea<SelfAnim>();
        if (newAnimations != null && newAnimations.Count > 0) {
            Undo.RecordObjects(targets, "Added Animations");
            foreach (var t in targets) {
                var datas = ((SelfAnimation)t).Datas;
                foreach (var anima in newAnimations) {
                    datas.Add(anima);
                }
            }
			serializedObject.SetIsDifferentCacheDirty();
        }
        serializedObject.ApplyModifiedProperties();

        // draw buttons unitility
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Clear animations")) {

        }
        if (GUILayout.Button("Clear animations22")) {
            
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}