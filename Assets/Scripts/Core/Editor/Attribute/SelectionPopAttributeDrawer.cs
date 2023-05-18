using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SelectionPopAttribute))]
public class SelectionPopAttributeDrawer: PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var selection = attribute as SelectionPopAttribute;
        var type = selection.type;

        var strValue = property.stringValue;
        var values = type.GetFields();
        var index = 0;
        List<GUIContent> names = new List<GUIContent>();
        for (int i = 0; i < values.Length; i++) {
            var name = values[i].GetRawConstantValue().ToString();
            if (String.IsNullOrEmpty(name))
                continue;
            if (strValue.Equals(name))
                index = i;
            names.Add(new GUIContent(name));
        }

        index = EditorGUI.Popup(position, label, index, names.ToArray());
        property.stringValue = names[index].text;
    }
}