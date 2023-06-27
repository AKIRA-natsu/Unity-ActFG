using System.Collections.Generic;
using AKIRA.Data;
using UnityEngine;

namespace AKIRA.Manager {
    /// <summary>
    /// 文本语言管理器
    /// </summary>
    public class LanguageManager : Singleton<LanguageManager> {
        [ReadOnly]
        [SerializeField]
        private SystemLanguage language;
        /// <summary>
        /// 系统语言
        /// </summary>
        public SystemLanguage Language => language;

        /// <summary>
        /// 保存键值
        /// </summary>
        private const string LanguageKey = "LanguageKey";
        /// <summary>
        /// 字体配置文件
        /// </summary>
        private LanguageConfig config;

        protected LanguageManager() {
            var value = LanguageKey.GetInt(-1);
            language = value == -1 ? SystemLanguage.English : (SystemLanguage)value;
            config = GameData.Path.LanguageConfig.Load<LanguageConfig>();
            $"当前系统读取的语言 {Language}".Log();
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="language"></param>
        public void SwtichLanguage(SystemLanguage language) {
            if (this.language == language)
                return;
            this.language = language;
            LanguageKey.Save((int)language);
            EventManager.Instance.TriggerEvent(GameData.Event.OnLanguageChanged);
            $"切换语言 {language}".Log();
        }

        /// <summary>
        /// <para>获得字体</para>
        /// <para>TextMeshPro可以通过CreateFontAsset创建需要的FontAsset</para>
        /// </summary>
        /// <returns></returns>
        public Font GetFont() {
            return config.GetFont(Language);
        }
    }
}