using UnityEngine;
        
/// <summary>
/// <para>文本</para>
/// <para>Create&Update By LanguageTextWindow</para>
/// </summary>
[System.Serializable]
public class LanguageText {
    public string textID;
            
    public string English;
            
    public string German;
            
    public string Japanese;
            
    public string ChineseSimplified;
            
    public string ChineseTraditional;
            
    /// <summary>
    /// 通过language返回对应的语言文本
    /// </summary>
    public string GetLanguageTextValue(SystemLanguage language) {
        switch (language) {
            case SystemLanguage.English:
            return English;
            
            case SystemLanguage.German:
            return German;
            
            case SystemLanguage.Japanese:
            return Japanese;
            
            case SystemLanguage.ChineseSimplified:
            return ChineseSimplified;
            
            case SystemLanguage.ChineseTraditional:
            return ChineseTraditional;
            
        }
        return default;
    }
}