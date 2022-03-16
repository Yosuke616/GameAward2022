using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // シーン遷移用
using UnityEngine.UI;               // UI用

public class Pause : MonoBehaviour
{
    // ポーズした時に表示するUI
    [SerializeField]
    private GameObject PauseUI;

    // ポーズ画面の最初の選択されているボタン
    public Button FirstSelectButton;

    // リトライするシーン
    public SceneObject RetryScene;

    // タイトルに行くシーン
    public SceneObject TitleScene;

    public 

    void Start()
    {
        // 最初の選択
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        // キーボードの p かコントローラーのStartで入力
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 7"))
        {
            //　ポーズUIのアクティブ、非アクティブを切り替え
            PauseUI.SetActive(!PauseUI.activeSelf);

            //　ポーズUIが表示されてる時は停止
            if (PauseUI.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    // リトライ
    public void OnRetry()
    {
        // 同一シーンを読込
        SceneManager.LoadScene(RetryScene);
        Time.timeScale = 1f;
    }

    // タイトルへ
    public void OnTitle()
    {
        // 同一シーンを読込
        SceneManager.LoadScene(TitleScene);
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