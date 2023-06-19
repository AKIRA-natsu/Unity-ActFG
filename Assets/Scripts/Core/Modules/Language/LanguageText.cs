/// <summary>
/// <para>文本</para>
/// <para>Create&Update By LanguageTextWindow</para>
/// </summary>
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
    public string GetLanguageTextValue(int language) {
        switch (language) {
            case 10:
            return English;
            
            case 15:
            return German;
            
            case 23:
            return Japanese;
            
            case 41:
            return ChineseSimplified;
            
            case 42:
            return ChineseTraditional;
            
        }
        return default;
    }
}