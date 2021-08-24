using System.Collections.Generic;
using UnityEngine;

public abstract class BarrageBase : MonoBehaviour
{
    //　一度に出す弾の数
    private int bulletMax;
    //　弾の元
    private BulletUnchi bullet;
    //　移動速度
    private float[] speed;

    //　保持数
    private int barrageMax;
    //　リスト
    private List<List<BulletUnchi>> barrageList = new List<List<BulletUnchi>>();
    //　インデックス
    private int index = -1;
    //　n秒ごとの処理のための時間
    private float currentTime;

    /// <summary>
    /// 弾幕の形を作る
    /// </summary>
    public abstract void BulletShape(int barrageIndex, int bulletIndex, bool enemy);

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Color color, int bulletMax, float[] speed, int barrageMax)
    {
        this.bulletMax = bulletMax;
        this.speed = speed;
        this.barrageMax = barrageMax;

        for (int i = 0; i < this.barrageMax; i++)
        {
            barrageList.Add(CreateBulletList(color));
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dt"></param>
    public void UpdateBulletList(Vector2 targetPos, float dt, float interval,  bool enemy)
    {
        Shot(targetPos, dt, interval);

        for (int i = 0; i < barrageList.Count; i++)
        {
            for (int j = 0; j < bulletMax; j++)
            {
                // 消滅
                if (
                    barrageList[i][j].OffScreen()
                    || barrageList[i][j].BezierEnd()
                    || barrageList[i][j].GetIsHitCollider())
                {
                    barrageList[i][j].GoBack();
                }

                // 形を決めて動かす
                BulletShape(i, j, enemy);
                barrageList[i][j].BulletUpdate(dt);
            }
        }
    }

    /// <summary>
    /// 打ち出す
    /// </summary>
    private void Shot(Vector2 targetPos, float dt, float interval)
    {
        currentTime += dt;
        if(currentTime >= interval)
        {
            index++;
            if (index >= barrageMax - 1)
            {
                index = 0;
            }
            for (int i = 0; i < bulletMax; i++)
            {
                barrageList[index][i].Move(transform.position, targetPos);
            }

            currentTime = 0.0f;
        }
    }

    /// <summary>
    /// 弾を作る
    /// </summary>
    /// <param name="color">色</param>
    /// <returns>弾リスト</returns>
    private List<BulletUnchi> CreateBulletList(Color color)
    {
        List<BulletUnchi> list = new List<BulletUnchi>();
        for (int i = 0; i < bulletMax; i++)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefubs/unti"), transform);
            this.bullet = bullet.GetComponent<BulletUnchi>();
            this.bullet.Init(Vector2.one * 0.5f, Vector2.zero, speed[i], color);
            list.Add(this.bullet);
        }
        return list;
    }

    protected List<List<BulletUnchi>> GetBarrageList()
    {
        return barrageList;
    }

    protected int GetBulletMax()
    {
        return bulletMax;
    }
}
