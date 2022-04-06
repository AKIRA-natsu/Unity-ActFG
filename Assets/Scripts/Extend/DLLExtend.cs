using System;
using System.Reflection;

public static class DLLExtend {
    // DLL 名称
    public static string DLLName = "Assembly-CSharp";

    /// <summary>
    /// 不同程序集查找类型
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static Type GetConfigTypeByAssembley(string className) {
        var types = Assembly.Load(DLLName).GetTypes();
        foreach (var type in types) {
            if (type.Name.Contains(className)) {
                return type;
            }
        }
        return null;
    }
}