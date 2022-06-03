using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

/// <summary>
/// 特殊事件面板
/// </summary>
[CustomEditor(typeof(RegistSpecialAction), true)]
[CanEditMultipleObjects]
public class RegistSpecialActionInspector : Editor {
    /// <summary>
    /// 方法数组
    /// </summary>
    private MethodInfo[] methods;
    /// <summary>
    /// 参数数组
    /// </summary>
    private ParameterInfo[] parameters;

    /// <summary>
    /// 名称数组
    /// </summary>
    private string[] methodNames;
    /// <summary>
    /// 名称键值
    /// </summary>
    private int[] methodValues;

    /// <summary>
    /// 如果参数是object
    /// 改为从behaviour中获得参数value
    /// </summary>
    private MonoBehaviour behaviour;

    /// <summary>
    /// 是否开始debug
    /// </summary>
    private bool debug = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        RegistSpecialAction script = target as RegistSpecialAction;

        EditorGUILayout.BeginHorizontal();
        script.keyCode = (KeyCode)EditorGUILayout.EnumPopup("键盘按键", script.keyCode);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        script.target = (MonoBehaviour)EditorGUILayout.ObjectField("目标脚本", script.target, typeof(MonoBehaviour), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (script.target != null) {
            // 设置触发方法
            EditorGUILayout.BeginHorizontal();
            methods = script.target.GetType().GetMethods();
            methodNames = new string[methods.Length];
            methodValues = new int[methods.Length];
            for (int i = 0; i < methods.Length; i++) {
                methodNames[i] = methods[i].Name;
                methodValues[i] = i;
            }
            script.index = EditorGUILayout.IntPopup("使用方法", script.index, methodNames, methodValues);

            // 重建数组
            if (script.methodInfo != null && !script.methodInfo.Equals(methods[script.index]))
                script.parameterValues = null;

            script.methodInfo = methods[script.index];
            EditorGUILayout.EndHorizontal();
        } else {
            // 重置
            script.methodInfo = null;
            script.parameterValues = null;
            script.index = 0;
        }

        if (script.methodInfo != null) {
            // 获得参数
            parameters = script.methodInfo.GetParameters();
            // 防止每次都新建数组
            if (script.parameterValues == null)
                script.parameterValues = new object[parameters.Length];

            // 遍历参数
            for (int i = 0; i < parameters.Length; i++) {
                EditorGUILayout.BeginHorizontal();
                var type = parameters[i].ParameterType;
                
                // 判断参数类型
                if (type.Equals(typeof(int))) {
                    if (script.parameterValues[i] == null)
                        script.parameterValues[i] = (int)default;
                    script.parameterValues[i] = EditorGUILayout.IntField("Int", (int)script.parameterValues[i]);
                } else if (type.Equals(typeof(float))) {
                    if (script.parameterValues[i] == null)
                        script.parameterValues[i] = (float)default;
                    script.parameterValues[i] = EditorGUILayout.FloatField("Float", (float)script.parameterValues[i]);
                } else if (type.Equals(typeof(bool))) {
                    if (script.parameterValues[i] == null)
                        script.parameterValues[i] = (bool)default;
                    script.parameterValues[i] = EditorGUILayout.Toggle("Boolean", (bool)script.parameterValues[i]);
                } else if (type.Equals(typeof(double))) {
                    if (script.parameterValues[i] == null)
                        script.parameterValues[i] = (double)default;
                    script.parameterValues[i] = EditorGUILayout.DoubleField("Double", (double)script.parameterValues[i]);
                } else if (type.Equals(typeof(string))) {
                    script.parameterValues[i] = EditorGUILayout.TextField("string", (string)script.parameterValues[i]);
                } else if (type.BaseType.Equals(typeof(Enum))) {
                    // Enum 默认
                    if (script.parameterValues[i] == null)
                        script.parameterValues[i] = type.GetEnumValues().GetValue(0);
                    script.parameterValues[i] = EditorGUILayout.EnumPopup($"{type}", (Enum)script.parameterValues[i]);
                }
                //  else if (IsList(type)) {
                //     $"列表".Log();
                //     script.parameterValues[i] = new List<object>();
                //     // script.parameterValues[i] = EditorGUILayout.MaskField("")
                // }
                else if (type.IsSubclassOf(typeof(Delegate))) {
                    // TODO: 待补充
                    EditorGUILayout.HelpBox("暂不支持Delegate", MessageType.Warning);
                } else {
                    // EditorGUILayout.HelpBox("参数\n选择脚本第一个类型相同的变量作为参数", MessageType.Info);
                    behaviour = (MonoBehaviour)EditorGUILayout.ObjectField($"{type}", behaviour, typeof(MonoBehaviour), true);
                    if (behaviour != null) {
                        var fields = behaviour.GetType().GetFields();
                        foreach (var field in fields) {
                            if (field.FieldType.Equals(type)) {
                                script.parameterValues[i] = field.GetValue(behaviour);
                                break;
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        debug = EditorGUILayout.Toggle("Debug", debug);
        EditorGUILayout.EndHorizontal();

        if (debug) {
            EditorGUILayout.HelpBox("已勾选Debug\n鼠标焦点在Inspector上且方法不为空时自动运行方法", MessageType.Info);
            if (script.methodInfo != null)
                script.methodInfo.Invoke(script.target, script.parameterValues);
        }

        if (GUI.changed) {
            EditorUtility.SetDirty(script);
        }

        // Editor OnInspectorGUI方法中
        // 强制每帧刷新一次
        // 来源：https://cxywk.com/gamedev/q/QZdC6snb
        // if (EditorApplication.isPlaying)
        //     Repaint();
    }

    /// <summary>
    /// 是否是列表类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool IsList(Type type) {
        if (typeof(IList).IsAssignableFrom(type))
            return true;
        
        foreach (var it in type.GetInterfaces()) {
            if (it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition())
                return true;
        }
        
        return false;
    }
}