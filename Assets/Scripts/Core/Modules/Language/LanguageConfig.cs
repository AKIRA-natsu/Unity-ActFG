using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 语言字体配置
    /// </summary>
    [CreateAssetMenu(fileName = "LanguageConfig", menuName = "Framework/LanguageConfig", order = 0)]
    public class LanguageConfig : ScriptableObject {
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        private class FontConfig {
            public SystemLanguage language;
            public Font font;
        }

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private FontConfig[] fonts;

        /// <summary>
        /// 获得字体
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public Font GetFont(SystemLanguage language) {
            return fonts.SingleOrDefault(font => font.language == language).font;
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Editor方法，Editor下更新数组
        /// </summary>
        /// <param name="names"></param>
        /// <param name="chooses"></param>
        public void UpdateFontConfig(bool[] chooses) {
            // 筛选选中的
            List<SystemLanguage> result = new List<SystemLanguage>();
            var values = Enum.GetValues(typeof(SystemLanguage));
            for (int i = 0; i < chooses.Length; i++) {
                if (!chooses[i])
                    continue;
                result.Add((SystemLanguage)values.GetValue(i));
            }
            var lastFonts = fonts;
            fonts = new FontConfig[result.Count];
            for (int i = 0; i < fonts.Length; i++) {
                fonts[i] = new FontConfig();
                fonts[i].language = result[i];
                var existIndex = Array.FindIndex(lastFonts, 0, lastFonts.Length, font => font.language.Equals(result[i]));
                if (existIndex == -1)
                    continue;
                fonts[i].font = lastFonts[existIndex].font;
            }
        }
        #endif
    }
}