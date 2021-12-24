namespace ActFG.Message {
    /// <summary>
    /// 消息体
    /// </summary>
    public class Msg {
        public int Type { get; private set; }
        public object Data { get; private set; }

        public Msg(int type, object data) {
            this.Type = type;
            this.Data = data;
        }
    }
}