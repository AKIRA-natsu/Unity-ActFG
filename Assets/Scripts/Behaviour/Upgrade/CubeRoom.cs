using UnityEngine;

public class CubeRoom : UpgradeBase
{
    public override int maxLevel => throw new System.NotImplementedException();

    protected override int CalculateCost()
    {
        return 10;
    }

    protected override float CalculateValue()
    {
        return 10;
    }
}