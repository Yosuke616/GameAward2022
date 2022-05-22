/*
 2022/3/26 ShimizuYosuke 
 リストから外周を得てマウスから中心の線分の交点を求める
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script_Second : MonoBehaviour
{
    //リストを保存するための変数
    [SerializeField] private List<Vector3> OutLinePaper = new List<Vector3>();

    //一回だけクリックしたときにそこで止まるためのフラグ
    [SerializeField] private bool First_Flg;
    //最初の一回だけ外部から読み込めるようにする(tureで読み込めなくする)
    private bool g_bFirst_Load;

    //交点の座標を保存する変数
    [SerializeField] private Vector2 Cross_Pos;

    //紙の中心座標は常に一体なのでここでセットする
    private Vector2 Paper_Center;

    //クリックした場所を保存する変数
    private Vector3 Old_Click_Pos;

    //最終的に決める座標
    private Vector3 Pos;

    //紙の外周を保存するための変数
    private Vector3 OutPaper_Pos;

    //中央に行かないようにするための変数
    private Vector3 Old_Pos;


    void Start()
    {
        //紙の中心座標をセットする
        Paper_Center = new Vector2(0.0f, 0.0f);

        //初期化するたびにfalseにして読み込めるようにする
        g_bFirst_Load = false;

        //紙の外周用の変数を初期化する
        OutPaper_Pos = new Vector3(0.0f, 0.0f, 0.0f);

        //最初のフラグはfalseになっている(trueで一回目は終了)
        First_Flg = false;

        OutLinePaper.Add(new Vector3(-CreateTriangle.paperSizeX - 0.05f, CreateTriangle.paperSizeY + 0.05f, 0.0f));  // 左上
        OutLinePaper.Add(new Vector3(CreateTriangle.paperSizeX + 0.05f, CreateTriangle.paperSizeY + 0.05f, 0.0f));  // 右上
        OutLinePaper.Add(new Vector3(CreateTriangle.paperSizeX + 0.05f, -CreateTriangle.paperSizeY - 0.05f, 0.0f));  // 右下
        OutLinePaper.Add(new Vector3(-CreateTriangle.paperSizeX - 0.05f, -CreateTriangle.paperSizeY - 0.05f, 0.0f));  // 左下

    }

    // Update is called once per frame
    void Update()
    {
        //これで問題ないはず
        if (Camera.main == null) { return; }

        // コリジョンカメラに映っているプレイヤーを取得
        GameObject player = GameObject.Find("ParentPlayer");

        if (player.GetComponent<PlayerMove2>().GetFlg() && // プレイヤーが動ける状態の場合
            player.GetComponent<PlayerMove2>().GetGameOverFlg())
        {
            //マウス座標をここでゲットします
            Vector2 Mouse_Pos = Input.mousePosition;
            //画面の半分の大きさをマイナスすることにより整合性を図るぜ
            Mouse_Pos.x -= Screen.width / 2;
            Mouse_Pos.y -= Screen.height / 2;

            //Old_Click_Pos = Input.mousePosition;
            //Old_Click_Pos.z = 10.0f;
            //var Pos2 = Camera.main.ScreenToWorldPoint(Old_Click_Pos);

            //一回クリックされたらそこでカーソルは止まる
            if (!First_Flg)
            {
                // 接続されているコントローラの名前を調べる
                var controllerNames = Input.GetJoystickNames();
                // 一台もコントローラが接続されていなければマウスで
                if (controllerNames.Length == 0)
                {
                    // マウス
                    Cross_Pos = calcCrossPos_Mouse(Mouse_Pos, Paper_Center, OutLinePaper);
                }
                else
                {
                    // ゲームパッド
                    Cross_Pos = calcCrossPos_GamePad(Mouse_Pos, Paper_Center, OutLinePaper);
                }

                //カーソルをセットした座標に移動させる
                this.transform.position = Cross_Pos;

                OutPaper_Pos = Cross_Pos;

            }
            else
            {
                this.transform.position = Cross_Pos;
            }

            #region --- 中央に行かないようにする処理
            if (!(this.transform.position == new Vector3(0.0f, 0.0f, 0.0f)))
            {
                Old_Pos = this.transform.position;
            }
            if (this.transform.position == new Vector3(0.0f, 0.0f, 0.0f))
            {
                this.transform.position = Old_Pos;
            }
            #endregion
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    //線分と線分の計算をするための関数
    //戻り値:計算して出した二次元ベクトル
    //引数  :マウスの座標と紙の中心の座標と紙の4辺の情報が入ったリスト
    private Vector2 calcCrossPos_Mouse(Vector2 Mouse, Vector2 Center, List<Vector3> Square)
    {
        //最後に返すときのための2次元ベクトル
        Vector2 CrossVector = new Vector2(0.0f, 0.0f);

        //マウスと中心が重なっていたら速攻で戻り値(0,0)を返す
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0))
        {
            return CrossVector;
        }

        //4つの辺農地どれかが交わるまで繰り返す
        for (int i = 0; i < Square.Count; i++)
        {
            //順番に外周を使っていく
            //ここで外周の長さが出るがこれは使わない
            //この考え方事態は必要
            Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //振る番号を作る
            int VegetaNum = ((i + 1) % (Square.Count));

            //割れる用の数字を用意する
            float Num = (Mouse.x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Mouse.y - Center.y) * (Square[VegetaNum].x - Square[i].x);

            //縦と横の計算をする
            float X = ((Square[i].x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Square[i].y - Center.y) * (Square[VegetaNum].x - Square[i].x)) / Num;
            float Y = ((Square[i].x - Center.x) * (Mouse.y - Center.y) - (Square[i].y - Center.y) * (Mouse.x - Center.x)) / Num;

            //範囲外だったら次の数字に行く
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f)
            {
                continue;
            }

            //最終調整
            CrossVector.x = Center.x + X * (Mouse.x - Center.x);
            CrossVector.y = Center.y + X * (Mouse.y - Center.y);


            //Debug.Log(CrossVector.x);
            //Debug.Log(CrossVector.y);

        }


        //最終的に出来た値を返す
        return CrossVector;
    }

    private Vector2 calcCrossPos_GamePad(Vector2 Mouse, Vector2 Center, List<Vector3> Square)
    {
        //コントローラーのやつ
        GameObject CTRL = GameObject.Find("CTRLCur");

        Vector3 ctrl = CTRL.GetComponent<CTRLCur>().GetCTRLPos();

        Vector2 mmm;

        mmm.x = ctrl.x;
        mmm.y = ctrl.y;

        Mouse = mmm;

        Mouse *= 100;

        //最後に返すときのための2次元ベクトル
        Vector2 CrossVector = new Vector2(0.0f, 0.0f);

        //マウスと中心が重なっていたら速攻で戻り値(0,0)を返す
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0))
        {
            return CrossVector;
        }

        //4つの辺農地どれかが交わるまで繰り返す
        for (int i = 0; i < Square.Count; i++)
        {
            //順番に外周を使っていく
            //ここで外周の長さが出るがこれは使わない
            //この考え方事態は必要
            Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //振る番号を作る
            int VegetaNum = ((i + 1) % (Square.Count));

            //割れる用の数字を用意する
            float Num = (Mouse.x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Mouse.y - Center.y) * (Square[VegetaNum].x - Square[i].x);

            //縦と横の計算をする
            float X = ((Square[i].x - Center.x) * (Square[VegetaNum].y - Square[i].y) - (Square[i].y - Center.y) * (Square[VegetaNum].x - Square[i].x)) / Num;
            float Y = ((Square[i].x - Center.x) * (Mouse.y - Center.y) - (Square[i].y - Center.y) * (Mouse.x - Center.x)) / Num;

            //範囲外だったら次の数字に行く
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f)
            {
                continue;
            }

            //最終調整
            CrossVector.x = Center.x + X * (Mouse.x - Center.x);
            CrossVector.y = Center.y + X * (Mouse.y - Center.y);


            //Debug.Log(CrossVector.x);
            //Debug.Log(CrossVector.y);

        }


        //最終的に出来た値を返す
        return CrossVector;
    }


    //妖精の場所を変更するためのゲッター
    public Vector2 GetCursorPos()
    {
        return Cross_Pos;
    }

    //中に入ったカーソルの場所を送るための関数
    public Vector3 GetCursorPoss_IN()
    {
        return this.transform.position;
    }

    //アウトラインのリストを更新する為の関数
    //public void SetMoveLine(List<Vector3> Line, Vector3 Center)
    //{
    //    //リストの更新をする
    //    OutLinePaper = Line;
    //
    //    //中心座標を更新する
    //    Paper_Center = Center;
    //}

    //破る時にカーソルをそこに移動させる
    private void CursorBreak()
    {
        GameObject TPA = GameObject.Find("CTRLCur");

        this.transform.position = TPA.transform.position;
    }

    //一回目かどうかのフラグを送るための関数
    public bool GetFirstFlg()
    {
        return First_Flg;
    }

    public void DivideStart()
    {
        First_Flg = true;
    }
    public void DivideEnd()
    {
        First_Flg = false;
    }

}
