using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideTriangle : MonoBehaviour
{
    // Divideメソッドの戻り値専用の列挙体
    private enum ResultOfDividing
    {
        NONE = 0,           // 破いていない状態
        MIDDLE_OF_BREAKING, // 破り途中
        END_OF_BREAKING,     // 破り終えた
        //------------------------------------
        DECISION_START_POINT, // 破り始める紙の確定
        RESET_DIVIDING,     // すべてリセット
    }

    // メッシュ
    MeshFilter attachedMeshFilter;
    Mesh attachedMesh;





    // ペーパーナンバー
    [SerializeField] private int number = 0;
    // 切断パス
    [SerializeField] List<Vector3> m_cuttingPath;
    // 切断パス
    [SerializeField] List<Vector2> m_uvs;

    // debug
    [SerializeField] private bool firstVertex;
    // 破り処理中
    [SerializeField] private bool m_bDividing;
    // 破り処理中getter
    public bool Dividing { get { return this.m_bDividing; } }


    //*** 初期化
    void Start()
    {
        // メッシュ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        // 切断パス
        m_cuttingPath = new List<Vector3>();
        m_uvs = new List<Vector2>();
        // 破り処理フラグ
        m_bDividing = false;
        // 最初に紙と交差したかどうか
        firstVertex = false;
    }


    //*** 破る
    public int Divide(List<Vector3> MousePoints)
    {
        int resultState = (int)ResultOfDividing.NONE;

        // 座標が2つなければ処理を行わない
        if (MousePoints.Count == 1)
        {
            Vector3 currentPoint = MousePoints[0];
            if (currentPoint.x >= CreateTriangle.paperSizeX) currentPoint.x = CreateTriangle.paperSizeX;
            if (currentPoint.x <= -CreateTriangle.paperSizeX) currentPoint.x = -CreateTriangle.paperSizeX;
            if (currentPoint.y >= CreateTriangle.paperSizeY) currentPoint.y = CreateTriangle.paperSizeY;
            if (currentPoint.y <= -CreateTriangle.paperSizeY) currentPoint.y = -CreateTriangle.paperSizeY;

            // 外周上に座標があるかどうか調べる
            var outlinePath = GetComponent<OutLinePath>().OutLineVertices;

            for (int i = 0; i < outlinePath.Count; i++)
            {
                // 外周の辺
                Vector3 edge = outlinePath[(i + 1) % outlinePath.Count] - outlinePath[i];

                // 外周の辺上に座標が存在するか
                if (Has(outlinePath[i], outlinePath[(i + 1) % outlinePath.Count], currentPoint))
                {
                    m_bDividing = true;
                    resultState = (int)ResultOfDividing.DECISION_START_POINT;
                }
            }

            return resultState;
        }
        else if (MousePoints.Count == 2)
        {
            Vector3 oldPoint = MousePoints[0];
            Vector3 currentPoint = MousePoints[1];
            if (((currentPoint.x >= CreateTriangle.paperSizeX) && oldPoint.x >= CreateTriangle.paperSizeX) ||
                ((currentPoint.x <= -CreateTriangle.paperSizeX) && oldPoint.x <= -CreateTriangle.paperSizeX) ||
                ((currentPoint.y >= CreateTriangle.paperSizeY) && oldPoint.y >= CreateTriangle.paperSizeY) ||
                ((currentPoint.y <= -CreateTriangle.paperSizeY) && oldPoint.y <= -CreateTriangle.paperSizeY))
            {
                Debug.LogWarning("リセット");

                // やり直し
                return (int)ResultOfDividing.RESET_DIVIDING;
            }
        }

        // 線分の始点・終点
        Vector3 Start, End;
        Start = MousePoints[MousePoints.Count - 2]; // 始点座標
        End = MousePoints[MousePoints.Count - 1];   // 終点座標
        // 始点-終点ベクトル
        Vector3 CurrentEdge = End - Start;

        // 三角形の頂点格納先
        Vector3[] p = new Vector3[3];


        //--- 外周の辺と交差しているか調べる
        List<Vector3> outline = GetComponent<OutLinePath>().OutLineVertices;

        List<Vector3> cross = new List<Vector3>();
        List<Vector2> crossUv = new List<Vector2>();
        //Vector2 crossUv = Vector2.zero;
        float t1, t2;
        for (int i = 0; i < outline.Count; i++)
        {
            // 外周の辺
            Vector3 outlineEdge = outline[(i + 1) % outline.Count] - outline[i];

            if (CalcCrossVertex(Start, outline[i], CurrentEdge, outlineEdge, out t1, out t2))
            {
                // 2目以降の交点を受け付けない
                if (cross.Count == 2) { break; }

                firstVertex = true; // デバッグ用フラグ

                // 破り中フラグON
                m_bDividing = true;
                // 交点
                Vector3 c = Start + (t1 * CurrentEdge);
                cross.Add(c);
                // 交点uv
                Vector2 uv = CalcUVPointToPoint(c);
                crossUv.Add(uv);

                // 切断パスに登録
                m_cuttingPath.Add(c);
                m_uvs.Add(uv);
            }
        }

        // アウトラインに交点を登録
        if (cross.Count != 0)
        {
            for (int i = 0; i < cross.Count; i++)
            {
                if (AddOutlineVertex(cross[i], crossUv[i]) == false) Debug.LogError(cross[i]);
            }
        }


        //--- 押された座標がオブジェクトの外か内部か判定する
        bool onceInside = false; // 一度でも内側判定になったかどうか
        for (int index = 0; index < attachedMesh.triangles.Length; index += 3)
        {
            //三角形の頂点を取得
            p[0] = attachedMesh.vertices[attachedMesh.triangles[index]];
            p[1] = attachedMesh.vertices[attachedMesh.triangles[index + 1]];
            p[2] = attachedMesh.vertices[attachedMesh.triangles[index + 2]];

            #region ---②終点が三角形の内部にあるかないか判定
            bool bInside = IsInsideTriangle(p[0], p[1], p[2], End);
            // 内側にある場合、カーソルの終点を頂点として登録する
            if (bInside)
            {
                //Debug.LogWarning("破り中だよい");

                //--- 内側判定
                onceInside = true;

                // 破るフラグON
                m_bDividing = true;

                // 戻り値:破り中
                resultState = (int)ResultOfDividing.MIDDLE_OF_BREAKING;

                // 切断パスに登録
                m_cuttingPath.Add(End);
                Vector2 uv = CalcEndingPointUV(index, End);
                m_uvs.Add(uv);
                //Debug.Log(uv);
            }
            #endregion
        }

        // 外側＆着る処理中の場合カットするぞ
        if (onceInside == false && m_bDividing)
        {
            //Debug.LogWarning("カットだよい");

            // 切断
            Cut();

            // 切断パスをクリア
            m_cuttingPath.Clear();
            m_uvs.Clear();

            // 破る処理は修了
            m_bDividing = false;
            // 戻り値:切断終了
            resultState = (int)ResultOfDividing.END_OF_BREAKING;
        }


        // 切断パスの整理
        //DecCuttingPath(ref m_cuttingPath);

        // 紙の破れ更新
        UpdateDottedLine();

        // 切断パスが切断パスと交差した場合
        if (MakeHole(Start, End))
        {
            //--- やり直し
            Debug.LogWarning("やり直しだよい");

            // 切断パスをクリア
            m_cuttingPath.Clear();
            m_uvs.Clear();
            // 破る処理は修了
            m_bDividing = false;
            // 戻り値:すべての紙を破りをリセット
            resultState = (int)ResultOfDividing.RESET_DIVIDING;

            // 点線も消す
            List<GameObject> papers = new List<GameObject>();
            papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
            foreach (var paper in papers)
            {
                for (int i = 0; i < paper.transform.childCount; i++)
                {
                    if (paper.transform.GetChild(i).name == "breaking paper line")
                    {
                        Destroy(paper.transform.GetChild(i).gameObject);
                    }
                }
            }

            //(仮SE)
            // 破れるSE
            SoundManager.Instance.PlaySeByName("SE_MenuOperation");

        }

        return resultState;
    }

    private bool AddOutlineVertex(Vector3 checkPoint, Vector2 uv)
    {
        var outline = GetComponent<OutLinePath>().OutLineVertices;
        var outlinePath = GetComponent<OutLinePath>();
        int index = -1;
        for (int i = 0; i < outline.Count; i++)
        {

            if (Has(outline[(i + 1) % outline.Count], outline[i], checkPoint))
            {
                index = i + 1;
                break;
            }
        }

        if (index != -1)
        {
            //Debug.LogWarning("outLinePathに" + checkPoint + "を挿入します");
            //Debug.Log("インデクス数:" + outline.Count + "   挿入要素:" + index);
            // 外周上の点を追加
            outlinePath.InsertPoint(index, checkPoint, uv);
            return true;
        }
        else
        {
            return false;
        }

    }

    //*** 紙が一つでも破かれている状態の場合ture、破る処理がされていなければfalse
    public static bool AllDividing()
    {
        List<GameObject> papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
        foreach (var paper in papers)
        {
            // 1枚でも破る処理が行われていたらtrue
            if (paper.GetComponent<DivideTriangle>().Dividing == true) return true;
        }

        return false;
    }

    // 破り中フラグをすべてリセットする
    public static void AllReset()
    {
        List<GameObject> papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
        foreach (var paper in papers)
        {
            if (paper == null) continue;


            var divide = paper.GetComponent<DivideTriangle>();
            // 破り中フラグOFF
            divide.m_bDividing = false;
            // 切断パスのリセット
            divide.m_cuttingPath.Clear();
            // 切断パスのUV座標のリセット
            divide.m_uvs.Clear();

            // 点線も消す
            for (int i = 0; i < paper.transform.childCount; i++)
            {
                if (paper.transform.GetChild(i).name == "breaking paper line")
                {
                    Destroy(paper.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    //*** オブジェクトを二つに分ける
    void Cut()
    {
        // 新オブジェクト
        List<Vector3> objOutline1 = new List<Vector3>();
        List<Vector3> objOutline2 = new List<Vector3>();
        var uvs1 = new List<Vector2>();
        var normals1 = new List<Vector3>();
        var uvs2 = new List<Vector2>();
        var normals2 = new List<Vector3>();
        // 始点とぶつかったか終点とぶつかったか
        bool start = true;
        // このオブジェクトの外周の頂点リストを取得
        var Outlines = GetComponent<OutLinePath>().OutLineVertices;
        var outlineUVs = GetComponent<OutLinePath>().Getuvs();

        if (Outlines.Count == outlineUVs.Count && m_cuttingPath.Count == m_uvs.Count) { Debug.LogWarning("関門突破"); }
        else { Debug.LogError("リストのサイズが一致しませんでした"); return; }

        // デバッグ------
        var d = new List<Vector3>();
        int crossNum = 0;
        foreach (var vec in Outlines)
            foreach (var cutting in m_cuttingPath)
                if (vec.Equals(cutting))
                {
                    crossNum++;
                    d.Add(cutting);
                }
        // 切断パスとアウトラインが3つ以上かぶっている
        if (crossNum != 2)
        {
            //foreach (var v in d) Debug.LogWarning(v + "   ");
            Debug.LogError(crossNum);
        }
        //-------------

        #region ---アウトラインの作成
        for (int outlineIndex = 0; outlineIndex < Outlines.Count; outlineIndex++)
        {
            // 切断パスと被った場合
            if (m_cuttingPath[0].Equals(Outlines[outlineIndex]))
            {
                //Debug.Log("切断パスとアウトラインが一致したよい");
                // 切断パスの"始点"と外周の頂点が等しいです
                start = true;

                // 切断パスをなぞる 先頭 から 最後尾
                objOutline1.AddRange(m_cuttingPath);
                uvs1.AddRange(m_uvs);

                // 最後尾と同じアウトラインの座標と被ったらi++やめる
                while (m_cuttingPath[m_cuttingPath.Count - 1].Equals(Outlines[outlineIndex]) == false)
                {
                    // 二つ目のアウトラインに追加しておく（切断パスの先頭、最後尾も含み追加しているはず）
                    objOutline2.Add(Outlines[outlineIndex]);
                    uvs2.Add(outlineUVs[outlineIndex]);

                    // 次の座標へ
                    outlineIndex++;
                    if (outlineIndex >= Outlines.Count) { Debug.LogError("外周の頂点情報がおかしい"); break; }
                }
                // 二つ目のアウトラインに追加しておく
                objOutline2.Add(Outlines[outlineIndex]);
                uvs2.Add(outlineUVs[outlineIndex]);
            }
            else if (m_cuttingPath[m_cuttingPath.Count - 1].Equals(Outlines[outlineIndex]))
            {
                //Debug.Log("切断パスとアウトラインが一致したよい");

                // 切断パスの"終点"と外周の頂点が等しいです
                start = false;

                // 切断パスをなぞる 最後尾 から 先頭
                for (int cuttingIndex = m_cuttingPath.Count - 1; cuttingIndex >= 0; cuttingIndex--)
                {
                    objOutline1.Add(m_cuttingPath[cuttingIndex]);
                    uvs1.Add(m_uvs[cuttingIndex]);
                }

                // 先頭と同じアウトラインの座標と被ったらi++やめる
                while (m_cuttingPath[0].Equals(Outlines[outlineIndex]) == false)
                {
                    // 二つ目のアウトラインに追加しておく（切断パスの先頭、最後尾も含み追加しているはず）
                    objOutline2.Add(Outlines[outlineIndex]);
                    uvs2.Add(outlineUVs[outlineIndex]);

                    // 次の座標へ
                    outlineIndex++;
                    if (outlineIndex >= Outlines.Count) { Debug.LogError("外周の頂点情報がおかしい"); break; }
                }
                // 二つ目のアウトラインに追加しておく
                objOutline2.Add(Outlines[outlineIndex]);
                uvs2.Add(outlineUVs[outlineIndex]);
            }
            else
            {
                // 新しいオブジェクト用のアウトラインリストに追加していく
                objOutline1.Add(Outlines[outlineIndex]);
                uvs1.Add(outlineUVs[outlineIndex]);
            }
        }


        if (start)
        {
            // 最後尾の１つ前 ～ 先頭の1つ次
            for (int i = m_cuttingPath.Count - 2; i >= 1; i--)
            {
                objOutline2.Add(m_cuttingPath[i]);
                uvs2.Add(m_uvs[i]);
            }
        }
        else
        {
            // 先頭の1つ次 ～ 最後尾の１つ前
            for (int i = 1; i < m_cuttingPath.Count - 1; i++)
            {
                objOutline2.Add(m_cuttingPath[i]);
                uvs2.Add(m_uvs[i]);

            }
        }
        #endregion

        // エラー対応
        if (objOutline1.Count < 3) { Debug.LogError("エラー objOutline1.Count"); return; }
        if (objOutline2.Count < 3) { Debug.LogError("エラー objOutline2.Count"); return; }

        #region １つ目のカットされたオブジェクトを作成
        GameObject obj1 = GetComponent<DrawMesh>().CreateMesh(objOutline1);
        // ---Components
        obj1.AddComponent<DrawMesh>();
        obj1.AddComponent<DivideTriangle>();
        var collider1 = obj1.AddComponent<MeshCollider>();
        var outline1 = obj1.AddComponent<OutLinePath>();
        var meshFilter1 = obj1.GetComponent<MeshFilter>();
        //var trun = obj1.AddComponent<Turn_Shader>();
        var psMove = obj1.AddComponent<PSMove>();
        // ---Settings
        // uv
        meshFilter1.mesh.uv = uvs1.ToArray();
        // 法線
        //meshFilter1.mesh.normals = normals1.ToArray();
        // メッシュコライダーにメッシュをセット
        collider1.sharedMesh = meshFilter1.mesh;
        // アウトラインを格納
        outline1.UpdateOutLine(objOutline1);
        // タグ
        obj1.tag = "paper";
        // 現在のマテリアルを受け継ぐ
        obj1.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        // レイヤー番号も引き継ぐ
        obj1.GetComponent<DivideTriangle>().number = this.number;
        // z座標を引き継ぐ
        obj1.transform.position += new Vector3(0, 0, transform.position.z);
        #endregion

        #region ２つ目のカットされたオブジェクトを作成
        // オブジェクト生成
        GameObject obj2 = GetComponent<DrawMesh>().CreateMesh(objOutline2);
        // ---Components
        obj2.AddComponent<DrawMesh>();
        obj2.AddComponent<DivideTriangle>();
        var collider2 = obj2.AddComponent<MeshCollider>();
        var outline2 = obj2.AddComponent<OutLinePath>();
        var meshFilter2 = obj2.GetComponent<MeshFilter>();
        //var trun2 = obj2.AddComponent<Turn_Shader>();
        var psMove2 = obj2.AddComponent<PSMove>();
        // ---Settings
        // uv
        meshFilter2.mesh.uv = uvs2.ToArray();
        // 法線
        //meshFilter2.mesh.normals = normals2.ToArray();
        // メッシュコライダーにメッシュをセット
        collider2.sharedMesh = meshFilter2.mesh;
        // アウトラインを格納
        outline2.UpdateOutLine(objOutline2);
        // タグ
        obj2.tag = "paper";
        // 現在のマテリアルを受け継ぐ
        obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        // レイヤー番号も引き継ぐ
        obj2.GetComponent<DivideTriangle>().number = this.number;
        // z座標を引き継ぐ
        obj2.transform.position += new Vector3(0, 0, transform.position.z);
        #endregion


        #region ---紙の破れをどちらのオブジェクトにつけるかを決める
        // obj1のアウトラインがデフォルトの形（四角形）のライン上に存在するかどうか
        List<Vector3> defaultOutline = new List<Vector3>();
        defaultOutline.Add(new Vector3(-CreateTriangle.paperSizeX, CreateTriangle.paperSizeY, 0.0f));
        defaultOutline.Add(new Vector3(CreateTriangle.paperSizeX, CreateTriangle.paperSizeY, 0.0f));
        defaultOutline.Add(new Vector3(CreateTriangle.paperSizeX, -CreateTriangle.paperSizeY, 0.0f));
        defaultOutline.Add(new Vector3(-CreateTriangle.paperSizeX, -CreateTriangle.paperSizeY, 0.0f));
        List<bool> boolList1 = new List<bool>();
        List<bool> boolList2 = new List<bool>();
        List<Vector3> breakLine1 = new List<Vector3>();
        List<Vector3> breakLine2 = new List<Vector3>();

        // オブジェクト1のアウトライン
        for (int i = 0; i < objOutline1.Count; i++)
        {
            bool ret = true;
            // obj1の辺
            Vector3 obj1Edge = objOutline1[(i + 1) % objOutline1.Count] - objOutline1[i];

            for (int j = 0; j < defaultOutline.Count; j++)
            {
                // デフォルトの辺
                Vector3 defEdge = defaultOutline[(j + 1) % defaultOutline.Count] - defaultOutline[j];

                // 平行
                if (defEdge.normalized.Equals(obj1Edge.normalized))
                {
                    if (Has(defaultOutline[(j + 1) % defaultOutline.Count], defaultOutline[j], objOutline1[i]))
                    {
                        ret = false;
                        break;
                    }
                }
            }

            // ブレークライン生成フラグ追加
            boolList1.Add(ret);
        }
        bool oldFlag = false;
        for (int i = 0; i < boolList1.Count; i++)
        {
            if (boolList1[i])
            {
                //--- 現在のフラグが立っている

                // その辺の座標２つをプッシュ
                breakLine1.Add(objOutline1[i]);                             // i
                breakLine1.Add(objOutline1[(i + 1) % objOutline1.Count]);   // i + 1
            }
            else
            {
                //--- 現在のフラグが立っていない
                if (oldFlag)
                {
                    //--- 前回のフラグが立っている

                    // ブレークライン生成
                    var line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, obj1);
                    line1.name = "broken paper line";
                    line1.GetComponent<LineRendererOperator>().hoge();
                    // ブレークラインを初期化
                    breakLine1.Clear();
                }
            }

            // 退避
            oldFlag = boolList1[i];
        }
        if (oldFlag)
        {
            // ブレークライン生成
            var line1 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine1, obj1);
            line1.name = "broken paper line";
            line1.GetComponent<LineRendererOperator>().hoge();
        }


        // オブジェクト２のアウトライン
        for (int i = 0; i < objOutline2.Count; i++)
        {
            bool ret = true;
            // obj２の辺
            Vector3 obj2Edge = objOutline2[(i + 1) % objOutline2.Count] - objOutline2[i];

            for (int j = 0; j < defaultOutline.Count; j++)
            {
                // デフォルトの辺
                Vector3 defEdge = defaultOutline[(j + 1) % defaultOutline.Count] - defaultOutline[j];

                // 平行
                if (defEdge.normalized.Equals(obj2Edge.normalized))
                {
                    // かつ、直線状に存在するかどうか
                    if (Has(defaultOutline[(j + 1) % defaultOutline.Count], defaultOutline[j], objOutline2[i]))
                    {
                        ret = false;
                        break;
                    }
                }
            }

            // ブレークライン生成フラグ追加
            boolList2.Add(ret);
        }
        bool oldFlag2 = false;
        //foreach (var item in boolList2) Debug.Log(item);
        for (int i = 0; i < boolList2.Count; i++)
        {
            if (boolList2[i])
            {
                //--- 現在のフラグが立っている

                // その辺の座標２つをプッシュ
                breakLine2.Add(objOutline2[i]);                             // i
                breakLine2.Add(objOutline2[(i + 1) % objOutline2.Count]);   // i + 1
            }
            else
            {
                //--- 現在のフラグが立っていない
                if (oldFlag2)
                {
                    //--- 前回のフラグが立っている
                    // ブレークライン生成
                    var line2 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, obj2);
                    line2.name = "broken paper line";
                    line2.GetComponent<LineRendererOperator>().hoge();
                    // ブレークラインを初期化
                    breakLine2.Clear();
                }
            }

            // 退避
            oldFlag2 = boolList2[i];
        }
        if (oldFlag2)
        {
            // ブレークライン生成
            var line2 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, obj2);
            line2.name = "broken paper line";
            line2.GetComponent<LineRendererOperator>().hoge();
        }
        #endregion



        // 切断パスをクリア
        m_cuttingPath.Clear();
        m_uvs.Clear();

        // 現在のオブジェクトを消す
        Destroy(gameObject);

        //--- 原点から中心へのベクトルで飛ばす
        Vector3 pos1 = obj1.GetComponent<Renderer>().bounds.center;
        Vector3 pos2 = obj2.GetComponent<Renderer>().bounds.center;

        // 最初はobj1の方を消す処理を進めていく
        // もし、消す紙にアリスが存在していることがわかったらobj2を消すようにする

        // obj1のグリッドを特定
        List<bool> chages = checkCollisionPoints(obj1, CollisionField.Instance.cellPoints());
        // アリスがいるか探す
        if (CollisionField.Instance.CheckAliceExists(chages))
        {
            // obj2を消す

            // タグの変更（廃棄する紙）
            obj2.tag = "waste";
            // 数秒後にデリート
            Destroy(obj2, 1.0f);

            // ステージの更新
            chages = checkCollisionPoints(obj2, CollisionField.Instance.cellPoints());
            CollisionField.Instance.UpdateMovingObjects(chages);
            CollisionField.Instance.UpdateStage(chages);

            // めくる方向を決める
            var BreakPaper = obj2.AddComponent<BreakingPaper>();
            BreakPaper.SetMaterial(GameManager.Instance._mats[number - 1]);
            if (pos2.x >= 0.0f) BreakPaper.SetRight();
            else if (pos2.x < 0.0f) BreakPaper.SetLeft();

            // 紙の破れにもAlphaを適用させる
            for (int i = 0; i < obj2.transform.childCount; i++)
            {
                var breakline = obj2.transform.GetChild(i).gameObject.AddComponent<BreakLine>();
                Material breaklineMat = (Material)Resources.Load("Effects/SecondBreakLine");
                breakline.SetMaterial(breaklineMat);
                if (pos2.x >= 0.0f)
                {
                    breakline.SetRightLine();
                }
                else if (pos2.x < 0.0f)
                {
                    breakline.SetLeftLine();
                }
            }
        }
        else
        {
            // obj1を消す

            // タグの変更（廃棄する紙）
            obj1.tag = "waste";
            // 数秒後にデリート
            Destroy(obj1, 1.0f);

            // ステージの更新
            chages = checkCollisionPoints(obj1, CollisionField.Instance.cellPoints());
            CollisionField.Instance.UpdateMovingObjects(chages);
            CollisionField.Instance.UpdateStage(chages);

            obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;

            // めくる方向を決める
            var BreakPaper = obj1.AddComponent<BreakingPaper>();
            BreakPaper.SetMaterial(GameManager.Instance._mats[number - 1]);
            if (pos1.x >= 0.0f) BreakPaper.SetRight();
            else if (pos1.x < 0.0f) BreakPaper.SetLeft();

            // 紙の破れにもAlphaを適用させる
            for (int i = 0; i < obj1.transform.childCount; i++)
            {
                var breakline = obj1.transform.GetChild(i).gameObject.AddComponent<BreakLine>();
                Material breaklineMat = (Material)Resources.Load("Effects/SecondBreakLine");
                breakline.SetMaterial(breaklineMat);
                // めくる方向を決める
                if (pos1.x >= 0.0f) breakline.SetRightLine();
                else if (pos1.x < 0.0f) breakline.SetLeftLine();
            }

        }

        //obj1の方のアウトラインをセットする
        var outsider = GameObject.Find("cursor").GetComponent<OutSide_Paper_Script_Second>();
        outsider.DivideEnd();

        // 破れるSE
        SoundManager.Instance.PlaySeByName("RipUpPaper07");
    }

    //*** 切断パスがクロススタかどうか判定する
    bool MakeHole(Vector3 Start, Vector3 End)
    {
        //if (!exsitStartPoint) return false;
        bool ret = false;

        float f1, f2;

        Vector3 currentLine = End - Start;

        // 切断パスと被っているか確認
        for (int edgeIndex = 0; edgeIndex < m_cuttingPath.Count - 3; edgeIndex++) // m_cuttingPathのサイズが3以下の場合は何もなし
        {
            // 切断パス
            Vector3 cuttingEdge = m_cuttingPath[(edgeIndex + 1) % m_cuttingPath.Count] - m_cuttingPath[edgeIndex];

            if (CalcCrossVertex(Start, m_cuttingPath[edgeIndex], currentLine, cuttingEdge, out f1, out f2))
            {
                Vector3 cross = Start + (currentLine * f1);
                Vector2 uv, uv1, uv2;
                Vector3 normal = Vector3.zero;
                uv1 = uv2 = Vector2.zero;
                for (int i = 0; i < attachedMesh.triangles.Length; i++)
                {
                    Vector3 p = attachedMesh.vertices[attachedMesh.triangles[i]];
                    if (p.Equals(m_cuttingPath[(edgeIndex + 1) % m_cuttingPath.Count]))
                    {
                        uv2 = attachedMesh.uv[attachedMesh.triangles[i]];
                        normal = attachedMesh.normals[attachedMesh.triangles[i]];
                    }
                }
                for (int i = 0; i < attachedMesh.triangles.Length; i++)
                {
                    Vector3 p = attachedMesh.vertices[attachedMesh.triangles[i]];
                    if (p.Equals(m_cuttingPath[edgeIndex]))
                    {
                        uv1 = attachedMesh.uv[attachedMesh.triangles[i]];
                    }
                }
                uv = Vector2.Lerp(uv1, uv2, f2);

                return true;

            }

        }

        return ret;
    }

    //*** 線分上に点が存在するか
    public bool Has(Vector3 point1, Vector3 point2, Vector3 p)
    {
        float ac = Vector3.Distance(point1, p);
        float cb = Vector3.Distance(p, point2);
        float ab = Vector3.Distance(point1, point2);

        float ret = ac + cb - ab;
        return (ret < 0.000001f);
    }

    // 交点のuv座標を計算する
    Vector2 CalcUV(int pattern, int triangleNum, float t2)
    {
        Vector2 ret = Vector2.zero;
        // ①p1-p2の辺をまたいでいるか
        if (pattern == 0)
        {
            ret = Vector2.Lerp(
                attachedMesh.uv[attachedMesh.triangles[triangleNum]],
                attachedMesh.uv[attachedMesh.triangles[triangleNum + 1]],
                t2);
        }
        // ②p2-p3の辺をまたいでいるか
        else if (pattern == 1)
        {
            ret = Vector2.Lerp(
                attachedMesh.uv[attachedMesh.triangles[triangleNum + 1]],
                attachedMesh.uv[attachedMesh.triangles[triangleNum + 2]],
                t2);
        }
        // ③p3-p1の辺をまたいでいるか
        else if (pattern == 2)
        {
            ret = Vector2.Lerp(
                attachedMesh.uv[attachedMesh.triangles[triangleNum + 2]],
                attachedMesh.uv[attachedMesh.triangles[triangleNum]],
                t2);
        }

        return ret;
    }

    //*** 終点のuv座標を計算する
    Vector2 CalcEndingPointUV(int triangleNum, Vector3 endPoint)
    {
        Vector2 ret = Vector2.zero;

        Vector3 p1, p2, p3;
        p1 = attachedMesh.vertices[attachedMesh.triangles[triangleNum]];
        p2 = attachedMesh.vertices[attachedMesh.triangles[triangleNum + 1]];
        p3 = attachedMesh.vertices[attachedMesh.triangles[triangleNum + 2]];


        // 考え方
        // p1から終点へ向けて線をひっぱり、線分p2-p3との交点の座標を求める
        // 交点座標をもとにuvの座標を求める
        // そうすることで、p1と交点の座標とuvがわかるので終点のuv座標を求められる


        // 線分のベクトル
        Vector3 v1, v2, v;
        // 交点までの距離の比率
        float t;
        v1 = endPoint - p1; // p1 → 終点ベクトル
        v2 = p3 - p2;       // p2 → p3ベクトル
        v = p2 - p1;        // お互いの始点間のベクトル
        t = Vector3.Cross(v, v2).z / Vector3.Cross(v1, v2).z;

        // 交点計算
        Vector3 cross = p1 + v1 * t;

        // p2-p3のuvをもとに交点のuvを求める
        Vector2 crossUV = Vector2.Lerp(
            attachedMesh.uv[attachedMesh.triangles[triangleNum + 1]], // p2のuv
            attachedMesh.uv[attachedMesh.triangles[triangleNum + 2]], // p3のuv
            (cross - p2).magnitude / v2.magnitude // 比率 p2-cross / p2-p3
            );


        // p1-crossのuvをもとに終点のuvを求める
        ret = Vector2.Lerp(
            attachedMesh.uv[attachedMesh.triangles[triangleNum]], // p1のuv
            crossUV,                                              // 交点のuv
            (endPoint - p1).magnitude / (cross - p1).magnitude    // 比率 終点-p1 / cross-p1
            );

        return ret;
    }


    // メッシュの頂点がかぶっていたら減らす
    private void DecVertices(ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<Vector3> normals, ref List<int> triangles)
    {
        // 頂点がかぶっているかどうか
        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = 0; j < vertices.Count; j++)
            {
                if (i == j) continue;

                if (vertices[i].Equals(vertices[j]) == false) continue;

                // ここまで来たらかぶっているのでj側の頂点情報を消す

                // jのuv,法線を消す
                uvs.RemoveAt(j); normals.RemoveAt(j);

                // トライアングルの番号jをiに書き換える
                for (int k = 0; k < triangles.Count; k++)
                {
                    if (triangles[k] == j) triangles[k] = i;
                }

                // 頂点を削除
                vertices.RemoveAt(j);
            }
        }
    }

    // 切断パスの頂点がかぶっていたら減らす
    private void DecCuttingPath(ref List<Vector3> cutting)
    {
        // リストの頂点群で頂点がかぶっているかどうか
        for (int i = 0; i < cutting.Count; i++)
        {
            for (int j = 0; j < cutting.Count; j++)
            {
                if (i == j) continue;

                if (cutting[i].Equals(cutting[j]) == false) continue;

                // ここまで来たらかぶっているのでj側の頂点情報を消す

                // 頂点を削除
                cutting.RemoveAt(j);
            }
        }
    }

    // 点線を生成 又は 更新する
    private void UpdateDottedLine()
    {
        // 破るフラグが立っている場合
        // かつ、このオブジェクトの子オブジェクトにpaper breakが存在しない場合
        // 紙の破れを生成する
        // paper break が存在している場合はやぶれを更新する
        if (m_cuttingPath.Count >= 2)
        {
            List<GameObject> breakLines = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
                breakLines.Add(transform.GetChild(i).gameObject);

            if (breakLines.Count != 0)
            {
                // 進行形で敗れているかどうか
                var breakLine = transform.Find("breaking paper line");
                if (breakLine != null)
                {
                    // 現在進行中の破れが存在するため、それを更新する
                    breakLine.gameObject.GetComponent<LineRendererOperator>().SetPoints(m_cuttingPath);
                }
                // 破れが存在しない場合、新しく生成する
                else
                {
                    PaperBreakLineManager.Instance.CreateBreakLine(m_cuttingPath, gameObject);
                }
            }
            else
            {
                // 破れが存在しない場合、新しく生成する
                PaperBreakLineManager.Instance.CreateBreakLine(m_cuttingPath, gameObject);
            }
        }
    }

    // 番号の設定
    public void SetNumber(int num) { number = num; }
    // 番号の取得
    public int GetNumber() { return number; }


    // 点と点の間のUV座標を求める
    Vector2 CalcUVPointToPoint(Vector3 checkPoint)
    {

        float rateX, rateY;
        rateX = (CreateTriangle.paperSizeX + checkPoint.x) / (CreateTriangle.paperSizeX * 2);
        rateY = (CreateTriangle.paperSizeY + checkPoint.y) / (CreateTriangle.paperSizeY * 2);

        Vector2 resultUV;
        resultUV.x = rateX;
        resultUV.y = rateY;

        return resultUV;
    }


    /*
    * @fn		CalcCrossVertex
    * @brief	線分と線分が交差しているかチェックする
    * @param	(Vector3) Startpoint1   線分１の始点の座標
    * @param	(Vector3) Startpoint2   線分２の始点の座標
    * @param	(Vector3) edge1         線分１のベクトル（長さも含んでいるためnormalizeしない）
    * @param	(Vector3) edge2         線分２のベクトル（長さも含んでいるためnormalizeしない）
    * @return   true:交差してる  false:交差していない	
    * @detail	交差している場合、下記の式で交点の座標が求められる
    *           Startpoint1 + edge1 * t1
    */
    private bool CalcCrossVertex(Vector3 Startpoint1, Vector3 Startpoint2, Vector3 edge1, Vector3 edge2, out float t1, out float t2)
    {
        // 線分１の始点から線分２の始点へのベクトル
        Vector3 distance = Startpoint2 - Startpoint1;

        // それぞれの線分の始点から交点になりうる座標までのベクトルの比率を求める
        t1 = Vector3.Cross(distance, edge2).z / Vector3.Cross(edge1, edge2).z;
        t2 = Vector3.Cross(distance, edge1).z / Vector3.Cross(edge1, edge2).z;

        // 交差判定
        return ((0 <= t1 && t1 <= 1) && (0 <= t2 && t2 <= 1));
    }

    /*
    * @fn		IsInsideTriangle
    * @brief	点が三角形の内側に存在するか
    * @param	(Vector3) p1            三角形の頂点１
    * @param	(Vector3) p2            三角形の頂点２
    * @param	(Vector3) p3            三角形の頂点３
    * @param	(Vector3) checkPoint    調べる点
    * @return   true:存在する  false:存在しない
    */
    private bool IsInsideTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 checkPoint)
    {
        bool ret = false;

        //線上は内側とみなします。
        Vector3 Area1 = p2 - p1; Vector3 AreaP1 = checkPoint - p1;
        Vector3 Area2 = p3 - p2; Vector3 AreaP2 = checkPoint - p2;
        Vector3 Area3 = p1 - p3; Vector3 AreaP3 = checkPoint - p3;
        Vector3 cr1 = Vector3.Cross(Area1, AreaP1);
        Vector3 cr2 = Vector3.Cross(Area2, AreaP2);
        Vector3 cr3 = Vector3.Cross(Area3, AreaP3);
        //内積で順方向か逆方向か調べる
        float dot_12 = Vector3.Dot(cr1, cr2);
        float dot_13 = Vector3.Dot(cr1, cr3);
        if (dot_12 >= 0 && dot_13 >= 0)
        {
            ret = true;
        }


        return ret;
    }

    // 3で割った余りを返す
    private int Percent3(int num)
    {
        return num % 3;
    }




    // その座標群がメッシュ内に存在するか
    // 存在する場合true、存在しない場合falseを
    // その要素と同じList<bool>の型に入れていく
    static List<bool> checkCollisionPoints(GameObject obj, List<Vector2> Pounts)
    {
        List<bool> result = new List<bool>();

        var attachedMeshFilter = obj.GetComponent<MeshFilter>();
        var attachedMesh = attachedMeshFilter.mesh;

        // 三角形の頂点格納先
        Vector3 p1, p2, p3;
        // 内側にあるかどうか
        bool bInside;
        Vector3 p;

        foreach (var point in Pounts)
        {
            bInside = false;

            // z座標はメッシュに合わせる
            p = new Vector3(point.x, point.y, attachedMesh.vertices[attachedMesh.triangles[0]].z);

            for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
            {
                //三角形の頂点を取得
                p1 = attachedMesh.vertices[attachedMesh.triangles[i]];
                p2 = attachedMesh.vertices[attachedMesh.triangles[i + 1]];
                p3 = attachedMesh.vertices[attachedMesh.triangles[i + 2]];

                //線上は内側とみなします。
                Vector3 Area1 = p2 - p1; Vector3 AreaP1 = p - p1;
                Vector3 Area2 = p3 - p2; Vector3 AreaP2 = p - p2;
                Vector3 Area3 = p1 - p3; Vector3 AreaP3 = p - p3;
                Vector3 cr1 = Vector3.Cross(Area1, AreaP1);
                Vector3 cr2 = Vector3.Cross(Area2, AreaP2);
                Vector3 cr3 = Vector3.Cross(Area3, AreaP3);
                //内積で順方向か逆方向か調べる
                float dot_12 = Vector3.Dot(cr1, cr2);
                float dot_13 = Vector3.Dot(cr1, cr3);
                if (dot_12 >= 0 && dot_13 >= 0)
                {
                    //***** 内側
                    bInside = true;
                    break;
                }
            }

            // 一度でも内側判定があればtrueが入る
            result.Add(bInside);
        }


        return result;
    }



}