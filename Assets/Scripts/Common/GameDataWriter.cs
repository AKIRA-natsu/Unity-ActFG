using System.IO;
using UnityEngine;

/// <summary>
/// 二进制文件写入
/// </summary>
public class GameDataWriter {
    private BinaryWriter writer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="version">数据版本号</param>
    public GameDataWriter(BinaryWriter writer, int version) {
        this.writer = writer;
        Write(version);
    }

    public void Write(float value) {
        writer.Write(value);
    }

    public void Write(int value) {
        writer.Write(value);
    }

    public void Write(bool value) {
        writer.Write(value);
    }

    public void Write(Vector3 value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public void Write(Quaternion value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

}

