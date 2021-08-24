using UnityEngine;

public class Curve
{
    /// <summary>
    /// 極座標
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public Vector2 CircularCoordinates(float r, float angle)
    {
        float theta = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(theta) * r;
        float y = Mathf.Sin(theta) * r;
        return new Vector2(x, y);
    }

    /// <summary>
    /// インボリュート曲線
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public Vector2 InvoluteOfCircle(float r, float angle)
    {
        float theta = angle * Mathf.Deg2Rad;
        float x = r * (Mathf.Cos(theta) + theta * Mathf.Sin(theta));
        float y = r * (Mathf.Sin(theta) - theta * Mathf.Cos(theta));
        return new Vector2(x, y);
    }

    /// <summary>
    /// 2次ベジェ曲線
    /// </summary>
    /// <param name="a">移動開始位置</param>
    /// <param name="b">経由位置</param>
    /// <param name="c">目標位置</param>
    /// <param name="acceleration">加速度</param>
    /// <returns></returns>
    public Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float acceleration)
    {
        Vector2 p0 = DividingPoint(a, b, acceleration);
        Vector2 p1 = DividingPoint(b, c, acceleration);
        
        return DividingPoint(p0, p1, acceleration);
    }

    /// <summary>
    /// 内分点の座標を返す
    /// 線分abのaを0、bを1としたときの線分上のp割の位置を返す
    /// </summary>
    /// <param name="a">端点</param>
    /// <param name="b">端点</param>
    /// <param name="p">割合</param>
    /// <returns>内分点の座標(Vector2)</returns>
    public Vector2 DividingPoint(Vector2 a, Vector2 b, float p)
    {
        p = Mathf.Min(p, 1.0f);
        p = Mathf.Max(p, 0.0f);
        return a + ((b - a).normalized * (b - a).magnitude * p);
    }
}
