using UnityEngine;
public class Straight : BarrageBase
{
    public override void BulletShape(int barrageIndex, int bulletIndex, bool enemy)
    {
        GetBarrageList()[barrageIndex][bulletIndex].Straight(enemy);
    }
}
