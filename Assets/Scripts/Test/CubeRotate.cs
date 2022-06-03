using AKIRA.Manager;

public class CubeRotate : CubeBehaviourBase
{
    public override void GameUpdate(Cube cube)
    {
        $"{cube.name} need rotate".Log();
    }
}