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

    /// <summary>
    /// 反射 fieldinfo转换类型
    /// </summary>
    /// <param name="convertibleValue"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object ConvertTo(this object convertibleValue, Type type) {
        if (!type.IsGenericType) {
            return Convert.ChangeType(convertibleValue, type);
        } else {
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Nullable<>)) {
                return Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(type));
            }
        }
        throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, type.FullName));
    }
}