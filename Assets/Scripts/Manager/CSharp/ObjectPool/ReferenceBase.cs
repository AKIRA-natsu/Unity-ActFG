namespace AKIRA.Manager {
    public class ReferenceBase : IPool {
        public bool active = false;

        public virtual void Wake() {
            active = true;
        }

        public virtual void Recycle() {
            active = false;
        }
    }
}