using System;
using UnityEngine;

namespace ActFG.Util.Save {
    public static class SaveExtend {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        public static void Save<T>(this T data, string key) {
            if (typeof(T) == typeof(int))
                PlayerPrefs.SetInt(key, (int)(object)data);
            else if (typeof(T) == typeof(string))
                PlayerPrefs.SetString(key, (string)(object)data);
            else if (typeof(T) == typeof(float))
                PlayerPrefs.SetFloat(key, (float)(object)data);
            else
                Debug.LogError("type not right");
        }

        /// <summary>
        /// 1: int, 2: string, 3: float
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(this string key, int type) {
            switch (type) {
                case 1:
                    return (T)(object)PlayerPrefs.GetInt(key, 1);
                case 2:
                    return (T)(object)PlayerPrefs.GetString(key, "");
                case 3:
                    return (T)(object)PlayerPrefs.GetFloat(key);
                default:
                    throw new Exception("type not right");
            }
            throw new Exception("key not found");
        }

        public static void DeleteKey(this string key) {
            PlayerPrefs.DeleteKey(key);
        }

        public static void DeleteAll() {
            PlayerPrefs.DeleteAll();
        }
    }
}