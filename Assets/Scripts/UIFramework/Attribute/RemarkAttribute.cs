using System;

/// <summary>
/// 标记，解释
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public class RemarkAttribute : Attribute {
    public string Remark { get; private set; }

    public RemarkAttribute(string remark) => 
        this.Remark = remark;
}
