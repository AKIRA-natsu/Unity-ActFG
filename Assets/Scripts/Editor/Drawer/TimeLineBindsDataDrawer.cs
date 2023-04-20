using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;

[CustomPropertyDrawer(typeof(TimeLineBindsData))]
public class TimeLineBindsDataDrawer : PropertyDrawer
{
    private const float LINE_HEIGHT = 18f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 获取tag和timeline字段的SerializedProperty对象
        SerializedProperty tagProp = property.FindPropertyRelative("tag");
        SerializedProperty timelineProp = property.FindPropertyRelative("timeline");

        // 绘制tag字段的面板
        EditorGUI.BeginProperty(position, label, tagProp);
        Rect tagRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(tagRect, tagProp);
        EditorGUI.EndProperty();

        // 绘制timeline字段的面板
        EditorGUI.BeginProperty(position, label, timelineProp);
        Rect timelineRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
            position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(timelineRect, timelineProp);

        // 获取timeline字段的引用
        TimelineAsset timeline = timelineProp.objectReferenceValue as TimelineAsset;

        if (timeline != null)
        {
            // 获取bindMap字段的引用
            Dictionary<string, Object> bindMap = new Dictionary<string, Object>();

            // 遍历TimelineAsset的所有outputs
            foreach (var output in timeline.outputs)
            {
                // 获取streamingName和sourceObject
                string streamingName = output.streamName;
                Object sourceObject = output.sourceObject;

                // 将streamingName和sourceObject添加到字典中
                bindMap.Add(streamingName, sourceObject);
            }

            // 绘制bindMap字段
            Rect bindMapRect = new Rect(position.x, position.y + LINE_HEIGHT * 2, position.width, LINE_HEIGHT * 2);
            EditorGUI.LabelField(bindMapRect, "Bind Map (read-only)");

            // 禁用编辑
            GUI.enabled = false;

            // 绘制字典
            float startY = bindMapRect.y + LINE_HEIGHT;
            foreach (var kvp in bindMap)
            {
                Rect keyRect = new Rect(bindMapRect.x, startY, bindMapRect.width / 2f - 2f, LINE_HEIGHT);
                EditorGUI.LabelField(keyRect, kvp.Key);

                Rect valueRect = new Rect(bindMapRect.x + bindMapRect.width / 2f + 2f, startY, bindMapRect.width / 2f - 2f, LINE_HEIGHT);
                EditorGUI.ObjectField(valueRect, kvp.Value, typeof(Object), true);

                startY += LINE_HEIGHT;
            }

            // 恢复编辑
            GUI.enabled = true;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = LINE_HEIGHT * 2;

        // 获取timeline字段的引用
        TimelineAsset timeline = (TimelineAsset)property.FindPropertyRelative("timeline").objectReferenceValue;

        if (timeline != null)
        {
            // 获取bindMap字段的引用
            Dictionary<string, Object> bindMap = new Dictionary<string, Object>();

            // 遍历TimelineAsset的所有outputs
            foreach (var output in timeline.outputs)
            {
                // 获取streamingName和sourceObject
                string streamingName = output.streamName;
                Object sourceObject = output.sourceObject;

                // 将streamingName和sourceObject添加到字典中
                bindMap.Add(streamingName, sourceObject);
            }

            // 计算字典的高度
            height += LINE_HEIGHT * 2 + LINE_HEIGHT * bindMap.Count;
        }

        return height;
    }
}