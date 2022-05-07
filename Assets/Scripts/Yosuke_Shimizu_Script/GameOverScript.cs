using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    //敵にぶつかってしまったかどうかのフラグ
    private bool g_bGameOverFlg;

    //文字のやつ
    public Text GO_Tex;

    //ゲームオーバーの画像を出すためのやつ
    public Image _GameOverBG;

    // Start is called before the first frame update
    void Start()
    {
        //初めは入らないようにする
        g_bGameOverFlg = false;

        //最初は非アクティブ
        _GameOverBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (g_bGameOverFlg) {

            Debug.Log("ロゼッタ愛してる");
            if (GO_Tex != null) GO_Tex.text = "　　　　失敗！";

            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlaySeByName("jingle37");

            _GameOverBG.gameObject.SetActive(true);
           
        }
    }

    //敵にぶつかったかどうかのフラグを得る
    public void SetGameOver_Flg(bool GO_Flg) {
        g_bGameOverFlg = GO_Flg;
    }

}
