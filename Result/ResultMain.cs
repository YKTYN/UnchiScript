using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultMain : MonoBehaviour
{
    [SerializeField]
    private Text text = default;
    [SerializeField]
    private Button back = default;

    private AudioSource se;

    // Start is called before the first frame update
    void Start()
    {
        se = back.gameObject.GetComponent<AudioSource>();

        text.text = PlayerPrefs.GetString(Fixed.resultKey, "");

        back.onClick.AddListener(() => { se.Play(); SceneManager.LoadScene("Select"); });
    }
}
