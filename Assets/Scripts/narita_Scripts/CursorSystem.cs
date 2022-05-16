using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField] private List<Vector3> MousePoints;

    public int cnt = 0;

    private int maxPaper = 2;

    private bool bDivide;

    // スクリーン座標
    Vector3 screenPoint;

    [SerializeField] private int selectPaper;

    [SerializeField] private List<GameObject> papers;

    // ゲームモード
    public enum GameState
    {
        MODE_OPENING,       // ウサギのシーン
        MODE_ACTION,        // 紙を破れるモード
        MODE_TURN_PAGES,    // めくるモード
    }
    [SerializeField] static private GameState gameState;
    static public void SetGameState(GameState state){ gameState = state; }
    static public GameState GetGameState() { return gameState; }

    // 初期化
    void Start()
    {
        MousePoints = new List<Vector3>();

        // 紙の束
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
        // 紙の枚数を保存(0からカウントしてるので-1する)
        maxPaper = papers.Count - 1;
        // 紙の番号、昇順
        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
        Debug.Log(maxPaper);

        selectPaper = 0;

        // 最初はウサギのシーン
        gameState = GameState.MODE_OPENING;
        gameState = GameState.MODE_ACTION;

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
                if (selectPaper < maxPaper)
                {
                    UpdatePage();
                    var topPaper = papers[selectPaper];
                    var turnShader = topPaper.GetComponent<Turn_Shader>();
                    // めくる
                    turnShader.SetPaperSta(1);
                    // めくった枚数をカウント
                    selectPaper++;
                    // めくる枚数の上限
                    if (selectPaper > maxPaper) selectPaper = maxPaper;

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
                }

                // めくるモードに変更
                SetGameState(GameState.MODE_TURN_PAGES);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 5"))
            {
                // 1枚目の時は何もしない
                if(selectPaper != 0)
                {
                    UpdatePage();
                    // めくるのを戻す
                    var topPaper = papers[selectPaper - 1];
                    var turnShader = topPaper.GetComponent<Turn_Shader>();
                    // めくってある状態から戻す
                    turnShader.SetPaperSta(2);

                    // めくった枚数をカウント
                    selectPaper--;
                    // めくる枚数の下限
                    if (selectPaper == 0)
                    {
                        //selectPaper = 0;

                        // めくるモード → アクションモード
                        SetGameState(GameState.MODE_ACTION);
                    }

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
            var outsider = Cursor.GetComponent<OutSide_Paper_Script_Second>();
            GameObject Padobj = GameObject.Find("CTRLCur");
            GameObject camera = GameObject.Find("MainCamera");





            // 座標保存
            if (Input.GetMouseButtonDown(0) || camera.GetComponent<InputTrigger>().GetOneTimeDown())
            {
                //座標の保存用の変数
                Vector3 SavePos = Vector3.zero;

                //送るものの座標を変える
                if (outsider.GetFirstFlg())
                {
                    Debug.Log("2回目以降");

                    // マウス
                    if (Input.GetMouseButtonDown(0)) SavePos = transform.position;
                    // ゲームパッド
                    else SavePos = Padobj.transform.position;
                }
                else
                {
                    Debug.LogWarning("1回目");
                    // 破る処理スタート
                    outsider.DivideStart();

                    SavePos = Cursor.transform.position;
                }

                cnt = 0;

                // 座標リストに追加
                MousePoints.Add(SavePos);
                //MousePoints.Add(transform.position);
                //Debug.LogWarning(SavePos);


                if (MousePoints.Count >= 2)
                {
                    papers.Clear();
                    // 紙のリストを作る
                    papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

                    if (papers != null)
                    {
                        // ソート
                        if (papers.Count >= 2)
                        {
                            // 紙の番号、昇順
                            papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
                        }

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
