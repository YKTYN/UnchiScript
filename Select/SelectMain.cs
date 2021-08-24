using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMain : MonoBehaviour
{
    [SerializeField]
    private GameObject levelMenu = default;
    [SerializeField]
    private GameObject creditWindow = default;
    [SerializeField]
    private GameObject checkMark = default;

    [SerializeField]
    private Button level = default;
    [SerializeField]
    private Button start = default;
    [SerializeField]
    private Button credit = default;
    [SerializeField]
    private Button creditHide = default;
    [SerializeField]
    private Button easy = default;
    [SerializeField]
    private Button normal = default;
    [SerializeField]
    private Button hard = default;

    //　SE用のオーディオソース
    private AudioSource seSource;

    // Start is called before the first frame update
    void Start()
    {
        seSource = GetComponent<AudioSource>();

        SetCheckMark(false, easy.transform);
        // 難易度選択を開く
        level.onClick.AddListener(() =>
        {
            seSource.Play();
            levelMenu.SetActive(true);
        });

        // 難易度設定（Easy）
        easy.onClick.AddListener(() =>
        {
            seSource.Play();
            SetCheckMark(true, easy.transform);
            PlayerPrefs.SetInt(Fixed.levelKey, (int)Fixed.LEVEL.easy);
            levelMenu.SetActive(false);
        });
        // 難易度設定（Normal）
        normal.onClick.AddListener(() =>
        {
            seSource.Play();
            SetCheckMark(true, normal.transform);
            PlayerPrefs.SetInt(Fixed.levelKey, (int)Fixed.LEVEL.normal);
            levelMenu.SetActive(false);
        });
        // 難易度設定（Hard）
        hard.onClick.AddListener(() =>
        {
            seSource.Play();
            SetCheckMark(true, hard.transform);
            PlayerPrefs.SetInt(Fixed.levelKey, (int)Fixed.LEVEL.hard);
            levelMenu.SetActive(false);
        });

        // メインシーンに行く
        start.onClick.AddListener(() =>
        {
            seSource.Play();
            SceneManager.LoadScene("MainScene");
        });


        // クレジットを開く
        credit.onClick.AddListener(() =>
        {
            seSource.Play();
            creditWindow.SetActive(true);
        });
        // クレジットを閉じる
        creditHide.onClick.AddListener(() =>
        {
            seSource.Play();
            creditWindow.SetActive(false);
        });
    }

    private void SetCheckMark(bool enable, Transform parent)
    {
        checkMark.SetActive(enable);
        checkMark.transform.SetParent(parent);
        checkMark.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
