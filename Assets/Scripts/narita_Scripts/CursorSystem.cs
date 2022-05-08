using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField] private List<Vector3> MousePoints;

    public int cnt = 0;

    private bool bDivide;

    // スクリーン座標
    Vector3 screenPoint;

    [SerializeField] private int Select;

    [SerializeField] private List<GameObject> papers;

    // ゲームモード
    public enum GameState
    {
        MODE_OPENING, // ウサギのシーン
        MODE_ACTION, // 紙を破れるモード
        MODE_TURN_PAGES, // めくるモード
    }
    [SerializeField] static private GameState gameState;
    static public void SetGameState(GameState state){ gameState = state; }


    // 初期化
    void Start()
    {
        MousePoints = new List<Vector3>();

        // 紙の束
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

        Select = 0;

        // 最初はウサギのシーン
        gameState = GameState.MODE_OPENING;

        if(GameObject.Find("Rabbit") == null)
        {
            Debug.LogWarning("ウサギいないよ    MODE_ACTIONから始めるよ");
            gameState = GameState.MODE_ACTION;

        }
    }

    void Update()
    {
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            // debug用
            if (Input.GetKeyDown(KeyCode.X))
            {
                List<bool> a = new List<bool>();
                for (int i = 0; i < 2880; i++)
                {
                    a.Add(true);
                }
                CollisionField.Instance.UpdateStage(a);
            }

            // MainCameraがenableではない場合は何もしない
            if (Camera.main == null) { return; }

            // オープニングモードの場合は何もしない
            if (gameState == GameState.MODE_OPENING) return;

            #region ---めくる処理
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("joystick button 4"))
            {
                UpdatePage();
                var topPaper = papers[Select];
                var turnShader = topPaper.GetComponent<Turn_Shader>();
                // めくる
                turnShader.SetPaperSta(1);
                // めくった枚数をカウント
                Select++;
                // めくる枚数の上限
                if (Select > 2) Select = 2;

                //--- 紙の子オブジェクトのブレークラインも消す
                for (int i = 0; i < topPaper.transform.childCount; i++)
                {
                    // 子オブジェクトの取得
                    var childObject = topPaper.transform.GetChild(i).gameObject;
                    // 仕切りの場合は何もしない
                    if (childObject.tag == "partition") continue;

                    // アクティブを解除
                    childObject.SetActive(false);
                }

                // めくるモードに変更
                SetGameState(GameState.MODE_TURN_PAGES);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 5"))
            {
                UpdatePage();
                // めくった枚数をカウント
                Select--;
                // めくる枚数の下限
                if (Select < 0) Select = 0;

                // めくるのを戻す
                var topPaper = papers[Select];
                var turnShader = topPaper.GetComponent<Turn_Shader>();
                // めくってある状態から戻す
                turnShader.SetPaperSta(2);

                //--- ブレークラインも戻す
                for (int i = 0; i < topPaper.transform.childCount; i++)
                {
                    // 子オブジェクトの取得
                    var childObject = topPaper.transform.GetChild(i).gameObject;
                    // 仕切りの場合は何もしない
                    if (childObject.tag == "partition") continue;

                    // アクティブにする
                    childObject.SetActive(true);
                }

                if(Select == 0)
                {
                    // めくるモード → アクションモード
                    SetGameState(GameState.MODE_ACTION);
                }
            }
            #endregion

            #region ---破る処理
            // めくるモードの場合は処理を行わない
            if (gameState == GameState.MODE_TURN_PAGES) return;

            // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            // マウス座標のzの値を0にする
            Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            // マウス座標をワールド座標に変換する
            transform.position = Camera.main.ScreenToWorldPoint(cursor);

            //カーソルの座標を送る
            GameObject Cursor = GameObject.Find("cursor");
            GameObject obj = GameObject.Find("CTRLCur");
            GameObject camera = GameObject.Find("MainCamera");

            //座標の保存用の変数
            Vector3 SavePos = new Vector3(0.0f, 0.0f, 0.0f);

            //送るものの座標を変える
            if (Cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg())
            {
                SavePos = obj.transform.position;
            }
            else
            {

                SavePos = Cursor.transform.position;
            }

            // 座標保存
            if (Input.GetMouseButtonDown(0) || camera.GetComponent<InputTrigger>().GetOneTimeDown())
            {

                cnt = 0;

                // 座標リストに追加
                //MousePoints.Add(SavePos);
                MousePoints.Add(transform.position);

                Debug.LogWarning(transform.position);

                if (MousePoints.Count >= 2)
                {
                    papers.Clear();
                    // 紙のリストを作る
                    //List<GameObject> objects = new List<GameObject>();
                    papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

                    if (papers != null)
                    {
                        // ソート
                        if (papers.Count >= 2)
                        {
                            // 紙の番号、昇順
                            papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
                        }


                        bDivide = false;

                        for (int i = 0; i < papers.Count; i++)
                        {
                            var divideTriangle = papers[i].GetComponent<DivideTriangle>();
                            if (divideTriangle)
                            {
                                // 破る
                                bDivide = divideTriangle.Divide(ref MousePoints);
                                cnt++;

                                // 一度破ったら、奥の紙は切らない
                                if (bDivide) break;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                MousePoints.Clear();
            }
            #endregion
        }
        else {
            this.gameObject.SetActive(false);
        }

    }

    //切っているかどうかのフラグをゲットするための関数
    public bool GetBreakFlg() {
        return bDivide;
    }


    private void UpdatePage()
    {
        papers.Clear();

        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

        // 紙の番号、昇順
        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
    }
}
