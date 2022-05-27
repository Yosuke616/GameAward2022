using System.Collections;
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

    const float limitLeft = -22.0f;
    const float limitRight = 22.0f;


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

    //認識が甘かった
    private bool SE;

    void Start()
    {
        AnimState = 0;
        // リジット
        rb = GetComponent<Rigidbody>();
        
        _resultBG.gameObject.SetActive(false);
        _GameOverBG.gameObject.SetActive(false);
        GameOver_Flg_Enemy = true;

        flg = true;

        // プレイヤーのステート
        _state = PLAYER_STATE.STATE_STOP;

        m_rotDestModel = 45.0f;

        SE = false;
    }

    // 更新
    void Update()
    {

        if (CursorSystem.GetGameState() == CursorSystem.GameState.MODE_OPENING) return;


        if (flg && GameOver_Flg_Enemy)
        {
            // プレイヤーの向き
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") == -1)     _state = PLAYER_STATE.STATE_LEFT_MOVE;
            else if (Input.GetKey(KeyCode.D)|| Input.GetAxis("Horizontal") == 1)  _state = PLAYER_STATE.STATE_RIGHT_MOVE;
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < 0) _state = PLAYER_STATE.STATE_STOP;
            // ジャンプ
            #if UNITY_EDITOR
            else if (Input.GetKeyDown(KeyCode.W)) GetComponent<Rigidbody>().AddForce(new Vector3(0, 10.0f, 0), ForceMode.Impulse);
            #endif

            switch (_state)
            {
                case PLAYER_STATE.STATE_STOP:
                    // 待機モーション
                    if(AnimState < 2)
                    AnimState = 0;
                    break;
                case PLAYER_STATE.STATE_LEFT_MOVE:

                    transform.position += -transform.right * speed;

                    // 歩きモーション
                    if (AnimState < 2)
                    AnimState = 1;
                    break;
                case PLAYER_STATE.STATE_RIGHT_MOVE:
                    transform.position += transform.right * speed;

                    // 歩きモーション
                    if (AnimState < 2)
                    AnimState = 1;
                    break;
                default: break;
            }
        }

        
        Vector3 pos = transform.parent.transform.InverseTransformPoint(transform.position);
        Debug.Log(pos);
        
        if (pos.x > limitRight) { pos.x = limitRight; /* Debug.LogError("");*/ }
        if (pos.x < limitLeft) { pos.x = limitLeft;/* Debug.LogError("");*/ }

        transform.position = transform.parent.gameObject.transform.TransformPoint(pos);

        // 画面外に出たらゲームオーバー
        if (transform.position.y < offScrren)
        {
            // if (tex != null) tex.text = "　　　　失敗！";

            if (!SE) {
                SoundManager.Instance.StopBgm();
                SoundManager.Instance.PlaySeByName("jingle37");
                SE = true;
            }

            GameObject enemy2 = GameObject.Find("MainCamera");
            enemy2.GetComponent<GameOverScript>().SetGameOver_Flg(true);

            _GameOverBG.gameObject.SetActive(true);
            flg = false;
        }
    }

    // 衝突処理
    void OnCollisionEnter(Collision collision)
    {
        // ゴールに触れたとき
        if (collision.gameObject.gameObject.tag == "goal")
        {
            GameObject camera = GameObject.Find("MainCamera");
            camera.GetComponent<Result_Script>().SetGoalFlg(true);

            // クリアモーション
            AnimState = 2;

            // 妖精さんのクリアモーション
            List<GameObject> fairys = new List<GameObject>();
            fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
            foreach (var fairy in fairys)
            {
                // ---ここでスケールを小さくするフラグをONにする
                fairy.GetComponent<FairyInPlayer>().ClearMotion();
            }

            flg = false;
        }

        // 敵に触れたとき
        else if (collision.gameObject.gameObject.tag == "enemy" || collision.gameObject.gameObject.tag == "CardSoldier")
        {
            GameObject enemy = GameObject.Find("MainCamera");
            enemy.GetComponent<GameOverScript>().SetGameOver_Flg(true);
            GameOver_Flg_Enemy = false;

            // 失敗モーション
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
