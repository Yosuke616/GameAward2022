using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    // Start is called before the first frame update
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
        Continue_B.Add(new Vector3(-5.0f,-3.9f,-0.1f));
        Continue_B.Add(new Vector3(-1.3f,-3.4f,-0.1f));
        Option_B.Add(new Vector3(1.25f,-3.9f,-0.1f));
        Option_B.Add(new Vector3(4.95f,-3.4f,-0.1f));
        End_B.Add(new Vector3(4.95f,-2.25f, -0.2f));
        End_B.Add(new Vector3(8.25f,-1.75f, -0.2f));

        first_Flg = false;


        //1枚目かどうかのフラグをオフにする
        bFirdt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Titleflg)
        {
            nCnt--;

            if (nCnt < 0)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") < 0)
                {
                    //GameObject delete = GameObject.Find("breaking paper line");

                    //スイッチ文で子オブジェクトを消す
                    switch (nSelectButton) {
                        case 0:
                            int nChildCnt = StartButton.transform.childCount;
                            if (nChildCnt < 0) {
                                break;
                            }
                            for (int x = 0;x < nChildCnt; x++) {
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
                    if (!first_Flg) {
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

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1"))
            {
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

    //初めから
    public void OnFirst()
    {
        Debug.Log("ロゼッター");
        Cre_Mesh();
        // 同一シーンを読込
        //SceneManager.LoadScene("StageSelect");
    }

    //続きから
    public void OnCountinue()
    {
        Debug.Log("ロゼッタかわいいー");
        Cre_Mesh();
        // 同一シーンを読込
        //SceneManager.LoadScene("StageSelect");
    }

    //オプション
    public void OnOption()
    {
        Debug.Log("ロゼッタ様抱いてー！");
        Cre_Mesh();

        Titleflg = true;
        Option.SetActive(true);
        Plate.SetActive(true);
        GameObject obj = GameObject.Find("OptionTitle");
        obj.GetComponent<TitleOption_Script>().SetTitleOption(false);
    }

    //終わる
    public void OnEnd()
    {
        Cre_Mesh();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
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
               
                //左側
                Start.Add(new Vector3(-8.0f,-2.25f,-0.1f));
                Start.Add(new Vector3(-8.0f,-1.75f,-0.1f));
                Start.Add(new Vector3(-5.0f,-1.75f,-0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f,0.0f);
                _Vector2[1] = new Vector2(0.0f,1.0f);
                _Vector2[2] = new Vector2(1.0f,1.0f);

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Start);

                //リストの中身を削除する
                Start.Clear();

                //右側を作る
                Start.Add(new Vector3(-8.0f, -2.25f, -0.1f));
                Start.Add(new Vector3(-5.0f, -1.75f, -0.1f));
                Start.Add(new Vector3(-5.0f, -2.25f, -0.1f));

                //UVの設定
                _Vector2[0] = new Vector2(0.0f, 0.0f);
                _Vector2[1] = new Vector2(1.0f, 1.0f);
                _Vector2[2] = new Vector2(1.0f, 0.0f);

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Start);

                break;
            //続きから
            case 1:
                //続きからを消す
                ContinueButton.SetActive(false);

                List<Vector3> Continue = new List<Vector3>();

                //左側
                Continue.Add(new Vector3(-5.0f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-1.3f,-3.4f,-0.1f));
                Continue.Add(new Vector3(-5.0f,-3.9f,-0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Continue);

                //リストの中身を削除する
                Continue.Clear();

                //右側
                Continue.Add(new Vector3(-1.3f, -3.4f, -0.1f));
                Continue.Add(new Vector3(-1.3f, -3.9f, -0.1f));
                Continue.Add(new Vector3(-5.0f, -3.9f, -0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Continue);

                break;
            case 2:
                //オプションを消す
                OptionButton.SetActive(false);

                List<Vector3> Option = new List<Vector3>();

                //左側
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Option);

                //リストの中身を削除する
                Option.Clear();

                //右側
                Option.Add(new Vector3(4.95f, -3.4f, -0.1f));
                Option.Add(new Vector3(4.95f, -3.9f, -0.1f));
                Option.Add(new Vector3(1.25f, -3.9f, -0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(Option);

                break;
            case 3:
                //終わるを消す
                EndButton.SetActive(false);

                List<Vector3> End = new List<Vector3>();

                //左側
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(End);

                //リストの中身を削除する
                End.Clear();

                //右側
                End.Add(new Vector3(8.25f, -1.75f, -0.1f));
                End.Add(new Vector3(8.25f, -2.25f, -0.1f));
                End.Add(new Vector3(4.95f, -2.25f, -0.1f));

                //メッシュを作る
                obj.GetComponent<DrawMesh>().CreateMesh(End);
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
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[0];
                }
                break;
            case 1:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[1];
                }
                break;
            case 2:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[2];
                }
                break;
            case 3:
                if (!bFirdt)
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                    bFirdt = true;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = _Mats[3];
                }
                break;
        }

    }

    //UV情報を送る
    public Vector2[] GetUV()
    {
        return _Vector2;
    }

}
