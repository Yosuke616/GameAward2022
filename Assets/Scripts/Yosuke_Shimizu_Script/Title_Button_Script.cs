using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Title_Button_Script : MonoBehaviour
{
    //ゲームオブジェクト(ボタン)を割り当てる
    public GameObject StartButton;
    public GameObject ContinueButton;
    public GameObject OptionButton;
    public GameObject EndButton;

    //ボタンの数
    private int nMaxButton = 3;
    private int nSelectButton = 0;

    //コントローラーの使用制限
    private int nCnt;

    //動かせるタイミングを決める
    private bool Titleflg;

    //破れ目を作るためのオブジェクト
    private GameObject button;

    //座標を保存する為のリスト
    private List<Vector3> Start_B = new List<Vector3>();
    private List<Vector3> Continue_B = new List<Vector3>();
    private List<Vector3> Option_B = new List<Vector3>();
    private List<Vector3> End_B = new List<Vector3>();

    private bool first_Flg;

    //オプションをつかえるようにする
    public GameObject Option;
    public GameObject Plate;

    //1枚目か2枚目かを判別するフラグ
    private bool bFirdt;

    //破るときに必要なマテリアルをあらかじめセットしておく
    public Material[] _Mats = new Material[4];

    //UVを設定して保存するための変数
    private Vector2[] _Vector2 = new Vector2[3];

    //1枚2枚を分ける演出を作る
    private GameObject Left_Paper;
    private GameObject Right_Paper;

    //演出用のフラグ
    private bool bNextStage;

    //シーン遷移用
    private bool FirstSceneMove;

    //動かせるようにするフラグ
    private bool StartFlg;

    void Start()
    {
        //コントローラーを動かす為の時間
        nCnt = 0;

        //タイトルを動かすためのフラグ
        Titleflg = false;

        //ボタンのやーつ
        button = GameObject.Find("CreatebreakManager");

        //リストの中身を決める
        Start_B.Add(new Vector3(-8.0f,-2.25f,-0.1f));
        Start_B.Add(new Vector3(-5.0f,-1.75f,-0.1f));
        Continue_B.Add(new Vector3(-4.5f,-3.9f,-0.1f));
        Continue_B.Add(new Vector3(-1.7f,-3.4f,-0.1f));
        Option_B.Add(new Vector3(1.75f,-3.9f,-0.1f));
        Option_B.Add(new Vector3(4.5f,-3.4f,-0.1f));
        End_B.Add(new Vector3(4.95f,-2.25f, -0.2f));
        End_B.Add(new Vector3(7.6f,-1.75f, -0.2f));

        first_Flg = false;


        //1枚目かどうかのフラグをオフにする
        bFirdt = false;

        //ネクステは最初はオフ
        bNextStage = false;

        FirstSceneMove = false;

        StartFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlg)
        {
            //ネクステの状況によって全ては変わる
            if (bNextStage)
            {
                SetNextScene();
                return;
            }

            if (!Titleflg)
            {
                nCnt--;

                if (nCnt < 0)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
                    {
                        //GameObject delete = GameObject.Find("breaking paper line");

                        //スイッチ文で子オブジェクトを消す
                        switch (nSelectButton)
                        {
                            case 0:
                                int nChildCnt = StartButton.transform.childCount;
                                if (nChildCnt < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt; x++)
                                {
                                    Destroy(StartButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 1:
                                int nChildCnt2 = ContinueButton.transform.childCount;
                                if (nChildCnt2 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt2; x++)
                                {
                                    Destroy(ContinueButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 2:
                                int nChildCnt3 = OptionButton.transform.childCount;
                                if (nChildCnt3 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt3; x++)
                                {
                                    Destroy(OptionButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 3:
                                int nChildCnt4 = EndButton.transform.childCount;
                                if (nChildCnt4 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt4; x++)
                                {
                                    Destroy(EndButton.transform.GetChild(x).gameObject);
                                }
                                break;
                        }

                        first_Flg = false;
                        nCnt = 45;
                        nSelectButton--;
                        if (nSelectButton < 0)
                        {
                            nSelectButton = nMaxButton;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") > 0)
                    {
                        //スイッチ文で子オブジェクトを消す
                        switch (nSelectButton)
                        {
                            case 0:
                                int nChildCnt = StartButton.transform.childCount;
                                if (nChildCnt < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt; x++)
                                {
                                    Destroy(StartButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 1:
                                int nChildCnt2 = ContinueButton.transform.childCount;
                                if (nChildCnt2 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt2; x++)
                                {
                                    Destroy(ContinueButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 2:
                                int nChildCnt3 = OptionButton.transform.childCount;
                                if (nChildCnt3 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt3; x++)
                                {
                                    Destroy(OptionButton.transform.GetChild(x).gameObject);
                                }
                                break;
                            case 3:
                                int nChildCnt4 = EndButton.transform.childCount;
                                if (nChildCnt4 < 0)
                                {
                                    break;
                                }
                                for (int x = 0; x < nChildCnt4; x++)
                                {
                                    Destroy(EndButton.transform.GetChild(x).gameObject);
                                }
                                break;
                        }

                        first_Flg = false;
                        nCnt = 45;
                        nSelectButton++;
                        if (nSelectButton > nMaxButton)
                        {
                            nSelectButton = 0;
                        }
                    }
                }

                //常に白に変えていく
                StartButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                ContinueButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                OptionButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                EndButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                switch (nSelectButton)
                {
                    case 0:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Start_B, StartButton);
                            first_Flg = true;
                        }
                        StartButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 1:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Continue_B, ContinueButton);
                            first_Flg = true;
                        }
                        ContinueButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 2:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(Option_B, OptionButton);
                            first_Flg = true;
                        }
                        OptionButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    case 3:
                        if (!first_Flg)
                        {
                            button.GetComponent<PaperBreakLineManager>().CreateBreakLine(End_B, EndButton);
                            first_Flg = true;
                        }
                        EndButton.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        break;
                    default: break;
                }

                // 決定
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
                {
                    // 点線 → 破れ線に変更
                    //List<GameObject> papers = new List<GameObject>();
                    //papers.AddRange(GameObject.Find("paper"));
                    //foreach(var papaer in papers)
                    //{
                        //for (int i = 0; i < paper.transform.childCount; i++)
                        //{
                        //    paper.transform.GetChild(i).GetComponent<LineRendererOperator>().hoge();
                        //}
                    //}

                    switch (nSelectButton)
                    {
                        case 0: OnFirst(); break;
                        case 1: OnCountinue(); break;
                        case 2: OnOption(); break;
                        case 3: OnEnd(); break;

                        default: break;
                    }
                }

            }
        }
    }

    //初めから
    public void OnFirst()
    {
        Debug.Log("ロゼッター");

        // データ初期化
        SaveLoad.InitSaveData();

        Cre_Mesh();

        

        // 同一シーンを読込
        //SceneManager.LoadScene("StageSelect");
    }

    //続きから
    public void OnCountinue()
    {
        Debug.Log("ロゼッタかわいいー");

        // データ読込
        SaveLoad.LoadData();

        Cre_Mesh();


        // 同一シーンを読込
        //SceneManager.LoadScene("StageSelect");
    }

    //オプション
    public void OnOption()
    {
        Debug.Log("ロゼッタ様抱いてー！");
        Cre_Mesh();

        //Titleflg = true;
        //Option.SetActive(true);
        //Plate.SetActive(true);
        //GameObject obj = GameObject.Find("OptionTitle");
        //obj.GetComponent<TitleOption_Script>().SetTitleOption(false);
    }

    //終わる
    public void OnEnd()
    {
        Cre_Mesh();

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//    Application.Quit();
//#endif
    }

    public void SetTitleFlg(bool TF)
    {
        Titleflg = TF;
    }

    private void Cre_Mesh() {
        //演出
        GameObject obj = GameObject.Find("CreatebreakManager");

        switch (nSelectButton) {
            //スタートボタン
            case 0:
                //スタートボタンを消す
                StartButton.SetActive(false);

                List<Vector3> Start = new List<Vector3>();
                List<Vector3> Start_Break = new List<Vector3>();
                List<Vector3> breakLine1 = new List<Vector3>();
               
                //左側
                Start.Add(new Vector3(-8.0f,-2.25f,-0.1f));
                Start.Add(new Vector3(-8.0f,-1.75f,-0.1f));
                Start.Add(new Vector3(-5.0f,-1.75f,-0.1f));

                // ブレークライン
                breakLine1.Add(new Vector3(-5.5f,-1.5f,-0.1f));
                breakLine1.Add(new Vector3(-7.2f,-2.4f,-0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f,0.0f);
                _Vector2[1] = new Vector2(0.0f,1.0f);
                _Vector2[2] = new Vector2(1.0f,1.0f);

                //メッシュを作る
                GameObject paper1 = obj.GetComponent<DrawMesh>().CreateMesh(Start);

                // ブレークライン生成
                var line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, paper1);
                line1.name = "broken paper line";
                line1.GetComponent<LineRendererOperator>().hoge();

                //リストの中身を削除する
                Start.Clear();

                //ブレイクラインのリストのリセット
                breakLine1.Clear();

                //右側を作る
                Start.Add(new Vector3(-8.0f, -2.25f, -0.1f));
                Start.Add(new Vector3(-5.0f, -1.75f, -0.1f));
                Start.Add(new Vector3(-5.0f, -2.25f, -0.1f));

                //ブレークライン
                breakLine1.Add(new Vector3(-5.5f, -1.5f, -0.1f));
                breakLine1.Add(new Vector3(-7.2f, -2.4f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //メッシュを作る
                GameObject paper2 = obj.GetComponent<DrawMesh>().CreateMesh(Start);
                paper2.tag = "paper";

                // ブレークライン生成
                line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, paper2);
                line1.name = "broken paper line";
                line1.GetComponent<LineRendererOperator>().hoge();

                break;
            //続きから
            case 1:
                //続きからを消す
                ContinueButton.SetActive(false);

                List<Vector3> Continue = new List<Vector3>();
                List<Vector3> breakLine2 = new List<Vector3>();


                //左側
                Continue.Add(new Vector3(-5.0f,-3.9f,-0.1f));
                Continue.Add(new Vector3(-5.0f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-1.3f,-3.4f,-0.1f));

                //ブレークラインの設定
                breakLine2.Add(new Vector3(-2.4f, -3.15f, -0.1f));
                breakLine2.Add(new Vector3(-3.95f, -4.1f, -0.1f));


                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //メッシュを作る
                GameObject paper3 = obj.GetComponent<DrawMesh>().CreateMesh(Continue);
                paper3.tag = "paper";

                // ブレークライン生成
                var line3 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, paper3);
                line3.name = "broken paper line";
                line3.GetComponent<LineRendererOperator>().hoge();

                //リストの中身を削除する
                Continue.Clear();


                //右側
                Continue.Add(new Vector3(-5.0f, -3.9f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.4f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.9f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //メッシュを作る
                GameObject paper4 = obj.GetComponent<DrawMesh>().CreateMesh(Continue);
                paper4.tag = "paper";

                // ブレークライン生成
                var line4 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, paper4);
                line4.name = "broken paper line";
                line4.GetComponent<LineRendererOperator>().hoge();

                break;
            case 2:
                //オプションを消す
                OptionButton.SetActive(false);

                List<Vector3> Option = new List<Vector3>();
                List<Vector3> breakLine3 = new List<Vector3>();


                //左側
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));


                //ブレークラインの設定
                breakLine3.Add(new Vector3(2.0f, -4.2f, -0.1f));
                breakLine3.Add(new Vector3(3.9f, -3.25f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //メッシュを作る
                GameObject paper5 = obj.GetComponent<DrawMesh>().CreateMesh(Option);
                paper5.tag = "paper";

                // ブレークライン生成
                var line5 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine3, paper5);
                line5.name = "broken paper line";
                line5.GetComponent<LineRendererOperator>().hoge();

                //リストの中身を削除する
                Option.Clear();

                //右側
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.9f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //メッシュを作る
                GameObject paper6 = obj.GetComponent<DrawMesh>().CreateMesh(Option);
                paper6.tag = "paper";

                // ブレークライン生成
                var line6 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine3, paper6);
                line6.name = "broken paper line";
                line6.GetComponent<LineRendererOperator>().hoge();

                break;
            case 3:
                //終わるを消す
                EndButton.SetActive(false);

                List<Vector3> End = new List<Vector3>();

                List<Vector3> breakLine4 = new List<Vector3>();

                //左側
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));

                //ブレークラインの設定
                breakLine4.Add(new Vector3(5.75f, -2.6f, -0.1f));
                breakLine4.Add(new Vector3(7.4f, -1.6f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(0.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 1.0f);

                //メッシュを作る
                GameObject paper7 = obj.GetComponent<DrawMesh>().CreateMesh(End);
                paper7.tag = "paper";

                // ブレークライン生成
                var line7 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine4, paper7);
                line7.name = "broken paper line";
                line7.GetComponent<LineRendererOperator>().hoge();

                //リストの中身を削除する
                End.Clear();

                //右側
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -2.25f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //メッシュを作る
                GameObject paper8 = obj.GetComponent<DrawMesh>().CreateMesh(End);
                paper8.tag = "paper";

                // ブレークライン生成
                var line8 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine4, paper8);
                line8.name = "broken paper line";
                line8.GetComponent<LineRendererOperator>().hoge();

                break;
        }
       
    }

    //どのボタンが選択されているかを送る関数
    public int GetSelectBUtton() {
        return nSelectButton;
    }

    //テクスチャを貼る為の関数
    public void SettingTexture(GameObject obj) {
        switch (nSelectButton)
        {
            case 0:               
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                    obj.transform.localScale = new Vector3(1.75f, 6.7f);
                    obj.transform.position = new Vector3(5.0f, 11.4f);               
                break;
            case 1:
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                    obj.transform.localScale = new Vector3(1.45f, 6.7f);
                    obj.transform.position = new Vector3(1.4f, 20.85f);
                break;
            case 2:
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                    obj.transform.localScale = new Vector3(1.75f, 6.7f);
                    obj.transform.position = new Vector3(-2.5f, 20.75f);
                break;
            case 3:
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                    obj.transform.localScale = new Vector3(1.65f, 6.7f);
                    obj.transform.position = new Vector3(-4.3f, 11.3f);
                break;
        }

        if (!bFirdt)
        {
            //左側の紙を移動させるため
            Left_Paper = obj;

            //switch (nSelectButton)
            //{
            //    case 0:
            //        List<Vector3> Start_Break = new List<Vector3>();
            //        //破り目を作る(左側)
            //        Start_Break.Add(new Vector3(-6.0f, -2.0f, -0.5f));
            //        Start_Break.Add(new Vector3(-5.5f, -1.5f, -0.5f));
            //        GameObject breaklineSt = PaperBreakLineManager.Instance.CreateBreakLine(Start_Break, obj);
            //        Debug.Log(0, breaklineSt);
            //        break;
            //    case 1:
            //        List<Vector3> Continue_Break = new List<Vector3>();
            //        //破り目を作る(左側)
            //        Continue_Break.Add(new Vector3(-5.0f, -3.9f, -0.5f));
            //        Continue_Break.Add(new Vector3(-1.3f, -3.4f, -0.5f));
            //        GameObject breaklineCo = PaperBreakLineManager.Instance.CreateBreakLine(Continue_Break, obj);
            //        Debug.Log(1, breaklineCo);
            //        break;
            //    case 2:
            //        List<Vector3> Option_Break = new List<Vector3>();
            //        //破り目を作る(左側)
            //        Option_Break.Add(new Vector3(1.25f, -3.9f, -0.2f));
            //        Option_Break.Add(new Vector3(4.95f, -3.4f, -0.2f));
            //        GameObject breaklineOp = PaperBreakLineManager.Instance.CreateBreakLine(Option_Break, obj);
            //        Debug.Log(2, breaklineOp);
            //        break;
            //    case 3:
            //        List<Vector3> End_Break = new List<Vector3>();
            //        //破り目を作る(左側)
            //        End_Break.Add(new Vector3(4.95f, -2.25f, -0.1f));
            //        End_Break.Add(new Vector3(8.25f, -1.75f, -0.1f));
            //        GameObject breaklineEn = PaperBreakLineManager.Instance.CreateBreakLine(End_Break, obj);
            //        Debug.Log(3, breaklineEn);
            //        break;

            //}

            bFirdt = true;
        }
        else {
            //右側の紙を移動させるため
            Right_Paper = obj;
            bNextStage = true;
           
        }

    }

    //UV情報を送る
    public Vector2[] GetUV()
    {
        return _Vector2;
    }

    //演出を作る
    public void SetNextScene() {

        //左上に移動する
        Left_Paper.transform.position += new Vector3(-0.01f,0.005f,0.0f) * 0.5f;

        //右下に移動させる
        Right_Paper.transform.position += new Vector3(0.01f, -0.005f, 0.0f) * 0.5f;

        if (!FirstSceneMove) {
            //フェードアウトsaseru 
            switch (nSelectButton) {
                case 0:
                    FadeManager.Instance.FadeStart("StageSelect");
                    break;
                case 1:
                    //FadeManager.Instance.FadeStart("StageSelect");
                    break;
                case 2:
                    //FadeManager.Instance.FadeStart("Title_Option");
                    break;
                case 3:
                    break;

            }

            FirstSceneMove = true;
            Titleflg = true;
        }
    }

    //タイトルを動かせるようにする
    public void SetStartFlg(bool bStart) {
        StartFlg = bStart;
    }
}
