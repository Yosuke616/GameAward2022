/*
 2022/3/19 ShimizuYosuke 
 GridStage似合ったものを移植させるやつ

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGridScript : MonoBehaviour
{
    //グリッド配置用の列挙隊宣言
    public enum e_Face
    {
        xy,
        zx,
        yz,
    }

    // false:カメラに写っている範囲を横幅、高さとする, true, 紙の横幅、高さ
    public bool paperGrid = false;

    // マスの数
    static public int horizon = 50;
    static public int virtical = 40;
    //グリッド1マスごとの大きさ
    static public float gridSizeX;
    static public float gridSizeY;
    // 紙のグリッドのサイズ
    static public float paperGridSizeX;
    static public float paperGridSizeY;

    // グリッド表示フラグ
    public bool visible = true;
    // 描画範囲を決めるカメラ
    public Camera dispCamera;

    //色の設定白色だよ
    public Color color = Color.white;
    //どの方向に見せるようにするか（今回はxy平面）
    public e_Face face = e_Face.xy;
    //何かの真偽の判定(分かり次第記入)
    public bool back = true;

    // カメラに写っている横幅、高さ
    [SerializeField] private float worldWidth;
    [SerializeField] private float worldHeight;

    //更新検出用
    float preGridSize = 0;
    int preSizeX = 0;
    int preSizeY = 0;
    Color preColor = Color.red;
    e_Face preFace = e_Face.zx;
    bool preBack = true;

    //メッシュ用の変数
    Mesh mesh;


    // Start is called before the first frame update
    void Awake()
    {
        if(paperGrid == false)
        {
            // カメラに写っている範囲を計算する
            Vector3 rightTop = dispCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
            Vector3 leftBottom = dispCamera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
            worldWidth = rightTop.x - leftBottom.x;
            worldHeight = rightTop.y - leftBottom.y;
            // グリッサイズ = 幅 / 個数
            gridSizeX = worldWidth  / horizon;
            gridSizeY = worldHeight / virtical;
        }
        else
        {
            // グリッサイズ = 幅 / 個数
            paperGridSizeX = CreateTriangle.paperSizeX * 2 / horizon;
            paperGridSizeY = CreateTriangle.paperSizeY * 2 / virtical;
        }


        //グリッド作成
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh = ReGrid(mesh);

        // 描画するかしないか
        if (!visible)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    //グリッドの為の関数
    private Mesh ReGrid(Mesh mesh)
    {
        if (back)
        {
            //デフォルトの表示
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }
        else
        {
            //変更がかかったとき用の表示
            GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        }

        //一回メッシュの情報を削除する
        mesh.Clear();

        

        

        //横に何個ある大きさか
        float width, height;
        //頂点数
        int resolutionX;    // 縦線の頂点数
        int resolutionY;    // 横線の頂点数

        // マスとマスの間隔
        float diffX, diffY;

        // 頂点情報
        Vector3[] vertices;
        Vector2[] uvs;
        int[] lines;
        Color[] colors;

        //描画の開始座標を決める
        if (paperGrid == false)
        {
            width  = gridSizeX * horizon * 0.5f;
            height = gridSizeY * virtical * 0.5f;

            //描画の間隔
            diffX = gridSizeX / 4.0f;
            diffY = gridSizeY / 4.0f;
        }
        else
        {
            width  = paperGridSizeX * horizon * 0.5f;
            height = paperGridSizeY * virtical * 0.5f;

            //描画の間隔
            diffX = paperGridSizeX / 4.0f;
            diffY = paperGridSizeY / 4.0f;
        }
        Vector2 startPosition = new Vector2(-width, -height);
        Vector2 endPosition = new Vector2(width, height);

        

        //頂点数を決める（本数 × 2)
        resolutionX = (horizon + 1) * 2;
        resolutionY = (virtical + 1) * 2;

        //頂点数だけ要素を確保
        vertices = new Vector3[resolutionX + resolutionY];  //座標
        uvs = new Vector2[resolutionX + resolutionY];       //uv
        lines = new int[resolutionX + resolutionY];         //頂点のつなぎ方
        colors = new Color[resolutionX + resolutionY];      //色

        // 頂点座標
        for (int i = 0; i < resolutionX; i += 2)
        {
            // 縦線
            vertices[i]     = new Vector3(startPosition.x + (diffX * (float)i * 2), startPosition.y, 0);
            vertices[i + 1] = new Vector3(startPosition.x + (diffX * (float)i * 2), endPosition.y, 0);
        }

        for (int i = 0; i < vertices.Length - resolutionX; i += 2)
        {
            // 横線
            vertices[resolutionX + i]     = new Vector3(startPosition.x, endPosition.y - (diffY * (float)i * 2), 0);
            vertices[resolutionX + i + 1] = new Vector3(endPosition.x,   endPosition.y - (diffY * (float)i * 2), 0);
        }


        //色とかの設定(見た目)
        for (int i = 0; i < resolutionX + resolutionY; i++)
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
        preGridSize = gridSizeX;
        preSizeX = horizon;
        preSizeY = virtical;
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
