using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //　HPの最大値
    private const float maxHp = 10.0f;


    //　ターゲット
    [SerializeField]
    private GameObject targetObj = default;
    //　hpバー
    [SerializeField]
    private Slider hpBar = default;
    //　現在のhp残尿
    private float hp = maxHp;
    // 弾の親オブジェクト
    private GameObject bulletParent = null;

    //　直線弾
    private Straight straight = null;
    private float straightInterval = 0.1f;

    //　敵の弾リスト
    private List<BulletUnchi> bulletList = null;

    //　SE用のオーディオソース
    private AudioSource seSource;
    //　爆発エフェクト
    private ParticleSystem particle;

    //　被弾回数
    private int hitCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // 直線弾の設定
        SetStraight();
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
    public bool PlayerUpdate(float dt)
    {
        if(hp <= 0)
        {
            return false;
        }

        ConnectToMouse();
        straight.UpdateBulletList(targetObj.transform.position, dt, straightInterval, false);

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
            bu.SetPlayerHitEvent(() =>
            {
                seSource.Play();

                particle.transform.position = transform.position;
                particle.Play();

                hpBar.value = hp -= bu.damagePoint;

                hitCount++;
            });
        }
    }

    /// <summary>
    /// 直線弾の設定
    /// </summary>
    private void SetStraight()
    {
        straight = bulletParent.AddComponent<Straight>();

        int bulletMax = 5;
        float[] speed = new float[bulletMax];
        int barrageMax = 5;
        for (int i = 0; i < speed.Length; i++)
        {
            speed[i] = 3;
        }

        straight.Init(Color.red, bulletMax, speed, barrageMax);
    }

    /// <summary>
    /// マウスに合わせる
    /// </summary>
    private void ConnectToMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        float x = Camera.main.ScreenToWorldPoint(mousePos).x;
        float y = Camera.main.ScreenToWorldPoint(mousePos).y;

        transform.position = new Vector2(x, y+0.15f);
    }

    public int GetHitCount()
    {
        return hitCount;
    }
}
