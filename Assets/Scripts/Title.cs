using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;  // シーン遷移用
using UnityEngine.UI;               // UI用

public class Title : MonoBehaviour
{
    // タイトル画面の最初の選択されているボタン
    public Button FirstSelectButton;

    // リトライスルシーン
    public SceneObject StartScene;

    void Start()
    {
        // 最初の選択
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ゲーム開始
    public void OnStart()
    {
        // 開始シーンを読込
        SceneManager.LoadScene(StartScene);
    }

    // 終了
    public void OnEnd()
    {
// 開発環境用
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
// それ以外
#else
        Application.Quit();
#endif
    }
}