using UnityEngine;

[CreateAssetMenu(fileName = "UIRule", menuName = "UIFramework/UIRule", order = 0)]
public class UIRule : ScriptableObject {
    public UIControlRule[] rules = {};

    /// <summary>
    /// 获得组件名称（不判断大小写）
    /// </summary>
    /// <param name="name1">UI组件命名名称</param>
    /// <param name="name2">UI规定组件名称</param>
    /// <returns>UI组件</returns>
    public void TryGetControlName(string name1, out string name2) {
        foreach (var rule in rules) {
            foreach (var n in rule.ruleNames) {
                if (name1.Contains(n)) {
                    name2 = rule.controlName;
                    return;
                }
            }
        }
        $"rules dont contain {name1}".Error();
        name2 = null;
    }
}