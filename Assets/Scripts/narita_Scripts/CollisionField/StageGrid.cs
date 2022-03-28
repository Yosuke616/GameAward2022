/*
 2022/3/18 ShimizuYosuke 
 画面いっぱいに当たり判定のある透明四角いオブジェクトを生成する(横400、縦300)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CreateGridScript))]
public class StageGrid : MonoBehaviour
{
    //色配置するマテリアルの設定
    public Material[] ColorSet = new Material[2];

    // 全マスのあたり判定の種類
    [SerializeField] private List<string>   collisionGrid = new List<string>();
    // 全マスのオブジェクト
    [SerializeField] private List<GameObject> Grid = new List<GameObject>();
    // どのカメラの前にグリッドを表示させるか
    [SerializeField] string cameraName = "MainCamera";
    // 描画開始位置
    private Vector2 StartPoint;

    // 紙の番号
    public int layer = 0;

    public GameObject MainGrid;
    //縦の数と横の数を設定する為の変数
    private int gridNumX;
    private int gridNumY;
    // グリッドの横幅と高さ
    private float GridSizeX;
    private float GridSizeY;



    // 初期化
    void Start()
    {
        GridSizeX = CreateGridScript.gridSizeX;
        GridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;

        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject Cameraobj = GameObject.Find(cameraName);

        //名前を変えるためにカウントを作る
        int nNameCnt = 0;

        // 描画開始位置 = カメラ座標 - gridの横幅 * 数の半分
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);

        // マスごとにあたり判定を取る用のオブジェクトを生成
        for (int i = 0; i < gridNumY; i++)
        {
            for (int u = 0; u < gridNumX; u++, nNameCnt++)
            {
                //スクリプトでオブジェクトを追加する
                GameObject mass = CreateMesh(GridSizeX, GridSizeY, ColorSet); 

                //親と子の設定をする(ここではｶﾒﾗを親にする)
                mass.transform.SetParent(Cameraobj.transform);

                //---オブジェクトの設定
                // 座標
                mass.transform.position = new Vector3(
                    StartPoint.x + (GridSizeX * u),
                    StartPoint.y - (GridSizeY * i),
                    transform.position.z);

                //名前の設定(最後のiは何行目にあるか)
                mass.name = "Grid_No." + nNameCnt + ":" + i;

                //色の設定(初期値は赤)
                mass.GetComponent<MeshRenderer>().material = ColorSet[1];

                //マウスの当たり判定用のコンポーネントを追加する
                mass.AddComponent<BoxCollider>();
                var Coll = mass.GetComponent<BoxCollider>();
                Coll.enabled = true;

                // 当たった時どのようなふるまいをするか
                mass.AddComponent<collsion_test>();

                mass.GetComponent<BoxCollider>().isTrigger = true;

                //タグを付ける
                mass.tag = "none";

                // オブジェクトリストに追加
                Grid.Add(mass);
                // タグを追加
                collisionGrid.Add(mass.tag);
            }
        }

        // 一瞬時間を置いてDelayMethod()を呼ぶ
        Invoke("DelayMethod", 0.001f);
    }

    // 少し遅らせて実行したい
    private void DelayMethod()
    {
        for (int i = 0; i < Grid.Count; i++)
        {
            if (Grid[i] == null) continue;

            // 衝突処理の種類をセットする
            collisionGrid[i] = Grid[i].tag;

            Destroy(Grid[i]);
        }

        Grid.Clear();

        // CollisionSystemのマスに反映させる
        CollisionField.Instance.AddStageInfo(layer, collisionGrid);
        // layerが0だったらセットする
        if(layer == 0)
        {
            CollisionField.Instance.SetStage(layer);
        }
    }


   

    // Quad作成
    static public GameObject CreateMesh(float GridSizeX, float GridSizeY, Material[] mats)
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();   // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 頂点座標
        vertices1.Add(new Vector3(-GridSizeX * 0.5f,  GridSizeY * 0.5f, 0.0f));  // 左上
        vertices1.Add(new Vector3( GridSizeX * 0.5f,  GridSizeY * 0.5f, 0.0f));  // 右上
        vertices1.Add(new Vector3( GridSizeX * 0.5f, -GridSizeY * 0.5f, 0.0f));  // 右下
        vertices1.Add(new Vector3(-GridSizeX * 0.5f, -GridSizeY * 0.5f, 0.0f));  // 左下
        // uv
        uvs1.Add(new Vector2(0, 1));
        uvs1.Add(new Vector2(1, 1));
        uvs1.Add(new Vector2(1, 0));
        uvs1.Add(new Vector2(0, 0));
        // 法線
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        // 頂点インデックス
        triangles1.Add(0);
        triangles1.Add(1);
        triangles1.Add(3);
        triangles1.Add(1);
        triangles1.Add(2);
        triangles1.Add(3);

        //カット後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("colliderBlock",
            typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();    // 頂点情報
        mesh.triangles = triangles1.ToArray();  // 頂点の数
        mesh.uv = uvs1.ToArray();               // uv
        mesh.normals = normals1.ToArray();      // 法線
        obj.GetComponent<MeshRenderer>().materials = mats;
        obj.GetComponent<MeshFilter>().mesh = mesh;                // メッシュフィルターにメッシュをセット
        //obj.GetComponent<MeshCollider>().sharedMesh = mesh;        // メッシュコライダーにメッシュをセット 

        return obj;
    }

    // getter
    public List<string> GetStageInfo()
    {
        return collisionGrid;
    }
}
