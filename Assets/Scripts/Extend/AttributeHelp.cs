using System;
using System.Linq;
using System.Reflection;

public static class AttributeHelp<T> where T : System.Attribute {
    /// <summary>
    /// 获得attribute标记的类
    /// </summary>
    /// <returns></returns>
    public static Type[] Handle() {
        Assembly asm = Assembly.GetAssembly(typeof(T));
        var types = asm.GetExportedTypes();
        return types.Where(o => {
            return IsMyAttribute(System.Attribute.GetCustomAttributes(o, false));
        }).ToArray();
    }

    /// <summary>
    /// 是否是需要的attribute
    /// </summary>
    /// <param name="attrs"></param>
    /// <returns></returns>
    private static bool IsMyAttribute(System.Attribute[] attrs) {
        foreach (System.Attribute a in attrs) {
            if (a is T) return true;
        }
        return false;
    }

    /// <summary>
    /// type 转 object
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object Type2Obj(Type type) {
        Assembly asm = Assembly.GetExecutingAssembly();
        return asm.CreateInstance(type.FullName);
    }

    /// <summary>
    /// 获取自定义attribute (Enum)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static T GetAttribute(Enum enumObj) {
        Type type = enumObj.GetType();
        System.Attribute attr = null;
        try {
            String enumName = Enum.GetName(type, enumObj);  //获取对应的枚举名
            FieldInfo field = type.GetField(enumName);  
            attr = field.GetCustomAttribute(typeof(T), false);
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
        
        return (T)attr;
    }
}