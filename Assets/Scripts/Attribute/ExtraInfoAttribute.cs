using System;
using UnityEngine;

/// <summary>
/// Info注释，用在class/struct上有问题
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ExtraInfoAttribute : PropertyAttribute {
    // 信息
    public string message { get; private set; }
    // 消息类型
    public int messageType { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="messageType">
    ///     <para>0 MessageType.None</para>
    ///     <para>1 MessageType.Info</para>
    ///     <para>2 MessageType.Warning</para>
    ///     <para>3 MessageType.Error</para>
    /// </param>
    public ExtraInfoAttribute(string message, int messageType = 1) {
        this.message = message;
        this.messageType = Mathf.Clamp(messageType, 0, 3);
    }
        
}
