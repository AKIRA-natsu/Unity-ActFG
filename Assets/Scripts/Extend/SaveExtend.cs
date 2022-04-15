using UnityEngine;

public static class SaveExtend {
    private const string @int = "Int32";
    private const string @string = "String";
    private const string @float = "Single";

    /// <summary>
    /// PlayerPrefs save
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void Save(this string key, int data) {
        PlayerPrefs.SetInt(key, data);
    }

    /// <summary>
    /// PlayerPrefs save
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void Save(this string key, string data) {
        PlayerPrefs.SetString(key, data);
    }

    /// <summary>
    /// PlayerPrefs save
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void Save(this string key, float data) {
        PlayerPrefs.SetFloat(key, data);
    }

    /// <summary>
    /// default return 1
    /// </summary>
    /// <param name="key"></param>
    /// <param name="default">默认返回</param>
    /// <returns></returns>
    public static int GetInt(this string key, int @default = 1) {
        return PlayerPrefs.GetInt(key, @default);
    }

    /// <summary>
    /// default return null
    /// </summary>
    /// <param name="key"></param>
    /// <param name="default">默认返回</param>
    /// <returns></returns>
    public static string GetString(this string key, string @default = "") {
        return PlayerPrefs.GetString(key, @default);
    }

    /// <summary>
    /// default return 0
    /// </summary>
    /// <param name="key"></param>
    /// <param name="default">默认返回</param>
    /// <returns></returns>
    public static float GetFloat(this string key, float @default = 0) {
        return PlayerPrefs.GetFloat(key, @default);
    }

    /// <summary>
    /// 是否存在保存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool Exist(this string key) {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(this string key) {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll() {
        PlayerPrefs.DeleteAll();
    }
}