public class Fan : BarrageBase
{
    public override void BulletShape(int barrageIndex, int bulletIndex, bool enemy)
    {
        float angle = -(50 / GetBulletMax()) * (bulletIndex + 7);
        GetBarrageList()[barrageIndex][bulletIndex].CircularCoordinates(angle);
    }
}
