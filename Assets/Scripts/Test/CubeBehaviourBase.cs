using AKIRA.Manager;

public abstract class CubeBehaviourBase : ReferenceBase, IUpdate<Cube> {
    public abstract void GameUpdate(Cube cube);
}