/*
 2022/3/18 ShimizuYosuke 
 画面いっぱいに当たり判定のある透明四角いオブジェクトを生成する(横400、縦300)

    各々の紙のあたり判定をこのスクリプトで確認して、
    CollisionField.csに情報を送る

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
    [SerializeField] private List<StageBlock> collisionGrid = new List<StageBlock>();
    // 全マスのオブジェクト
    [SerializeField] private List<GameObject> Grids = new List<GameObject>();
    // どのカメラの前にグリッドを表示させるか
    [SerializeField] string cameraName = "MainCamera";
    // 描画開始位置
    private Vector2 StartPoint;

    // 紙の番号
    public int layer = 0;

    //public float z = 0.0f;

    public GameObject MainGrid;
    //縦の数と横の数を設定する為の変数
    private int gridNumX;
    private int gridNumY;
    // グリッドの横幅と高さ
    private float GridSizeX;
    private float GridSizeY;

    //void Start()
    //{
    //    // マスの横・縦の数
    //    gridNumX = CreateGridScript.horizon;
    //    gridNumY = CreateGridScript.virtical;
    //
    //    //名前を変えるためにカウントを作る
    //    int nNameCnt = 0;
    //
    //    for (int i = 0; i < gridNumY; i++)
    //    {
    //        for (int u = 0; u < gridNumX; u++, nNameCnt++)
    //        {
    //            // 空のブロックを追加しておく
    //            collisionGrid.Add(new StageBlock());
    //        }
    //    }
    //
    //    RayToGrid();
    //}


    //public void RayToGrid()
    //{
    //    // マスの横・縦の数
    //    float gridSizeX = CreateGridScript.gridSizeX;
    //    float gridSizeY = CreateGridScript.gridSizeY;
    //    int gridNumX = CreateGridScript.horizon;
    //    int gridNumY = CreateGridScript.virtical;
    //
    //    Vector2 start;
    //    // rayを飛ばす座標
    //    start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
    //    start.y = gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);
    //
    //
    //    // カメラを見つける
    //    GameObject cameraObj = GameObject.Find(cameraName);
    //
    //    int cnt = 0;
    //
    //    for (int y = 0; y < gridNumY; y++)
    //        for (int x = 0; x < gridNumX; x++)
    //        {
    //            RaycastHit hit;
    //            if (Physics.Raycast(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), z), out hit))
    //            {
    //                // 衝突処理の種類をセットする
    //                collisionGrid[(y * gridNumX) + x].tag = hit.collider.tag;
    //                Debug.Log(collisionGrid[(y * gridNumX) + x].tag);
    //
    //                // 回転角もセットする
    //                collisionGrid[(y * gridNumX) + x].rotate = hit.collider.transform.localEulerAngles;
    //
    //                cnt++;
    //            }
    //
    //            Debug.DrawRay(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), z));
    //            Debug.LogError("");
    //        }
    //    Debug.LogWarning(cnt);
    //
    //    // CollisionSystemのマスに反映させる
    //    CollisionField.Instance.AddStageInfo(layer, collisionGrid);
    //
    //    // 1枚目の神だった場合、あたり判定として設定する
    //    if (layer == 0) CollisionField.Instance.SetStage(layer);
    //}

    // 初期化
    void Start()
    {
        // 1マスの大きさ
        GridSizeX = CreateGridScript.gridSizeX;
        GridSizeY = CreateGridScript.gridSizeY;
        // マスの数
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
                Grids.Add(mass);

                // 空のブロックを追加しておく
                collisionGrid.Add(new StageBlock());
            }
        }

        // 一瞬時間を置いてDelayMethod()を呼ぶ
        Invoke("DelayMethod", 0.001f);
    }

    //Start()で生成したオブジェクトのOnCollisionTriggerが呼ばれた後にこのメソッドを呼びたい

    private void DelayMethod()
    {
        for (int i = 0; i < Grids.Count; i++)
        {
            if (Grids[i] == null) continue;

            // 衝突処理の種類をセットする
            collisionGrid[i].tag = Grids[i].tag;
            //Debug.Log(collisionGrid[i].tag);

            // 回転角もセットする
            collisionGrid[i].rotate = Grids[i].transform.localEulerAngles;

            // オリジナルのオブジェクト
            collisionGrid[i].sourceObject = Grids[i].GetComponent<collsion_test>().getOriginalObject();

            // あたり判定確認用オブジェクトを削除
            Destroy(Grids[i]);
        }

        Grids.Clear();

        // CollisionSystemのマスに反映させる
        CollisionField.Instance.AddStageInfo(layer, collisionGrid);

        // layerが0だったらセットする
        if (layer == 0)
        {
            CollisionField.Instance.SetStage(layer);
        }
    }

    // プレイヤーが敗れた紙側にあるかどうか
    static public bool CheckPlayerSideOfThePaper(string FindTag, List<bool> changes, GameObject cameraObject)
    {
        // マスの横・縦の数
        float gridSizeX = CreateGridScript.gridSizeX;
        float gridSizeY = CreateGridScript.gridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        Vector2 start;

        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++)
            {
                // 変更があるマスだけRayCast
                if (changes[y * gridNumX + x])
                {
                    // rayを飛ばす座標
                    start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
                    start.y = gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);

                    RaycastHit hit;
                    if (Physics.Raycast(cameraObject.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f), out hit))
                    {
                        if (hit.collider.tag == FindTag) return true;
                    }

                    //Debug.DrawRay(Cameraobj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f));
                    //Debug.LogError("");
                }
            }
        }

        return false;
    }


    // Quad作成
    static public GameObject CreateMesh(float GridSizeX, float GridSizeY, Material[] mats)
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();   // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 頂点座標
        vertices1.Add(new Vector3(-GridSizeX * 0.5f, GridSizeY * 0.5f, 0.0f));  // 左上
        vertices1.Add(new Vector3(GridSizeX * 0.5f, GridSizeY * 0.5f, 0.0f));  // 右上
        vertices1.Add(new Vector3(GridSizeX * 0.5f, -GridSizeY * 0.5f, 0.0f));  // 右下
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
    public List<StageBlock> GetStageInfo()
    {
        return collisionGrid;
    }
}
