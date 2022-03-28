using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // シーン遷移用
using UnityEngine.UI;               // UI用

public class Pause : MonoBehaviour
{
    // ポーズした時に表示するUI
    [SerializeField]
    private GameObject PauseUI;

    // ポーズ画面のボタン
    public Button Resume;
    public Button Retry;
    public Button Title;
    public Button Option;
    private int MaxButton = 3;
    private int SelectButton = 0;

    private int Cnt = 0;

    public void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
        {
            SelectButton--;
            if (SelectButton < 0)
            {
                SelectButton = MaxButton;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
        {
            SelectButton++;
            if (SelectButton > MaxButton)
            {
                SelectButton = 0;
            }
        }

        switch(SelectButton)
        {
            case 0:
                Resume.Select();
                break;

            case 1:
                Retry.Select();
                break;

            case 2:
                Title.Select();
                break;

            case 3:
                Option.Select();
                break;
        }

    }

    // リトライ
    public void OnRetry()
    {
        // 同一シーンを読込
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    // タイトルへ
    public void OnTitle()
    {
        // 同一シーンを読込
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }

    // 再開
    public void OnResume()
    {
        //　ポーズUIのアクティブ、非アクティブを切り替え
        PauseUI.SetActive(!PauseUI.activeSelf);
        Time.timeScale = 1f;
    }
}