using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageBlock
{
    public string tag = "none";
    public Vector3 rotate = Vector3.zero;
    public int type = 0;
}

public class CollisionField : SingletonMonoBehaviour<CollisionField>
{
    



    // 実際のあたり判定を行っているシーンのカメラ
    private string cameraName = "SubCamera0";

    // ステージ(紙)情報
    [SerializeField] private List<StageBlock>[] StageInfo = new List<StageBlock>[3];
    // メインカメラで見えている部分のあたり判定のリスト
    [SerializeField] private List<GameObject> CollisionGrid = new List<GameObject>();
    [SerializeField] private List<int> layerList = new List<int>();
    List<int> LayerList{ get { return this.layerList; } }
    

    // グリッドの幅、高さ、数
    public GameObject MainGrid;
    private float gridSizeX;
    private float gridSizeY;
    private int gridNumX, gridNumY;

    // 描画開始位置
    private Vector2 StartPoint;

    [SerializeField] private List<GameObject> storeObject = new List<GameObject>();

    void Start()
    {
        // グリッドの数、サイズを取得
        gridSizeX = CreateGridScript.gridSizeX;
        gridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;
        Debug.Log(gridNumX * gridNumY);
        // あたり判定リストのサイズをマスの数と同じだけ増やす
        for (int i = 0; i < gridNumX * gridNumY; i++)
        {
            CollisionGrid.Add(null);
            LayerList.Add(0);
        }
    }


    // StageGrid.csのステージ（紙）情報を追加
    public void AddStageInfo(int layer, List<StageBlock> stage)
    {
        StageInfo[layer] = stage;
    }


    // StageGrid.csの情報を元にあたり判定を構築する
    public void SetStage(int layer)
    {
        // 引数の番号のステージ(紙)情報
        List<StageBlock> _stageGrid = StageInfo[layer];

        //名前を変えるためにカウントを作る
        int nNameCnt = 0;

        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject Cameraobj = GameObject.Find(cameraName);
        // 描画開始位置
        StartPoint.x = Cameraobj.transform.position.x - gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);

