using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField] private List<Vector3> MousePoints;

    //[SerializeField] private bool startDivide;

    [SerializeField] private int maxPaper = 2;

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
        MODE_CROSSING,      // 破れ線がクロスしてしまった場合
    }
    [SerializeField] static private GameState gameState;
    static public void SetGameState(GameState state) { gameState = state; }
    static public GameState GetGameState() { return gameState; }

    // 初期化
    void Start()
    {
        //startDivide = false;
        MousePoints = new List<Vector3>();

        // 紙の束
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
        // 紙の枚数を保存(0からカウントしてるので-1する)
        maxPaper = papers.Count - 1;
        // 紙の番号、昇順
        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());

        selectPaper = 0;

        // 最初はウサギのシーン
        gameState = GameState.MODE_OPENING;

        if (GameObject.Find("Rabbit") == null)
        {
            Debug.LogWarning("ウサギいないよ    MODE_ACTIONから始めるよ");
            gameState = GameState.MODE_ACTION;
        }
    }

    void Update()
    {
        GameObject player = GameObject.Find("ParentPlayer");
        GameObject fairy = GameObject.Find("Yousei1");

        if (player.GetComponent<PlayerMove2>().GetFlg() && player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            if (fairy.GetComponent<Fiary_Script>().GetState == Fiary_Script.eFairyState.STATE_BREAKING_MOVE) return;

            // debug用
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.X))
            {
                List<bool> a = new List<bool>();
                for (int i = 0; i < 2880; i++)
                {
                    a.Add(true);
                }
                CollisionField.Instance.UpdateStage(a);
            }
            #endif

            // MainCameraがenableではない場合は何もしない
            if (Camera.main == null) { return; }
            // オープニングモードの場合は何もしない
            if (gameState == GameState.MODE_OPENING) { return; }

            #region ---めくる処理
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("joystick button 4"))
            {
                if (selectPaper < maxPaper)
                {
                    UpdatePage();
                    var topPaper = papers[selectPaper];
                    var movePaper = topPaper.GetComponent<PSMove>();

                    // 紙を動かす処理
                    if (movePaper.StartLeft())
                    {
                        // めくった枚数をカウント
                        selectPaper++;
                        // めくる枚数の上限
                        if (selectPaper > maxPaper) selectPaper = maxPaper;
                    }

                    // めくるモードに変更
                    SetGameState(GameState.MODE_TURN_PAGES);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 5"))
            {
                // 1枚目の時は何もしない
                if (selectPaper != 0)
                {
                    UpdatePage();
                    // めくるのを戻す
                    var topPaper = papers[selectPaper - 1];
                    var movePaper = topPaper.GetComponent<PSMove>();

                    // 紙を戻す処理
                    if (movePaper.StartRight())
                    {
                        // めくった枚数をカウント
                        selectPaper--;
                        // めくる枚数の下限
                        if (selectPaper <= 0)
                        {
                            selectPaper = 0;

                            // めくるモード → アクションモード
                            //SetGameState(GameState.MODE_ACTION);
                        }
                    }
                }

            }
            #endregion

            #region ---破る処理
            // めくるモードの場合は処理を行わない
            if (gameState == GameState.MODE_TURN_PAGES) { return; }

            // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            // マウス座標のzの値を0にする
            Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            // マウス座標をワールド座標に変換する
            transform.position = Camera.main.ScreenToWorldPoint(cursor);

            //カーソルの座標を送る
            GameObject Cursor = GameObject.Find("cursor");
            GameObject Padobj = GameObject.Find("CTRLCur");
            GameObject camera = GameObject.Find("MainCamera");

            // 外周を動くオブジェクトのスクリプト
            var outsider = Cursor.GetComponent<OutSide_Paper_Script_Second>();

            // 座標保存
            if (Input.GetMouseButtonDown(0) || camera.GetComponent<InputTrigger>().GetOneTimeDown())
            {
                //座標の保存用の変数
                Vector3 SavePos = Vector3.zero;

                //送るものの座標を変える
                if (outsider.GetFirstFlg() == false)
                {
                    //Debug.LogWarning("1回目");
                    // 破る処理スタート
                    //startDivide = true;
                    outsider.DivideStart();

                    SavePos = Cursor.transform.position;
                }
                else
                {
                    //Debug.Log("2回目以降");

                    // マウス
                    if (Input.GetMouseButtonDown(0)) SavePos = transform.position;
                    // ゲームパッド
                    else SavePos = Padobj.transform.position;
                }

                if (MousePoints.Count > 1 && SavePos.Equals(MousePoints[MousePoints.Count - 1])) { return; }
                // 座標リストに追加
                MousePoints.Add(SavePos);


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

                    int breakingState = 0;
                    for (int paperNum = 0; paperNum < papers.Count; paperNum++)
                    {
                        // 破る処理
                        // 2枚目破き状態のときは1枚目を破けるように
                        // 3枚目破き状態のときは1枚目、2枚目を破けるように
                        // 4枚目破き状態のときは1枚目、2枚目、3枚目を破けるように
                        var divideTriangle = papers[paperNum].GetComponent<DivideTriangle>();

                        breakingState = divideTriangle.Divide(MousePoints);


                        switch (breakingState)
                        {
                            case 0: // 破いてない状態
                                continue;

                            case 1: // 破り途中
                                // 次の紙がすでに破る処理が行われているかチェック
                                //if (paperNum != papers.Count - 1 && papers[paperNum + 1].GetComponent<DivideTriangle>().Dividing == true)
                                if (CheckNextPaperDividing(paperNum))
                                {
                                    // 次の紙も破る
                                    continue;
                                }
                                else
                                {
                                    return;
                                }
                            case 2: // 破り終えた
                                // 手前の紙を破ったら奥の紙は破らない
                                // ※すでに破る処理を開始している奥の紙は破る

                                // 次の紙がすでに破る処理が行われているかチェック
                                if (CheckNextPaperDividing(paperNum))
                                {
                                    Debug.LogWarning("continue");
                                    // 次の紙も破る
                                    continue;
                                }
                                else
                                {
                                    //startDivide = false;
                                    // ポジションリストをクリア
                                    MousePoints.Clear();
                                    return;
                                }

                            case 3: return; // 一番最初はここ

                            case 4: // 予期しない操作の場合はリセットする
                                // 破り中フラグのリセット
                                DivideTriangle.AllReset();
                                //startDivide = false;
                                outsider.DivideEnd();
                                // ポジションリストをクリア
                                MousePoints.Clear();

                                return;

                            default: break;
                        }
                    }
                }
            }

            #endregion

            //　破りキャンセル
            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Q))
            {
                // 破り中フラグのリセット
                DivideTriangle.AllReset();
                outsider.DivideEnd();
                // ポジションリストをクリア
                MousePoints.Clear();

                // ---妖精をプレイヤー追従モードに
                var fs = fairy.GetComponent<Fiary_Script>();
                fs.SetState(Fiary_Script.eFairyState.STATE_FOLLOWING_PLAYER);

                //--- メインカメラの妖精を大きくする
                fs.SmallStart();

                //--- サブカメラの妖精を大きくする
                List<GameObject> fairys = new List<GameObject>();
                fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
                foreach (var f in fairys)
                {
                    // ---ここでスケールを小さくするフラグをONにする
                    f.GetComponent<Fiary_Move>().BigStart();
                }

                //(仮SE)
                SoundManager.Instance.PlaySeByName("SE_MenuOperation");
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    //切っているかどうかのフラグをゲットするための関数
    public bool GetBreakFlg()
    {
        return bDivide;
    }


    private void UpdatePage()
    {
        papers.Clear();

        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

        // 紙の番号、昇順
        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
    }


    // 奥の紙が破り中かどうか
    public bool CheckNextPaperDividing(int currentPaperNum)
    {
        for (int paperNum = currentPaperNum + 1; paperNum <= maxPaper; paperNum++)
        {
            // 奥の紙が1枚でも破り途中ならtrue
            if (papers[paperNum].GetComponent<DivideTriangle>().Dividing == true)
            {
                Debug.Log(paperNum);
                return true;
            }
        }
        return false;
    }
}