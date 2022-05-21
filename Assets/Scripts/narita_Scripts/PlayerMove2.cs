﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// プレイヤーの移動クラス
public class PlayerMove2 : MonoBehaviour
{
    [SerializeField] static public int AnimState = 0;

    enum PLAYER_STATE
    {
        STATE_STOP,
        STATE_LEFT_MOVE,
        STATE_RIGHT_MOVE,
    }

    // 下画面
    public float offScrren = -12.4f;
    private float m_rotDestModel;
    const float RATE_ROTATE_MODEL = 0.10f;


    Rigidbody rb;
    
    //bool bGround;
    public float speed = 1.0f;

    public Text tex;
    public Text timerTex;
    public Image _resultBG;
    private PLAYER_STATE _state;

    private bool flg;

    //敵に当たったときのゲームオーバーのフラグ
    private bool GameOver_Flg_Enemy;

    //ゲームオーバーの時に出すUIのやつ
    public Image _GameOverBG;

    //文字もゲームオーバー用のやつを使う
    public Text GO_Tex;

    void Start()
    {
        // リジット
        rb = GetComponent<Rigidbody>();
        
        _resultBG.gameObject.SetActive(false);
        _GameOverBG.gameObject.SetActive(false);
        GameOver_Flg_Enemy = true;

        flg = true;

        // プレイヤーのステート
        _state = PLAYER_STATE.STATE_STOP;

        m_rotDestModel = 45.0f;
    }

    // 更新
    void Update()
    {
        if (flg && GameOver_Flg_Enemy)
        {
            // プレイヤーの向き
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") == -1)        _state = PLAYER_STATE.STATE_LEFT_MOVE;
            else if (Input.GetKey(KeyCode.D)|| Input.GetAxis("Horizontal") == 1)   _state = PLAYER_STATE.STATE_RIGHT_MOVE;
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < 0) _state = PLAYER_STATE.STATE_STOP;
            // ジャンプ
            else if (Input.GetKeyDown(KeyCode.W)) GetComponent<Rigidbody>().AddForce(new Vector3(0, 10.0f, 0), ForceMode.Impulse);

            switch (_state)
            {
                case PLAYER_STATE.STATE_STOP:
                    // 待機モーション
                    if(AnimState < 2)
                    AnimState = 0;
                    break;
                case PLAYER_STATE.STATE_LEFT_MOVE:
                    transform.position += -transform.right * speed;
                    m_rotDestModel = -45.0f;
                    // 歩きモーション
                    if(AnimState < 2)
                    AnimState = 1;
                    break;
                case PLAYER_STATE.STATE_RIGHT_MOVE:
                    m_rotDestModel = 45.0f;
                    transform.position += transform.right * speed;
                    // 歩きモーション
                    if(AnimState < 2)
                    AnimState = 1;
                    break;
                default: break;
            }
        }

        //// 目的の角度までの差分
        //float m_fDiifRotY = m_rotDestModel - this.transform.rotation.y;
        //if (m_fDiifRotY >= 180.0f) m_fDiifRotY -= 360.0f;
        //if (m_fDiifRotY < -180.0f) m_fDiifRotY += 360.0f;
        //// 目的の角度まで慣性をかける
        //this.transform.Rotate(0.0f, m_fDiifRotY * RATE_ROTATE_MODEL, 0.0f);
        //m_pPlayer->Rotate.y += m_fDiifRotY * RATE_ROTATE_MODEL;
        //if (m_pPlayer->Rotate.y > 360.0f) m_pPlayer->Rotate.y -= 360.0f;
        //if (m_pPlayer->Rotate.y < 0.0f) m_pPlayer->Rotate.y += 360.0f;

        // 画面外に出たらゲームオーバー
        if (transform.position.y < offScrren)
        {
            if (tex != null) tex.text = "　　　　失敗！";

            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlaySeByName("jingle37");

            _resultBG.gameObject.SetActive(true);
            flg = false;
        }
    }

    // 衝突処理
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Ground")
        //{
        //    bGround = true;
        //}

        // ゴールに触れたとき
        if (collision.gameObject.gameObject.tag == "goal")
        {
            GameObject camera = GameObject.Find("MainCamera");
            camera.GetComponent<Result_Script>().SetGoalFlg(true);

            AnimState = 2;

            flg = false;
        }

        // 敵に触れたとき
        else if (collision.gameObject.gameObject.tag == "enemy" || collision.gameObject.gameObject.tag == "CardSoldier")
        {
            GameObject enemy = GameObject.Find("MainCamera");
            enemy.GetComponent<GameOverScript>().SetGameOver_Flg(true);
            GameOver_Flg_Enemy = false;

            AnimState = 3;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ゴールに触れたとき
        if (other.gameObject.gameObject.tag == "goal")
        {
            GameObject camera = GameObject.Find("MainCamera");
            camera.GetComponent<Result_Script>().SetGoalFlg(true);

            AnimState = 2;

            flg = false;
        }

        // 敵に触れたとき
        else if (other.gameObject.gameObject.tag == "enemy" || other.gameObject.gameObject.tag == "CardSoldier")
        {
            GameObject enemy = GameObject.Find("MainCamera");
            enemy.GetComponent<GameOverScript>().SetGameOver_Flg(true);
            GameOver_Flg_Enemy = false;

            AnimState = 3;
        }
    }

    public bool GetFlg() {
        return flg;
    }

    //敵に当たったときのゲームオーバーのやつ
    public bool GetGameOverFlg() {
        return GameOver_Flg_Enemy;
    }

}
