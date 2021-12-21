using UnityEngine;
using UnityEditor;

namespace ActFG.Attribute.Drawer {
    [CustomPropertyDrawer(typeof(PrefabPath))]
    public class PrefabPathDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (SerializedPropertyType.String != property.propertyType) {
                EditorGUI.PropertyField(position, property);
                return;
            }

            // 根据路劲得到 GameObject
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(property.stringValue, typeof(GameObject));
            // Inspector 面板中右边的点 选择prefab
            var obj = (GameObject)EditorGUI.ObjectField(position, property.displayName, prefab, typeof(GameObject), false);
            // 得到 prefab路劲
            string newPath = AssetDatabase.GetAssetPath(obj);
            // 设置路劲
            property.stringValue = newPath;
            // Debug.Log(newPath);
        }
    }
}