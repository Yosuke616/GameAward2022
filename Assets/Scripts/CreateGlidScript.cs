/*
 2022/3/19 ShimizuYosuke 
 GlidStage似合ったものを移植させるやつ

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGlidScript : MonoBehaviour
{
    //==============================
    //グリッド配置用の列挙隊宣言
    public enum e_Face
    {
        xy,
        zx,
        yz,
    }

    //グリッド1マスごとの大きさ
    public float gridSize = 1f;
    //これも大きさかな？よくわからぬ
    public int size = 8;
    //色の設定白色だよ
    public Color color = Color.white;
    //どの方向に見せるようにするか（今回はxy平面）
    public e_Face face = e_Face.xy;
    //何かの真偽の判定(分かり次第記入)
    public bool back = true;

    //更新検出用
    float preGridSize = 0;
    int preSize = 0;
    Color preColor = Color.red;
    e_Face preFace = e_Face.zx;
    bool preBack = true;

    //メッシュ用の変数
    Mesh mesh;

    //==============================

    // Start is called before the first frame update
    void Start()
    {
        //==========================
        //グリッド用の初期化
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        //メッシュの変更をするための関数
        mesh = ReGrid(mesh);
        //==========================
    }

    // Update is called once per frame
    void Update()
    {
        //グリッドのためのやつ
        //関係値の更新を検出したらメッシュも更新
        if (gridSize != preGridSize || size != preSize || preColor != color || preFace != face || preBack != back)
        {
            if (gridSize < 0) { gridSize = 0.000001f; }
            if (size < 0) { size = 1; }
            ReGrid(mesh);
        }
    }


    //グリッドの為の関数
    private Mesh ReGrid(Mesh mesh)
    {
        if (back)
        {
            //デフォルトの表示
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        }
        else
        {
            //変更がかかったとき用の表示
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }

        //一回メッシュの情報を削除する
        mesh.Clear();

        //メッシュ作成するに当たって必要になってくる変数たち
        //描画時の大きさ
        int drawSize;
        //横に何個ある大きさか
        float width;
        //解像度の設定
        int resolution;
        //謎なので後書く
        float diff;
        //頂点情報
        Vector3[] vertices;
        //多分テクスチャ的な奴
        Vector2[] uvs;
        //線のやつ
        int[] lines;
        //色
        Color[] colors;

        //大きさを決定
        drawSize = size * 2;
        //横の数の決定
        width = gridSize * drawSize / 4.0f;
        //開始座標を決める
        Vector2 startPosition = new Vector2(-width, -width);
        //終了場所を決める
        Vector2 endPosition = new Vector2(width, width);
        //相変わらずよくわからん
        diff = width / drawSize;
        //どのくらいきれいにするか
        resolution = (drawSize + 2) * 2;
        //最期の２辺を追加している

        //頂点の情報をセットする
        vertices = new Vector3[resolution];
        //テクスチャ情報のセット
        uvs = new Vector2[resolution];
        //線がドっからどこまで引くか
        lines = new int[resolution];
        //線の色をセット
        colors = new Color[resolution];

        //編集した情報を設定している(頂点情報とか)
        for (int i = 0; i < vertices.Length; i += 4)
        {
            vertices[i] = new Vector3(startPosition.x + (diff * (float)i), startPosition.y, 0);
            vertices[i + 1] = new Vector3(startPosition.x + (diff * (float)i), endPosition.y, 0);
            vertices[i + 2] = new Vector3(startPosition.x, endPosition.y - (diff * (float)i), 0);
            vertices[i + 3] = new Vector3(endPosition.x, endPosition.y - (diff * (float)i), 0);
        }

        //色とかの設定(見た目)
        for (int i = 0; i < resolution; i++)
        {
            uvs[i] = Vector2.zero;
            lines[i] = i;
            colors[i] = color;
        }

        //回転情報を設定する
        Vector3 rotDirection;
        //どっち向いているかによって変えるよ
        switch (face)
        {
            case e_Face.xy:
                rotDirection = Vector3.forward;
                break;
            case e_Face.zx:
                rotDirection = Vector3.up;
                break;
            case e_Face.yz:
                rotDirection = Vector3.right;
                break;
            default:
                rotDirection = Vector3.forward;
                break;
        }

        //回転を適応させるための関数
        mesh.vertices = RotationVertices(vertices, rotDirection);
        //テクスチャ情報を適応させる
        mesh.uv = uvs;
        //色を反映させる
        mesh.colors = colors;
        //何かをセットしている
        mesh.SetIndices(lines, MeshTopology.Lines, 0);

        ///よくわからない変更軍
        preGridSize = gridSize;
        preSize = size;
        preColor = color;
        preFace = face;
        preBack = back;

        return mesh;
    }


    //グリッドを回転させるための関数
    private Vector3[] RotationVertices(Vector3[] vertices, Vector3 rotDirection)
    {
        Vector3[] ret = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            ret[i] = Quaternion.LookRotation(rotDirection) * vertices[i];
        }
        return ret;
    }
}
