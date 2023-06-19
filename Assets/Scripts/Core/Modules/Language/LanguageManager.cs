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

        protected LanguageManager() {
            var value = LanguageKey.GetInt(-1);
            language = value == -1 ? Application.systemLanguage : (SystemLanguage)value;
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
            EventManager.Instance.TriggerEvent(GameData.Event.OnLanguageChanged, language);
            $"切换语言 {language}".Log();
        }
    }
}