        // マスごとにあたり判定を取る用のオブジェクトを生成
        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, nNameCnt++)
            {
                // タグが"none"だったらnullを入れておく
                if (_stageGrid[(y * gridNumX) + x].tag == "none")
                {
                    CollisionGrid[(y * gridNumX) + x] = null;
                    continue;
                }

                //スクリプトでオブジェクトを追加する
                GameObject mass = CreateCollisionObject(
                    "Grid_No." + nNameCnt + ":" + x,    // 名前
                    new Vector3(
                        StartPoint.x + (gridSizeX * x), // 座標
                        StartPoint.y - (gridSizeY * y),
                        transform.position.z),
                    Vector3.zero,
                    _stageGrid[(y * gridNumX) + x].tag, // タグ
                    Cameraobj                           // 親オブジェクト
                    );

                //float rate = Mathf. Mathf.Pow(gridSizeX, 2) + Mathf.Pow(gridSizeY, 2)
                //mass.transform.localScale = new Vector3(gridSizeX*1.414f, gridSizeY * 1.414f, 1.0f);
                mass.transform.localScale = new Vector3(gridSizeX, gridSizeY , 1.0f);

                // あたり判定リストに登録
                CollisionGrid[(y * gridNumX) + x] = mass;

                //Debug.Log(mass.name + "   " + mass.tag + "   " + _stageGrid[x + y].tag);
            }


        }
    }


    // 破った後に合わせてあたり判定リストを更新する
    public void UpdateStage(List<bool> changes)
    {
        if(CollisionGrid.Count != changes.Count) { Debug.LogWarning("サイズが違います" + "   CollisionGrid" + CollisionGrid.Count + "changes" + changes.Count); return; }

        //名前を変えるためにカウントを作る
        int objCount = 0;

        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, objCount++)
            {
                // 変更フラグの確認
                if(changes[objCount])
                {
                    // レイヤーが最後だった場合、あたり判定オブジェクトを消す
                    if (layerList[objCount] == 2)
                    {
                        if(CollisionGrid[objCount])
                        {
                            Destroy(CollisionGrid[objCount]);
                            CollisionGrid[objCount] = null;
                        }
                        continue;
                    }

                    // ある
                    if(CollisionGrid[objCount])
                    {
                        if (StageInfo[layerList[objCount] + 1][objCount].tag != "none")
                        {
                            // 次もある
                            // タグの変更
                            CollisionGrid[objCount].tag = StageInfo[layerList[objCount] + 1][objCount].tag;

                            // 次のレイヤーに更新
                            layerList[objCount]++;
                        }
                        else
                        {
                            // 次はない

                            // オブジェクト消去
                            if (CollisionGrid[objCount])
                            {
                                Destroy(CollisionGrid[objCount]);
                                CollisionGrid[objCount] = null;
                            }

                            // 次のレイヤーに更新
                            layerList[objCount]++;
                        }
                    }
                    // ない
                    else
                    {
                        if (StageInfo[layerList[objCount] + 1][objCount].tag != "none")
                        {
                            // 次はある
                            CollisionGrid[objCount] = CreateCollisionObject(
                                "chage_mass" + objCount,    // 名前
                                new Vector3(
                                    StartPoint.x + (gridSizeX * x), // 座標
                                    StartPoint.y - (gridSizeY * y),
                                    transform.position.z),
                                StageInfo[layerList[objCount] + 1][objCount].rotate,
                                StageInfo[layerList[objCount] + 1][objCount].tag,   // タグ
                                gameObject                                          // 親オブジェクト
                                );

                            float rate = gridSizeX * Mathf.Tan(gridSizeY / gridSizeX);
                            CollisionGrid[objCount].transform.localScale = new Vector3(gridSizeX, gridSizeY, 1);
                            //CollisionGrid[objCount].transform.position += new Vector3(gridSizeX * 0.5f, gridSizeY * 0.5f, 0);

                            // 次のレイヤーに更新
                            layerList[objCount]++;
                        }
                        else
                        {
                            // 次はない
                            // オブジェクト消去
                            if (CollisionGrid[objCount])
                            {
                                Destroy(CollisionGrid[objCount]);
                                CollisionGrid[objCount] = null;
                            }

                            // 次のレイヤーに更新
                            layerList[objCount]++;
                        }
                    }
                }
            }
        }
     }

    // 紙の切れ端とのあたり判定を取る用のオブジェクト生成
    public void CheckCollisionwasteOfPaper(Transform trans)
    {
        float GridSizeX = CreateGridScript.paperGridSizeX;
        float GridSizeY = CreateGridScript.paperGridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;
        Material[] mats = new Material[1];

        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject Cameraobj = GameObject.Find("MainCamera");

        //名前を変えるためにカウントを作る
        int nNameCnt = 0;

        // 描画開始位置 = カメラ座標 - gridの横幅 * 数の半分
        Vector3 StartPoint;
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);

        // マスごとにあたり判定を取る用のオブジェクトを生成
        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, nNameCnt++)
            {
                //スクリプトでオブジェクトを追加する
                //GameObject mass = StageGrid.CreateMesh(GridSizeX, GridSizeY, mats);
                GameObject mass = CreateCollisionObject(
                    "chage_mass" + nNameCnt,            // 名前
                    new Vector3(
                        StartPoint.x + (GridSizeX * x), // 座標
                        StartPoint.y - (GridSizeY * y),
                        trans.position.z),
                    Vector3.zero,
                    "none",                             // タグ
                    Cameraobj                           // 親オブジェクト
                    );

                mass.transform.localScale = new Vector3(CreateGridScript.paperGridSizeX, CreateGridScript.paperGridSizeY, 1);

                mass.GetComponent<BoxCollider>().isTrigger = true;

                // rigidbody
                var rb = mass.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePosition;
                rb.freezeRotation = true;
                rb.useGravity = false;

                //---オブジェクトの設定

                // 当たった時どのようなふるまいをするか
                mass.AddComponent<collsion_test>();

                storeObject.Add(mass);
            }
        }
    }


    public List<Vector2> cellPoints()
    {
        List<Vector2> vector2s = new List<Vector2>();

        // 紙のグリッドの大きさ
        float GridSizeX = CreateGridScript.paperGridSizeX;
        float GridSizeY = CreateGridScript.paperGridSizeY;
        // 縦と横の数
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject Cameraobj = GameObject.Find("MainCamera");

        // 描画開始位置 = カメラ座標 - gridの横幅 * 数の半分
        Vector3 StartPoint;
        StartPoint.x = Cameraobj.transform.position.x - GridSizeX * gridNumX * 0.5f + (GridSizeX * 0.5f);
        StartPoint.y = Cameraobj.transform.position.y + GridSizeY * gridNumY * 0.5f - (GridSizeY * 0.5f);
        int objCount = 0;
        // マスごとにあたり判定を取る用のオブジェクトを生成
        for (int y = 0; y < gridNumY; y++)
        {
            for (int x = 0; x < gridNumX; x++, objCount++)
            {
                vector2s.Add(new Vector2(StartPoint.x + (GridSizeX * x), StartPoint.y - (GridSizeY * y)));
            }
        }

        return vector2s;
    }


    private void CheckCollisionFlags()
    {
        List<bool> changes = new List<bool>();

        for (int i = 0; i < storeObject.Count; i++)
        {
            if (storeObject[i] == null)
            {
                changes.Add(false);
                continue;
            }

            // タグに変更があったらフラグを立てる
            if (storeObject[i].tag != "waste")
            {
                changes.Add(true);
                Destroy(storeObject[i]);
            }
            else
            {
                changes.Add(false);
            }

        }


        storeObject.Clear();

        UpdateStage(changes);
    }


    private GameObject CreateCollisionObject(string name, Vector3 pos, Vector3 rot, string tag, GameObject parent)
    {
        //---生成
        GameObject mass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //親
        mass.transform.SetParent(parent.transform);
        //座標
        mass.transform.position = pos;
        // 回転
        mass.transform.Rotate(rot);
        //名前
        mass.name = name;
        //タグを付ける
        mass.tag = tag;

        return mass;
    }
}
