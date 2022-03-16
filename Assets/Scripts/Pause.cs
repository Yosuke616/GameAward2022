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

    public void Start()
    {
        // 最初の選択
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

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