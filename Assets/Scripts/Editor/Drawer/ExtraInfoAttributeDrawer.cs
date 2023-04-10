using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ExtraInfoAttribute))]
public class ExtraInfoAttributeDrawer: PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var info = attribute as ExtraInfoAttribute;

        EditorGUI.BeginProperty(position, label, property);
        
        var height = base.GetPropertyHeight(property, label);
        var origin = new Rect(
            position.x,
            position.y,
            position.width,
            height
        );
        var infoRect = new Rect(
            position.x,
            position.y + height + 2.5f,
            position.width,
            // 如果是英文，存在高度不对的问题
            12f * GetMessageHeightCount(info.message) + 7f
        );
        EditorGUI.PropertyField(origin, property, label);
        EditorGUI.HelpBox(infoRect, info.message, (MessageType)info.messageType);
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var info = attribute as ExtraInfoAttribute;
        var height = base.GetPropertyHeight(property, label);
        // 如果是英文，存在高度不对的问题
        return height + 12f * GetMessageHeightCount(info.message) + 7f;
    }

    /// <summary>
    /// 信息高度数量
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private int GetMessageHeightCount(string message) {
        int heightCount = 1;
        for (int i = 0; i < message.Length; i++) {
            if (String.IsNullOrWhiteSpace(message[i].ToString()))
                heightCount++;
        }
        return heightCount;
    }
}