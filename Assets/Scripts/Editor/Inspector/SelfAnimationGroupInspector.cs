using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// <para>ReorderableList：https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/</para>
/// </summary>
[CustomEditor(typeof(SelfAnimationGroup))]
public class SelfAnimationGroupInspector : Editor {
    private ReorderableList animations;
    private Editor selectedComponentInspectorEditor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ShowSelfAnimWindow(target as SelfAnimationGroup);
    }

    private void OnEnable() {
        animations = new ReorderableList(serializedObject, serializedObject.FindProperty("datas"));
        animations.drawElementCallback = DrawAnimationElement;
        animations.drawHeaderCallback = DrawAnimationHeader;
        animations.onAddDropdownCallback = AddSelfAnimation;
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
        SelfAnim animation = element.objectReferenceValue as SelfAnim;
        animation.Enable = EditorGUI.Toggle(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), animation.Enable);
        EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), element, GUIContent.none);

        if (index == animations.index) {

            if (animation == null)
                return;
            
            Editor.CreateCachedEditor(animation, null, ref selectedComponentInspectorEditor);
            var foldoutName = $"{ObjectNames.NicifyVariableName (animation.GetType().Name)} Properties";
            using (var foldout = new EditorGUILayoutx.FoldoutContainerScope(animations.serializedProperty, foldoutName, "Box", EditorStyles.foldout)) {
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
    /// 下拉按钮添加Animation
    /// </summary>
    /// <param name="buttonRect"></param>
    /// <param name="list"></param>
    private void AddSelfAnimation(Rect buttonRect, ReorderableList list) {
        "点击添加".Log();
    }


    /// <summary>
    /// 动画面板
    /// </summary>
    /// <param name="animation"></param>
    private void ShowSelfAnimWindow(SelfAnimationGroup animation) {
        if (animations.Equals(null)) 
            return;

        EditorGUILayout.BeginVertical("framebox");
        serializedObject.Update();
        animations.DoLayoutList();

        var newAnimations = EditorGUILayoutx.DragAndDropArea<SelfAnim>();
        if (newAnimations != null && newAnimations.Count > 0) {
            Undo.RecordObjects(targets, "Added Animations");
            foreach (var t in targets) {
                var datas = ((SelfAnimationGroup)t).Datas;
                foreach (var anima in newAnimations) {
                    datas.Add(anima);
                }
            }
			serializedObject.SetIsDifferentCacheDirty();
        }
        serializedObject.ApplyModifiedProperties();

        // draw buttons unitility
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Collect Child Animations")) {
            var group = target as SelfAnimationGroup;
            var datas = group.Datas;
            var childDatas = group.GetComponentsInChildren<SelfAnim>();
            foreach (var data in childDatas) {
                if (datas.Contains(data))
                    continue;
                else
                    datas.Add(data);
            }            
        }
        if (GUILayout.Button("Destory Choosen Aniamtion")) {
            GameObject.DestroyImmediate(animations.serializedProperty.GetArrayElementAtIndex(animations.index).objectReferenceValue);
            (target as SelfAnimationGroup).Datas.RemoveAt(animations.index);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}