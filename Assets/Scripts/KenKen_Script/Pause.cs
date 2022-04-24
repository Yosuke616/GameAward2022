using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // シーン遷移用
using UnityEngine.UI;               // UI用

public class Pause : MonoBehaviour
{
    // ポーズした時に表示するUI
    public GameObject PauseUi;
    public GameObject Pausepanel;
    public GameObject Optionpanel;

    // ポーズ画面のボタン
    public Button Resume;
    public Button Retry;
    public Button Title;
    public Button Option;
    private int MaxButton = 3;
    private int SelectButton = 0;

    private int Cnt = 0;

    private bool OptionFlg;

    public void Start()
    {
        Optionpanel.SetActive(false);
        OptionFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        Cnt--;
        if (Cnt < 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
            {
                Cnt = 75;
                SelectButton--;
                if (SelectButton < 0)
                {
                    SelectButton = MaxButton;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
            {
                Cnt = 75;
                SelectButton++;
                if (SelectButton > MaxButton)
                {
                    SelectButton = 0;
                }
            }
        }

        //常に白にかえておく
        Resume.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Retry.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Title.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Option.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        switch (SelectButton)
        {
            case 0:
                Resume.Select();
                Resume.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                Debug.Log("Resume");
                break;

            case 1:
                Retry.Select();
                Retry.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                Debug.Log("Select");
                break;
            case 2:
                Title.Select();
                Title.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                Debug.Log("Title");
                break;

            case 3:
                Option.Select();
                Option.GetComponentInChildren<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                Debug.Log("Option");
                break;
        }


        //ボタンを押せるかどうかを判別する
        GameObject Camera = GameObject.Find("MainCamera");
        if (Camera.GetComponent<PauseContorol>().GetPauseFlf() && OptionFlg == false) {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (SelectButton) {
                    case 0: OnResume(); break;
                    case 1: OnRetry(); break;
                    case 2: OnTitle(); break;
                    case 3: OnOption(); break;
                }
            }

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
        SceneManager.LoadScene("StageSelect");
        Time.timeScale = 1f;
    }

    // 再開
    public void OnResume()
    {
        //　ポーズUIのアクティブ、非アクティブを切り替え
        PauseUi.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnOption()
    {
        Pausepanel.SetActive(false);
        Optionpanel.SetActive(true);
        OptionFlg = true;
    }

    public void OffOption()
    {
        Pausepanel.SetActive(true);
        Optionpanel.SetActive(false);
    }

}