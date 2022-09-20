 using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float min, max;
    
    public float RangeValueInRange
    {
        get
        {
            return Random.Range(min, max);
        }
    }
}