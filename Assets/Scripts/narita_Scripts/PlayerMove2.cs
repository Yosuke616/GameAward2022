using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// プレイヤーの移動クラス
public class PlayerMove2 : MonoBehaviour
{
    enum PLAYER_STATE
    {
        STATE_STOP,
        STATE_LEFT_MOVE,
        STATE_RIGHT_MOVE,
    }

    public float offScrren = -12.4f;


    Rigidbody rb;
    
    //bool bGround;
    public float speed = 1.0f;

    public Text tex;
    public Text timerTex;
    public Image _resultBG;
    private PLAYER_STATE _state;

    private bool flg;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //bGround = false;

        _resultBG.gameObject.SetActive(false);
        flg = true;

        _state = PLAYER_STATE.STATE_STOP;
    }

    // 更新
    void Update()
    {
        if (flg)
        {
            // プレイヤーの向き
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") == -1)        _state = PLAYER_STATE.STATE_LEFT_MOVE;
            else if (Input.GetKey(KeyCode.D)|| Input.GetAxis("Horizontal") == 1)   _state = PLAYER_STATE.STATE_RIGHT_MOVE;
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < 0) _state = PLAYER_STATE.STATE_STOP;
            // ジャンプ
            else if (Input.GetKeyDown(KeyCode.W)) GetComponent<Rigidbody>().AddForce(new Vector3(0, 10.0f, 0), ForceMode.Impulse);

            switch (_state)
            {
                case PLAYER_STATE.STATE_STOP: break;
                case PLAYER_STATE.STATE_LEFT_MOVE: transform.position += -transform.right * speed; break;
                case PLAYER_STATE.STATE_RIGHT_MOVE: transform.position += transform.right * speed; break;
                default: break;
            }
        }

        // 画面外に出たらゲームオーバー
        if(transform.position.y < offScrren)
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
            // ステージ進捗保存
            //StageSelect.UpdateProgress(SceneManager.GetActiveScene().name);

            //if (tex && timerTex) tex.text = "クリアタイム:" + timerTex.text;

            //SoundManager.Instance.StopBgm();
            //SoundManager.Instance.PlaySeByName("clear");

            //_resultBG.gameObject.SetActive(true);

            GameObject camera = GameObject.Find("MainCamera");
            camera.GetComponent<Result_Script>().SetGoalFlg(true);

            flg = false;
        }

        // 敵に触れたとき
        else if (collision.gameObject.gameObject.tag == "enemy")
        {
            if(tex != null) tex.text = "　　　　失敗！";

            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlaySeByName("jingle37");

            _resultBG.gameObject.SetActive(true);
            flg = false;
        }
    }

    //ゴールしたか死んだか
    public bool GetFlg() {
        return flg;
    }

}
