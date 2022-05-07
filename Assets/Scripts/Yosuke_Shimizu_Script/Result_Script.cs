using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Result_Script : MonoBehaviour
{
    //クリアしたかどうかのフラグ
    private bool g_bGoal;

    //よる場所
    private Vector3 targetPos = new Vector3(3.5f, -1.5f, -5.0f);

    //寄っていくスピード
    private float m_fSpeed = 0.5f;

    //ゆっくりさせる
    private float m_fYukkuri = 0.25f;

    //計算用変数
    private Vector3 m_fvelocity;

    //文字を出すためのやつ
    public Text tex;
    public Text timerTex;

    //使用する画像
    public Image _resultBG;

    //星の画像
    public Image Star_1;
    public Image Star_2;
    public Image Star_3;

    //タイムを記録する人
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        //初めは入ら無いようにする
        g_bGoal = false;

        //アクティブにしない
        _resultBG.gameObject.SetActive(false);

        //星はデフォルトではアクティブにしない
        Star_1.gameObject.SetActive(false);
        Star_2.gameObject.SetActive(false);
        Star_3.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        GameObject GO = GameObject.Find("ParentPlayer");

        if (!g_bGoal && GO.GetComponent<PlayerMove2>().GetGameOverFlg()) {
            //時間を取得してある程度早かったら星の画像を出す
            GameObject Time = GameObject.Find("Timer");

            Timer = Time.GetComponent<TimerScript>().GetTime();
        }

        if (g_bGoal) {
            //カメラがよる
            GameObject Camera = GameObject.Find("MainCamera");

            
            m_fvelocity += ((targetPos - Camera.transform.position) * m_fSpeed);

            m_fvelocity *= m_fYukkuri;

            Camera.transform.position += m_fvelocity;

            if (Camera.transform.position == targetPos) {
                // ステージ進捗保存
                StageSelect.UpdateProgress(SceneManager.GetActiveScene().name);

                if (tex && timerTex) tex.text = "クリアタイム:" + timerTex.text;

                SoundManager.Instance.StopBgm();
                SoundManager.Instance.PlaySeByName("clear");

                _resultBG.gameObject.SetActive(true);

                if (Timer < 30.0f) {
                    Star_1.gameObject.SetActive(true);
                    Star_2.gameObject.SetActive(true);
                }

            }
            //Camera.transform.position = new Vector3(3.5f,-1.5f,-5.0f);

        }
    }

    //クリアしたかどうかのフラグを得る
    public void SetGoalFlg(bool GoalFlg) {
        g_bGoal = GoalFlg;
    }
}
