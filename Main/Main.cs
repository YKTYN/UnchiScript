using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    enum RESULT_KIND
    {
        none = 0,
        boss,
        player,
    }

    [SerializeField]
    private BossUnchi boss = default;
    [SerializeField]
    private Player player = default;
    [SerializeField]
    private AudioSource endSe = default;

    //　ゲームおーばーフラグ
    private bool isGameOver = false;
    //　勝敗種類(どっちが勝ったか)
    private RESULT_KIND resultKind = RESULT_KIND.none;

    //　ゲーム開始時の現在時刻
    private DateTime startTime;
    //　ゲーム終了字んｐ現在時刻
    private DateTime endTime;

    private bool isResultEvent = false;

    private void Start()
    {
        boss.CreateBulletParent();
        player.CreateBulletParent();

        boss.Init();
        player.Init();

        boss.SetDamageEvent();
        player.SetDamageEvent();

        startTime = DateTime.Now;
        endTime = DateTime.Now;

        InvokeRepeating(Util.GetMethodName(MoveResult), 0.0f, 0.5f);
    }

    private void MoveResult()
    {
        if(!isGameOver || isResultEvent)
        {
            return;
        }
        isResultEvent = true;

        endTime = DateTime.Now;

        string text = resultKind == RESULT_KIND.boss ? "倒された" : "倒した";
        TimeSpan span = (endTime - startTime);
        string hour = span.Hours <= 0 ? "" : span.Hours.ToString() + "時間";
        string minutes = span.Minutes <= 0 ? "" : span.Minutes.ToString() + "分";
        string seconds = span.Seconds.ToString() + "秒";

        string test = hour + minutes + seconds + "で" + text + "！\n被弾回数は" + player.GetHitCount() + "回！\nすごい！";

        endSe.Play();

        PlayerPrefs.SetString(Fixed.resultKey, test);
        Invoke(Util.GetMethodName(Load), 1.5f);
    }

    private void Load()
    {
        SceneManager.LoadScene("Result");
    }

    private void FixedUpdate()
    {
        if(isGameOver)
        {
            return;
        }

        float dt = Time.deltaTime;
        bool bossLose = !boss.BossUpdate(dt);
        bool playerLose = !player.PlayerUpdate(dt);
        isGameOver = bossLose || playerLose;
        resultKind = bossLose ? RESULT_KIND.player : RESULT_KIND.boss;

        Time.timeScale = Input.GetMouseButton(0) ? Fixed.slow : 1.0f;
    }
}
