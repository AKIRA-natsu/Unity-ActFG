namespace ActFG.Manager.Interface {
    public interface IObjectPoolAction {
        void Wake();
        void Recycle();
    }
}