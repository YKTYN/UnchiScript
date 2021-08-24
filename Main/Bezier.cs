
public class Bezier : BarrageBase
{
    public override void BulletShape(int barrageIndex, int bulletIndex, bool enemy)
    {
        GetBarrageList()[barrageIndex][bulletIndex].Bezier();
    }
}
