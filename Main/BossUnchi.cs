using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUnchi : MonoBehaviour
{
    //　HPの最大値
    private const float maxHp = 2000.0f;

    //　移動速度
    private const float moveSpeed = 0.3f;

    //　横方向の移動限界地
    private const float xMoveMax = 8.0f;

    //　ターゲット
    [SerializeField]
    private GameObject targetObj = default;
    //　hpバー
    [SerializeField]
    private Slider hpBar = default;
    //　現在のHP残量
    private float hp = maxHp;
    // 弾の親オブジェクト
    private GameObject bulletParent = null;

    //　扇弾
    private Fan fanBarrage = null;
    private float fanInterval = 0.3f;
    //　2次ベジェ曲線の弾
    private Bezier bezierBarrage = null;
    private float bezierInterval = 0.5f;
    //　円形
    private Circle circleBarrage = null;
    private float circleInterval = 0.6f;

    //　敵の弾リスト
    private List<BulletUnchi> bulletList = null;

    //　待機時間
    private float moveInterval = 0.0f;

    //　SE用のオーディオソース
    private AudioSource seSource;
    //　爆発エフェクト
    private ParticleSystem particle;

    //　難易度
    private Fixed.LEVEL level;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        level = (Fixed.LEVEL)PlayerPrefs.GetInt(Fixed.levelKey, (int)Fixed.LEVEL.easy);

        switch(level)
        {
            case Fixed.LEVEL.easy:
                // 円
                SetCircleInfo();
                break;
            case Fixed.LEVEL.normal:
                // 扇弾設定
                SetFanInfo();
                // 円
                SetCircleInfo();
                break;
            case Fixed.LEVEL.hard:
                // 扇弾設定
                SetFanInfo();
                // ベジェ曲線弾の設定
                SetBezierInfo();
                // 円
                SetCircleInfo();
                break;
        }

        // hpバーの設定
        hpBar.maxValue = maxHp;
        hpBar.value = hpBar.maxValue;
        // se
        seSource = GetComponent<AudioSource>();
        // particle
        GameObject p = Instantiate(Resources.Load<GameObject>("Prefubs/Explosion"));
        particle = p.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dt"></param>
    public bool BossUpdate(float dt)
    {
        if(hp <= 0)
        {
            return false;
        }

        switch(level)
        {
            case Fixed.LEVEL.easy:
                circleBarrage.UpdateBulletList(targetObj.transform.position, dt, circleInterval, true);
                break;
            case Fixed.LEVEL.normal:
                fanBarrage.UpdateBulletList(targetObj.transform.position, dt, fanInterval, true);
                circleBarrage.UpdateBulletList(targetObj.transform.position, dt, circleInterval, true);
                break;
            case Fixed.LEVEL.hard:
                fanBarrage.UpdateBulletList(targetObj.transform.position, dt, fanInterval, true);
                bezierBarrage.UpdateBulletList(targetObj.transform.position, dt, bezierInterval, true);
                circleBarrage.UpdateBulletList(targetObj.transform.position, dt, circleInterval, true);
                break;
        }

        Move(dt);

        return true;
    }

    /// <summary>
    /// 弾の親オブジェクト生成
    /// </summary>
    public void CreateBulletParent()
    {
        bulletParent = new GameObject(Fixed.genName);
        bulletParent.transform.SetParent(transform, false);
        bulletParent.transform.SetAsFirstSibling();
    }

    /// <summary>
    /// ダメージイベントセット
    /// </summary>
    public void SetDamageEvent()
    {
        // 敵の弾リスト取得
        Transform child = targetObj.transform.GetChild(0);
        bulletList = new List<BulletUnchi>(child.GetComponentsInChildren<BulletUnchi>(true));

        // イベントセット
        foreach (BulletUnchi bu in bulletList)
        {
            bu.SetEnemyHitEvent(() =>
            {
                seSource.Play();

                particle.transform.position = transform.position;
                particle.Play();

                hpBar.value = hp -= bu.damagePoint;
            });
        }
    }

    /// <summary>
    /// 扇弾設定
    /// </summary>
    private void SetFanInfo()
    {
        fanBarrage = bulletParent.AddComponent<Fan>();

        int bulletMax = 5;
        float[] speed = new float[bulletMax];
        int barrageMax = 5;
        for(int i = 0; i < speed.Length; i++)
        {
            speed[i] = 8;
        }

        fanBarrage.Init(Color.yellow, bulletMax, speed, barrageMax);
    }

    /// <summary>
    /// ベジェ曲線弾の設定
    /// </summary>
    private void SetBezierInfo()
    {
        bezierBarrage = bulletParent.AddComponent<Bezier>();

        int bulletMax = 5;
        float[] speed = new float[bulletMax];
        int barrageMax = 5;
        for (int i = 0; i < speed.Length; i++)
        {
            speed[i] = 1;
        }

        bezierBarrage.Init(Color.white, bulletMax, speed, barrageMax);
    }

    /// <summary>
    /// 円形弾の設定
    /// </summary>
    private void SetCircleInfo()
    {
        circleBarrage = bulletParent.AddComponent<Circle>();

        int bulletMax = 36;
        float[] speed = new float[bulletMax];
        int barrageMax = 10;
        for (int i = 0; i < speed.Length; i++)
        {
            speed[i] = 2;
        }

        circleBarrage.Init(Color.red, bulletMax, speed, barrageMax);
    }

    private void Move(float dt)
    {
        moveInterval += dt * moveSpeed;
        float x = Mathf.Sin(moveInterval) * xMoveMax;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
