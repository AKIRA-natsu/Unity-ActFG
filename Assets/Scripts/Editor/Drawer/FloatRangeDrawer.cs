using UnityEngine;
using UnityEditor;//编辑器依赖

//告诉Unity要创建FloatRange类型的CustomPropertyDrawer
[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    //覆盖Unity本身OnGUI函数，它在每次要显示FloatRange时被调用。
    //三个参数 position 定义绘制的区域 property 定义浮点数的范围值
    //label使用的默认标签
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var minProp = property.FindPropertyRelative("min");
        var maxProp = property.FindPropertyRelative("max");

        if (Application.isPlaying) {
            EditorGUI.BeginProperty(position, label, property);

            //显示字段标签，返回标签占用后剩余的区域
            position = EditorGUI.PrefixLabel(position, label);
            var valueProp = property.FindPropertyRelative("value");
            valueProp.floatValue =
                EditorGUI.Slider(position, valueProp.floatValue, minProp.floatValue, maxProp.floatValue);

            EditorGUI.EndProperty();
        } else {
            EditorGUI.BeginProperty(position, label, property);

            //显示字段标签，返回标签占用后剩余的区域
            position = EditorGUI.PrefixLabel(position, label);
            //区域宽度设为一半
            position.width = position.width / 2f - 5f;
            //调整标签Label宽度为 当前宽度的一半
            EditorGUIUtility.labelWidth = position.width / 2f - 20f;
            EditorGUI.PropertyField(position, minProp);
            //max属性右移
            position.x += position.width + 10f;

            EditorGUI.PropertyField(position, maxProp);

            EditorGUI.EndProperty();
        }
    }
}