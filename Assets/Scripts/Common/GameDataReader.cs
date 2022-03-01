using System.IO;
using UnityEngine;

namespace ActFG.Util.Tools {
    /// <summary>
    /// 二进制读取
    /// </summary>
    public class GameDataReader {
        private BinaryReader reader;
        // 数据版本号
        private int version; 

        public GameDataReader(BinaryReader reader) {
            this.reader = reader;
            this.version = ReadInt();
        }

        public float ReadFloat() {
            return reader.ReadSingle();
        }

        public int ReadInt() {
            return reader.ReadInt32();
        }

        public bool ReadBool() {
            return reader.ReadBoolean();
        }

        public Vector3 ReadVec3() {
            Vector3 value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            value.z = reader.ReadSingle();
            return value;
        }

        public Quaternion ReadQua() {
            Quaternion value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            value.z = reader.ReadSingle();
            value.w = reader.ReadSingle();
            return value;
        }
    }
}
