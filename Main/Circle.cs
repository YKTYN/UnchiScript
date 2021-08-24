
public class Circle : BarrageBase
{
    public override void BulletShape(int barrageIndex, int bulletIndex, bool enemy)
    {
        float angle = (360 / GetBulletMax()) * (bulletIndex);
        GetBarrageList()[barrageIndex][bulletIndex].CircularCoordinates(angle+5);
    }
}
