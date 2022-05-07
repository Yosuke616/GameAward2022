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

    // Start is called before the first frame update
    void Start()
    {
        //初めは入ら無いようにする
        g_bGoal = false;

        //アクティブにしない
        _resultBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            g_bGoal = true;
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
            }
            //Camera.transform.position = new Vector3(3.5f,-1.5f,-5.0f);

        }
    }

    //クリアしたかどうかのフラグを得る
    public void SetGoalFlg(bool GoalFlg) {
        g_bGoal = GoalFlg;
    }
}
