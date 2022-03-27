/*
 2022/3/26 ShimizuYosuke 
 リストから外周を得てマウスから中心の線分の交点を求める
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSide_Paper_Script_Second : MonoBehaviour
{
    //リストの情報を保存するためのゲームオブジェクトを保存する変数
    GameObject PaperObject;

    //アウトラインのスクリプトを持ってくる
    OutLinePath outlinepath_script;

    //リストを保存するための変数
    private List<Vector3> OutLinePaper = new List<Vector3>();

    //最初の一回だけ外部から読み込めるようにする(tureで読み込めなくする)
    private bool g_bFirst_Load;

    //交点の座標を保存する変数
    public Vector2 Cross_Pos;

    //紙の中心座標は常に一体なのでここでセットする
    //紙の中心の座標をセットする
    Vector2 Paper_Center;

    // Start is called before the first frame update
    void Start()
    {
        //紙の中心座標をセットする
        Paper_Center = new Vector2(0.0f, 0.0f);

        //初期化するたびにfalseにして読み込めるようにする
        g_bFirst_Load = false;

    }

    // Update is called once per frame
    void Update()
    {
        //一回だけ読み込めるようにするやつ
        if (!g_bFirst_Load) {
            //アウトラインの情報を持つオブジェクトを探してきてリストの情報を貰う
            //出来なかったらこっちでやろう
            //紙のオブジェクトを見つけてくる
            PaperObject = GameObject.Find("paper");
            //スクリプトをゲットする
            outlinepath_script = PaperObject.GetComponent<OutLinePath>();

            //リストを代入する
            OutLinePaper = outlinepath_script.OutLineVertices;

            //最初の一回だけ読み込めるようにする
            g_bFirst_Load = true;
        }

        //マウス座標をここでゲットします
        Vector2 Mouse_Pos = Input.mousePosition;

        //画面の半分の大きさをマイナスすることにより整合性を図るぜ
        Mouse_Pos.x -= Screen.width/2;
        Mouse_Pos.y -= Screen.height/2;

        //マウスと中心とそれぞれどの辺かを考える(関数を作って) 
        Cross_Pos = CalculationVector(Mouse_Pos,Paper_Center, OutLinePaper);
        
        //カーソルをセットした座標に移動させる
        this.transform.position = Cross_Pos;

    }

    //線分と線分の計算をするための関数
    //戻り値:計算して出した二次元ベクトル
    //引数  :マウスの座標と紙の中心の座標と紙の4辺の情報が入ったリスト
    private Vector2 CalculationVector(Vector2 Mouse,Vector2 Center,List<Vector3> Square) {
        //最後に返すときのための2次元ベクトル
        Vector2 CrossVector = new Vector2(0.0f,0.0f);

        //マウスと中心が重なっていたら速攻で戻り値(0,0)を返す
        if (((Mouse.x - Center.x) == 0) && ((Mouse.y - Center.y) == 0)){
            return CrossVector;
        }

        //4つの辺農地どれかが交わるまで繰り返す
        for (int i = 0;i < Square.Count;i++) {
            //順番に外周を使っていく
            //ここで外周の長さが出るがこれは使わない
            //この考え方事態は必要
            //Vector2 outlineEdge = Square[(i + 1) % (Square.Count)] - Square[i];

            //振る番号を作る
            int VegetaNum = ((i+1)%(Square.Count));

            //割れる用の数字を用意する
            float Num = (Mouse.x-Center.x)*(Square[VegetaNum].y-Square[i].y)-(Mouse.y-Center.y)*(Square[VegetaNum].x-Square[i].x);

            //縦と横の計算をする
            float X = ((Square[i].x-Center.x)*(Square[VegetaNum].y-Square[i].y)-(Square[i].y-Center.y)*(Square[VegetaNum].x-Square[i].x))/Num;
            float Y = ((Square[i].x-Center.x)*(Mouse.y-Center.y)-(Square[i].y-Center.y)*(Mouse.x-Center.x))/Num;

            //範囲外だったら次の数字に行く
            if (X < 0.0f || X > 1.0f || Y < 0.0f || Y > 1.0f) {
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
    public Vector2 GetCursorPos() {
        return Cross_Pos;
    }

}
