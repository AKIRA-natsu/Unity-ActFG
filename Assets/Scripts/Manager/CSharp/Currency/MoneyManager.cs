using AKIRA.Manager;

/// <summary>
/// 金钱
/// </summary>
public class MoneyManager : CurrencyManager<MoneyManager> {
    protected override string Key => "Money";

    protected MoneyManager() {
        currency = Key.GetInt();
    }
}