using UnityEditor;

/// <summary>
/// EditorPrefs保存的封装
/// </summary>
internal static class EditorPrefsHelp {
    public static void Set(string key, int @default) {
        EditorPrefs.SetInt(key, @default);
    }

    public static void Set(string key, float @default) {
        EditorPrefs.SetFloat(key, @default);
    }

    public static void Set(string key, bool @default) {
        EditorPrefs.SetBool(key, @default);
    }

    public static void Set(string key, string @default) {
        EditorPrefs.SetString(key, @default);
    }

    public static int GetInt(string key, int @default = 0) {
        return EditorPrefs.GetInt(key, @default);
    }

    public static float GetFloat(string key, float @default = 0f) {
        return EditorPrefs.GetFloat(key, @default);
    }

    public static bool GetBool(string key, bool @default = true) {
        return EditorPrefs.GetBool(key, @default);
    }

    public static string GetString(string key, string @default = "") {
        return EditorPrefs.GetString(key, @default);
    }

    public static void Delete(string key) {
        EditorPrefs.DeleteKey(key);
    }

    public static void Delete() {
        EditorPrefs.DeleteAll();
    }
}