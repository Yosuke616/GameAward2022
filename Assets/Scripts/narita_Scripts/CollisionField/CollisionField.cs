/*
    メインカメラに写っている紙のとおりにあたり判定を設定する
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StageBlock
{
    public string tag = "none";
    public Vector3 rotate = Vector3.zero;
    public int type = 0;
    // このあたり判定ブロックのもととなるオブジェクト
    public GameObject sourceObject;
}

public class CollisionField : SingletonMonoBehaviour<CollisionField>
{
    // 0から数えるので
    // ３枚の場合は2に設定
    // ４枚の場合は3に設定
    public int MaxLayerNum = 2;
    // 実際のあたり判定を行っているシーンのカメラ
    private string cameraName = "SubCamera0";
    // ステージ(紙)情報
    [SerializeField] private List<StageBlock>[] StageInfo;
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

    // 初期化
    void Start()
    {
        StageInfo = new List<StageBlock>[MaxLayerNum + 1];

        // グリッドの数、サイズを取得
        gridSizeX = CreateGridScript.gridSizeX;
        gridSizeY = CreateGridScript.gridSizeY;
        gridNumX = CreateGridScript.horizon;
        gridNumY = CreateGridScript.virtical;
        // あたり判定リストのサイズをマスの数と同じだけ増やす
        for (int i = 0; i < gridNumX * gridNumY; i++)
        {
            CollisionGrid.Add(null);
            LayerList.Add(0);
        }

    }


    // その紙のあたり判定を登録する
    public void AddStageInfo(int layer, List<StageBlock> stage)
    {
        StageInfo[layer] = stage;
    }



    /***** 1枚目のあたり判定オブジェクトを生成 *****/
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
                GameObject mass = CreateCollisionObject("Grid_No." + nNameCnt + ":" + x,    // 名前
                new Vector3(StartPoint.x + (gridSizeX * x), StartPoint.y - (gridSizeY * y), transform.position.z), // 座標
                Vector3.zero, // 回転
                _stageGrid[(y * gridNumX) + x].tag, // タグ
                Cameraobj                           // 親オブジェクト
                );

                // あたり判定の大きさをグリッドの1マスサイズににする
                mass.transform.localScale = new Vector3(gridSizeX, gridSizeY , 1.0f);

                // あたり判定リストに登録
                CollisionGrid[(y * gridNumX) + x] = mass;


                //--- エネミー

                if (CollisionGrid[(y * gridNumX) + x].tag == "enemy")
                {
                    // 決められたレイヤーでしか存在できない
                    var checkLayer = CollisionGrid[(y * gridNumX) + x].AddComponent<CheckSameLayer>();
                    checkLayer.SetLayer(layer + 1);

                    // もととなるオブジェクト
                    GameObject original = _stageGrid[(y * gridNumX) + x].sourceObject;

                    // 座標を合わせる
                    original.GetComponent<Enemy>().Synchronous(CollisionGrid[(y * gridNumX) + x]);
                    original.GetComponent<Enemy>().SetActive(true);
                    // 親を変える
                    CollisionGrid[(y * gridNumX) + x].transform.SetParent(original.transform);
                    // コリジョンをトリガーに変更
                    CollisionGrid[(y * gridNumX) + x].GetComponent<BoxCollider>().isTrigger = true;
                    // エネミーだった場合は生成はするがStagelayerは空にしておく
                    CollisionGrid[(y * gridNumX) + x] = null;
                }
                else if (CollisionGrid[(y * gridNumX) + x].tag == "rock")
                {
                    // 決められたレイヤーでしか存在できない
                    var checkLayer = CollisionGrid[(y * gridNumX) + x].AddComponent<CheckSameLayer>();
                    checkLayer.SetLayer(layer + 1);

                    // もととなるオブジェクト
                    GameObject original = _stageGrid[(y * gridNumX) + x].sourceObject;

                    // 親を変える
                    CollisionGrid[(y * gridNumX) + x].transform.SetParent(original.transform);
                    // コリジョンをトリガーに変更
                    CollisionGrid[(y * gridNumX) + x].GetComponent<BoxCollider>().isTrigger = true;
                    // エネミーだった場合は生成はするがStagelayerは空にしておく
                    CollisionGrid[(y * gridNumX) + x] = null;
                }
            }
        }

        // 最初は1枚目なのでlayerを0にしておく
        for (int i = 0; i < LayerList.Count; i++)
            LayerList[i] = 0;
    }


    /***** 破った後に合わせてあたり判定リストを更新する *****/
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
                    if (layerList[objCount] == MaxLayerNum)
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

                            //--- エネミー

                            if (CollisionGrid[objCount].tag == "enemy")
                            {
                                // 決められたレイヤーでしか存在できない
                                var checkLayer = CollisionGrid[objCount].AddComponent<CheckSameLayer>();
                                checkLayer.SetLayer(layerList[objCount] + 2);

                                // もととなるオブジェクト
                                GameObject original = StageInfo[layerList[objCount] + 1][objCount].sourceObject;
                                if (original != null)
                                {

                                    // 座標を合わせる
                                    original.GetComponent<Enemy>().Synchronous(CollisionGrid[objCount]);
                                    original.GetComponent<Enemy>().SetActive(true);
                                    // 親を変える
                                    CollisionGrid[objCount].transform.SetParent(original.transform);
                                    // コリジョンをトリガーに変更
                                    CollisionGrid[objCount].GetComponent<BoxCollider>().isTrigger = true;
                                }
                                else
                                {
                                    Destroy(CollisionGrid[objCount]);
                                }
                                // エネミーだった場合は生成はするがStagelayerは空にしておく
                                CollisionGrid[objCount] = null;
                            }


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
                            CollisionGrid[objCount] = CreateCollisionObject("chage_mass" + objCount,    // 名前
                                new Vector3(StartPoint.x + (gridSizeX * x),StartPoint.y - (gridSizeY * y),transform.position.z), // 座標
                                StageInfo[layerList[objCount] + 1][objCount].rotate,// 回転
                                StageInfo[layerList[objCount] + 1][objCount].tag,   // タグ
                                gameObject                                          // 親オブジェクト
                                );

                            // あたり判定の大きさをグリッドの1マスサイズににする
                            CollisionGrid[objCount].transform.localScale = new Vector3(gridSizeX, gridSizeY, 1);

                            //--- エネミー
                            if (CollisionGrid[objCount].tag == "enemy")
                            {
                                // 決められたレイヤーでしか存在できない
                                var checkLayer = CollisionGrid[objCount].AddComponent<CheckSameLayer>();
                                checkLayer.SetLayer(layerList[objCount] + 2);

                                // もととなるオブジェクト
                                GameObject original = StageInfo[layerList[objCount] + 1][objCount].sourceObject;
                                if (original != null)
                                {
                                    // 座標を合わせる
                                    original.GetComponent<Enemy>().Synchronous(CollisionGrid[objCount]);
                                    original.GetComponent<Enemy>().SetActive(true);
                                    // 親を変える
                                    CollisionGrid[objCount].transform.SetParent(original.transform);
                                    // コライダーをトリガーに
                                    CollisionGrid[objCount].GetComponent<BoxCollider>().isTrigger = true;
                                }
                                else
                                {
                                    Destroy(CollisionGrid[objCount]);
                                }
                                // エネミーだった場合は生成はするがStagelayerは空にしておく
                                CollisionGrid[objCount] = null;
                            }
                            
                            

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

    /***** 破った後に合わせて動いているオブジェクトのあたり判定を更新する *****/
    public void UpdateMovingObjects(List<bool> changes)
    {
        // マスの横・縦の数
        float gridSizeX = CreateGridScript.gridSizeX;
        float gridSizeY = CreateGridScript.gridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;
        
        Vector2 start;
        // rayを飛ばす座標
        start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        start.y = gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);
        
        
        // カメラを見つける
        GameObject cameraObj = GameObject.Find(cameraName);

        int massCounter = 0;
        for (int y = 0; y < gridNumY; y++)
            for (int x = 0; x < gridNumX; x++)
            {
                // 紙の破れていないところにはレイを飛ばさない
                if (changes[massCounter] == false)
                {
                    massCounter++;
                    continue;
                }

                RaycastHit hit;
                if (Physics.Raycast(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f), out hit))
                {
                    // 動いている敵だった場合
                    if(hit.collider.gameObject.tag == "enemy" || hit.collider.gameObject.tag == "CardSoldier")
                    {
                        // オブジェクトを消す
                        Destroy(hit.collider.gameObject);

                        //Debug.DrawRay(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f));
                        //Debug.LogError("");
                    }
                }

                massCounter++;
            }
    }



    //*** 破った紙にアリスが存在するかどうかを返す
    //あたり判定グリッドにレイを飛ばしている 
    public bool CheckAliceExists(List<bool> changes)
    {
        bool result = false;

        // マスの横・縦の数
        float gridSizeX = CreateGridScript.gridSizeX;
        float gridSizeY = CreateGridScript.gridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        // rayを飛ばす座標
        Vector2 start;
        start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        start.y = gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);

        // カメラを見つける
        GameObject cameraObj = GameObject.Find(cameraName);

        int massCounter = 0;
        for (int y = 0; y < gridNumY; y++)
            for (int x = 0; x < gridNumX; x++)
            {
                // 紙の破れていないところにはレイを飛ばさない
                if (changes[massCounter] == false)
                {
                    massCounter++;
                    continue;
                }

                RaycastHit hit;
                if (Physics.Raycast(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f), out hit))
                {
                    // プレイヤーだった場合
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        result = true;

                        //Debug.DrawRay(cameraObj.transform.position, new Vector3(start.x + (gridSizeX * x), start.y - (gridSizeY * y), 22.0f));
                        //Debug.LogError("");
                    }
                }

                massCounter++;
            }

        return result;
    }


    // 紙のグリッドのすべてのマスの中心座標をリストにして返す
    // 順番は左上から右下
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
            for (int x = 0; x < gridNumX; x++, objCount++)
                vector2s.Add(new Vector2(StartPoint.x + (GridSizeX * x), StartPoint.y - (GridSizeY * y)));

        return vector2s;
    }






    private GameObject CreateCollisionObject(string name, Vector3 pos, Vector3 rot, string tag, GameObject parent)
    {
        //---生成
        GameObject mass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //親
        mass.transform.SetParent(parent.transform);
        //座標
        mass.transform.position = pos;
        //回転
        mass.transform.Rotate(rot);
        //名前
        mass.name = name;
        //タグを付ける
        mass.tag = tag;

        return mass;
    }
}
