using System;
using System.Reflection;

public static class ReflectionHelp {
    /// <summary>
    /// 根据类型创建实例
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateInstance<T>(this Type type) {
        Assembly asm = Assembly.GetExecutingAssembly();
        return (T)asm.CreateInstance(type.FullName);
    }
}