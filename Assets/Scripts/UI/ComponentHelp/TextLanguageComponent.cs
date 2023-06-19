using System.IO;
using System;
using System.Collections.Generic;
using AKIRA.Data;
using AKIRA.Manager;
using TMPro;
using UnityEngine;
using System.Linq;

namespace AKIRA.UIFramework {
    /// <summary>
    /// 语言文本表现
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLanguageComponent : MonoBehaviour {
        // xml名称
        public string xmlName;
        // 文本id
        public string textID;

        // 文本
        private TextMeshProUGUI text;
        // 文本列表
        private List<LanguageText> texts = new List<LanguageText>();

        private void Awake() {
            EventManager.Instance.AddEventListener(GameData.Event.OnLanguageChanged, UpdateText);
            text = this.GetComponent<TextMeshProUGUI>();
            texts = XML.DeSerialize<List<LanguageText>>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, $"{xmlName}.xml")));

            UpdateText(LanguageManager.Instance.Language);
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="data"></param>
        private void UpdateText(object data) {
            if (String.IsNullOrEmpty(textID))
                return;
            text.text = 
                texts.SingleOrDefault(text => text.textID.Equals(textID))?.GetLanguageTextValue((int)data);
        }
    }
}