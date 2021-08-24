using System;
using UnityEngine;

public class BulletUnchi : MonoBehaviour
{
    //　ダメージ量、弾の種類とかで値変えるか迷う
    public float damagePoint
    {
        get { return 1.0f; }
    }


    //　曲線系のまとめたやつ
    private Curve curve;
    //　速度
    private float speed = 0.0f;
    //　加速度
    private float acceleration = 0.0f;
    //　待機フラグ
    private bool isStandby;
    //　発射位置
    private Vector2 startPos;
    //　ターゲット位置
    private Vector2 targetPos;
    //　ベジェ曲線の経由地の振れ幅調整用
    private float viaAdjust;
    //　コライダーに当たったぞい
    private bool isHitCollider;
    //　プレイヤーに当たった時のイベント
    private delegate void PlayerHitEvent();
    private event PlayerHitEvent playerHitEvent;
    //　エネミーに当たった時のイベント
    private delegate void EnemyHitEvent();
    private event EnemyHitEvent enemyHitEvent;
    //　スプライトの表示切り替え用
    private SpriteRenderer[] spriteArray;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="scale">大きさ</param>
    /// <param name="angle">角度</param>
    /// <param name="speed">速度</param>
    /// <param name="color">色</param>
    public void Init(Vector2 scale, Vector2 angle, float speed, Color color)
    {
        curve = new Curve();
        Vector3 _scale = new Vector3(
            transform.localScale.x / transform.lossyScale.x * scale.x,
            transform.localScale.y / transform.lossyScale.y * scale.y,
            transform.localScale.z / transform.lossyScale.z * 1.0f
            );
        transform.localScale = _scale;
        transform.eulerAngles = angle;
        this.speed = speed;
        isStandby = true;
        isHitCollider = false;
        startPos = transform.position;
        GetComponent<SpriteRenderer>().color = color;
        spriteArray = GetComponentsInChildren<SpriteRenderer>();
        SetSpriteEnabled(false);
    }
    
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dt"></param>
    public void BulletUpdate(float dt)
    {
        if (isStandby)
        {
            return;
        }
        acceleration += dt * speed;
    }

    /// <summary>
    /// 再利用のために色々戻す
    /// </summary>
    public void GoBack()
    {
        acceleration = 0.0f;
        isStandby = true;
        isHitCollider = false;
        SetSpriteEnabled(false);
    }

    /// <summary>
    /// 極座標指定するやつを使って半径に加速度を入れて飛ばしてる
    /// </summary>
    /// <param name="angle">中心点からの角度</param>
    public void CircularCoordinates(float angle)
    {
        float x = startPos.x + curve.CircularCoordinates(acceleration, angle).x;
        float y = startPos.y + curve.CircularCoordinates(acceleration, angle).y;
        transform.position = new Vector2(x, y);
    }

    /// <summary>
    /// まっすぐ動かす
    /// </summary>
    public void Straight(bool enemy)
    {
        float targetY = enemy ? Fixed.bottom - 1 : Fixed.top + 1;
        Vector2 targetPos = new Vector2(startPos.x, targetY);
        float x = curve.DividingPoint(startPos, targetPos, acceleration).x;
        float y = curve.DividingPoint(startPos, targetPos, acceleration).y;
        transform.position = new Vector2(x, y);
    }

    /// <summary>
    /// 2次ベジェ曲線
    /// </summary>
    public void Bezier()
    {
        Vector2 via = curve.DividingPoint(startPos, targetPos, viaAdjust*0.1f) + Vector2.right * viaAdjust;
        float x = curve.Bezier(startPos, via, targetPos, acceleration).x;
        float y = curve.Bezier(startPos, via, targetPos, acceleration).y;
        transform.position = new Vector2(x, y);
    }

    /// <summary>
    /// 画面外判定
    /// </summary>
    /// <returns></returns>
    public bool OffScreen()
    {
        Vector2 pos = transform.position;
        return (pos.x < Fixed.left || pos.x > Fixed.right || pos.y < Fixed.bottom || pos.y > Fixed.top);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool BezierEnd()
    {
        return ((Vector3)targetPos - transform.position).magnitude <= 0.05f;
    }

    /// <summary>
    /// 動く
    /// </summary>
    public void Move(Vector2 startPos, Vector2 targetPos)
    {
        if (!isStandby)
        {
            return;
        }
        this.targetPos = targetPos;
        this.startPos = startPos;
        isStandby = false;
        int max = 6;
        viaAdjust = UnityEngine.Random.Range(0, max * 2) - max;
        SetSpriteEnabled(true);
    }


    /// <summary>
    /// コライダーに当たった
    /// </summary>
    /// <returns></returns>
    public bool GetIsHitCollider()
    {
        return isHitCollider;
    }

    /// <summary>
    /// プレイヤーに衝突したときのイベントをセット
    /// </summary>
    /// <param name="a"></param>
    public void SetPlayerHitEvent(Action a)
    {
        playerHitEvent += () => { a?.Invoke(); };
    }

    /// <summary>
    /// エネミーに衝突したときのイベントをセット
    /// </summary>
    /// <param name="a"></param>
    public void SetEnemyHitEvent(Action a)
    {
        enemyHitEvent += () => { a?.Invoke(); };
    }

    /// <summary>
    /// スプライトの表示切り替え用
    /// </summary>
    /// <param name="value"></param>
    private void SetSpriteEnabled(bool value)
    {
        foreach(SpriteRenderer spr in spriteArray)
        {
            spr.enabled = value;
        }
    }

    /// <summary>
    /// コライダーの判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if(transform.CompareTag(tag))
        {
            return;
        }
        isHitCollider = true;

        switch (tag)
        {
            case Fixed.enemyTag:
                enemyHitEvent?.Invoke();
                break;
            case Fixed.playerTag:
                playerHitEvent?.Invoke();
                break;
            default:
                break;
        }
    }
}
