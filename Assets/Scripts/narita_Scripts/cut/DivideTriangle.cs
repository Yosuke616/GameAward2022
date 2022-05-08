using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideTriangle : MonoBehaviour
{
    // メッシュ
    MeshFilter attachedMeshFilter;
    Mesh attachedMesh;
    
    // ペーパーナンバー
    [SerializeField] private int number = 0;
    // 切断パス
    [SerializeField] List<Vector3> cuttingPath;
    // 切断パスの頂点番号
    [SerializeField] List<int> cuttingVertexNumbers;
    // メッシュの頂点座標群(デバッグ用)
    [SerializeField] List<Vector3> debugList = new List<Vector3>();
    // 始点が存在するか
    private bool exsitStartPoint;

    // 初期化
    void Start()
    {
        // メッシュ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        cuttingPath = new List<Vector3>();
        cuttingVertexNumbers = new List<int>();

        exsitStartPoint = false;
    }


    /* 破る処理
     * 戻り値:メッシュを一度でも三角形分割していたらtrue
     */
    public bool Divide(ref List<Vector3> MousePoints)
    {
        if (MousePoints.Count <= 1) return false;
        // 線分の始点・終点
        Vector3 Start, End;
        Start = MousePoints[MousePoints.Count - 2];
        End = MousePoints[MousePoints.Count - 1];
        // 始点-終点ベクトル
        var CurrentEdge = End - Start;

        // メッシュを一度でも三角形分割していたらtrue
        bool bDivide = false;

        // 初回かどうか（破る処理の1回目とその他で分ける）
        bool first;

        // 交点の 座標・頂点番号
        Vector3 cross1, cross2;
        int c1, c2;
        cross1 = cross2 = Vector3.zero;
        c1 = c2 = 0;

        // 終点の 頂点番号
        int e = 0;

        // 三角形の頂点格納先
        Vector3[] p = new Vector3[3];
        int[] n = new int[3];
        // 辺と交わっているかどうか
        bool[] IsCrossing = new bool[3];

        // 新しく設定するメッシュ情報
        List<Vector3> vertices = new List<Vector3>();   // 頂点座標
        List<Vector2> uvs = new List<Vector2>();        // 頂点のuv値
        List<Vector3> normals = new List<Vector3>();    // 頂点の法線情報
        List<int> triangles = new List<int>();          // 頂点のつなぎ方

        // 頂点情報をローカルにコピーして
        // 必要になる頂点(交点や終点)を追加していく
        // triangle（頂点のつなぎ方）はコピーせず最初から構築していく
        vertices.AddRange(attachedMesh.vertices);
        uvs.AddRange(attachedMesh.uv);
        normals.AddRange(attachedMesh.normals);

        // 三角形と線分の交点の座標と頂点番号を保持する
        List<Vector3> tempCrosses = new List<Vector3>();
        List<int> crossVertexNumbers = new List<int>();


        // ---破り始めの座標と三角形の頂点座標がかぶっているか
        first = true;
        if (vertices.Contains(Start)) first = false;

        // index=0の三角形 0,1,2    index=3の三角形 3,4,5   index=6の三角形 6,7,8
        for (int index = 0; index < attachedMesh.triangles.Length; index += 3)
        {
            // 交差フラグ３辺
            IsCrossing[0] = IsCrossing[1] = IsCrossing[2] = false;
            cross1 = cross2 = Vector3.zero;

            //三角形の頂点を取得
            p[0] = attachedMesh.vertices[attachedMesh.triangles[index]];
            p[1] = attachedMesh.vertices[attachedMesh.triangles[index + 1]];
            p[2] = attachedMesh.vertices[attachedMesh.triangles[index + 2]];
            // 三角形の頂点番号取得
            n[0] = attachedMesh.triangles[index];
            n[1] = attachedMesh.triangles[index + 1];
            n[2] = attachedMesh.triangles[index + 2];

            #region ---①三角形と線分との「交点」を求める
            for (int cnt = 0; cnt < 3; cnt++)
            {
                // 交点までの距離の比率
                float t1, t2;
                // 交差フラグ
                bool bCross = false;
                // 三角形の１辺
                Vector3 triangleEdge = p[(cnt + 1) % 3] - p[cnt];

                // 交差確認
                bCross = CalcCrossVertex(Start, p[cnt], CurrentEdge, triangleEdge, out t1, out t2);

                // 交差判定
                if (bCross)
                {
                    // ２つ目の交点の場合
                    if (IsCrossing[0] || IsCrossing[1])
                    {
                        // ２つ目の交点の座標を求める
                        cross2 = Start + (t1 * CurrentEdge);
                        // 交差フラグON
                        IsCrossing[cnt] = true;

                        // 交点と始点が同じ位置の場合、交差判定を取り消す
                        if (Start.Equals(cross2)) IsCrossing[cnt] = false;

                        // tempCrossesとcross2がかぶっていない場合、頂点リストに追加
                        if(tempCrosses.Contains(cross2))
                        {
                            // かぶっていた場合、元からある頂点の番号に設定する
                            c2 = crossVertexNumbers[tempCrosses.IndexOf(cross2)];
                        }
                        else
                        {
                            // 交点2を追加
                            vertices.Add(cross2);
                            uvs.Add(CalcUV(cnt, index, t2));
                            normals.Add(attachedMesh.normals[attachedMesh.triangles[index]]);
                            c2 = vertices.Count - 1;

                            // 交点を保存しておく
                            tempCrosses.Add(cross2);
                            crossVertexNumbers.Add(c2);
                        }
                    }
                    // １つ目の交点の場合
                    else
                    {
                        // 交点を求める
                        cross1 = Start + (t1 * CurrentEdge);
                        // 交差フラグON
                        IsCrossing[cnt] = true;
                        // 交点と始点が同じ位置の場合、交差判定を取り消す
                        if (Start.Equals(cross1))
                        {
                            IsCrossing[cnt] = false;
                            //****
                            if (!first)
                            {
                                // 切断パスと被っているはずなので
                                if(cuttingPath.Contains(cross1))
                                {
                                    tempCrosses.Add(cross1);
                                    crossVertexNumbers.Add(cuttingVertexNumbers[cuttingPath.IndexOf(cross1)]);
                                }
                            }
                        }

                        // 頂点情報がかぶっていない場合、頂点リストに追加
                        if(tempCrosses.Contains(cross1))
                        {
                            // かぶっていた場合、元からある頂点の番号に設定する
                            c1 = crossVertexNumbers[tempCrosses.IndexOf(cross1)];
                        }
                        else
                        {
                            // 頂点リストにを追加
                            vertices.Add(cross1);
                            uvs.Add(CalcUV(cnt, index, t2));
                            normals.Add(attachedMesh.normals[attachedMesh.triangles[index]]);
                            c1 = vertices.Count - 1;

                            // 交点を保存しておく
                            tempCrosses.Add(cross1);
                            crossVertexNumbers.Add(c1);
                        }
                    }
                }
            }
            #endregion

            #region ---②終点が三角形の内部にあるかないか判定
            bool bInside = IsInsideTriangle(p[0], p[1], p[2], End);
            // 内側にある場合、カーソルの終点を頂点として登録する
            if (bInside)
            {
                //--- 内側判定

                // 破るフラグON
                bDivide = true;

                // 終点を頂点として追加する
                if(tempCrosses.Contains(End))
                {
                    // かぶっていた場合、元からある頂点の番号に設定する
                    e = crossVertexNumbers[tempCrosses.IndexOf(End)];
                }
                else
                {
                    // 終点を追加
                    vertices.Add(End);
                    uvs.Add(CalcEndingPointUV(index, End));
                    normals.Add(attachedMesh.normals[attachedMesh.triangles[index]]);
                    e = vertices.Count - 1;
                }
            }
            #endregion

            if(first)
            {
                //=== 初回の破る処理のみ
                if (bInside)
                {
                    //--- 終点が内側にある

                    //*** 「交差していない」
                    if (!IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2])
                    {
                        // 現在の三角形の状態のまま保存
                        for (int cnt = 0; cnt < 3; cnt++)
                            triangles.Add(attachedMesh.triangles[index + cnt]);
                    }
                    //*** 「１辺」と交わっている
                    else
                    {
                        // 三角形の頂点座標３つ、「交差した座標」、「終点」の計5つの頂点で三角形を「３つ」作る
                        Dividing_one_triangle_into_Four_triangles(ref triangles, IsCrossing, n, e, c1);
                    }
                }
                else
                {
                    //--- 終点が外側にある
                    //*** 線分が三角形の「２辺」と交わっている
                    if ((IsCrossing[0] && IsCrossing[1]) || (IsCrossing[1] && IsCrossing[2]) || (IsCrossing[2] && IsCrossing[0]))
                    {
                        // 三角形の頂点座標３つ、交差した座標２つの計5つの頂点で三角形を「３つ」作る
                        Dividing_one_triangle_into_Three_triangles_by_two_intersections(ref triangles, IsCrossing, n, c1, c2);
                    }
                    //*** 線分が三角形と「交わっていない」
                    else
                    {
                        // 現在の三角形の状態のまま保存
                        for (int cnt = 0; cnt < 3; cnt++)
                            triangles.Add(attachedMesh.triangles[index + cnt]);
                    }
                }
            }
            else
            {
                //=== 破る処理２回目以降
                if (bInside)
                {
                    //--- 終点が内側にある

                    // パターン① 「交わっていない」
                    if (!IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2])
                    {
                        // 三角形の頂点座標３つ、「終点」の計4つの頂点で三角形を「３つ」作る
                        Dividing_one_triangle_into_Three_triangles_by_center(ref triangles, n, e);
                    }
                    // パターン② 「１辺」と交わっている
                    else if (IsCrossing[0] || IsCrossing[1] || IsCrossing[2])
                    {
                        Dividing_one_triangle_into_Four_triangles(ref triangles, IsCrossing, n, e, c1);
                    }
                    // 例外パターン
                    else
                    {
                        // 現在の三角形の状態のまま保存
                        for (int cnt = 0; cnt < 3; cnt++)
                            triangles.Add(attachedMesh.triangles[index + cnt]);
                    }
                }
                else
                {
                    //--- 終点が外側

                    // パターン③ １辺だけが交わっている
                    if ((IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2]) ||
                        (!IsCrossing[0] && IsCrossing[1] && !IsCrossing[2]) ||
                        (!IsCrossing[0] && !IsCrossing[1] && IsCrossing[2]))
                    {
                        // 交点を基準に”２つ”に分ける
                        Dividing_one_triangle_into_Two_triangles_by_intersection(ref triangles, IsCrossing, n, c1);

                    }
                    // パターン④ ２辺が交わっているとき
                    else if ((IsCrossing[0] && IsCrossing[1] && !IsCrossing[2]) ||
                             (!IsCrossing[0] && IsCrossing[1] && IsCrossing[2]) ||
                             (IsCrossing[0] && !IsCrossing[1] && IsCrossing[2]))
                    {
                        // 二つの交点によって”３つ”の三角形に分割する
                        Dividing_one_triangle_into_Three_triangles_by_two_intersections(ref triangles, IsCrossing, n, c1, c2);
                    }
                    // パターン⑤ 「交わっていない」
                    else
                    {
                        // 現在の三角形の状態のまま保存
                        for (int cnt = 0; cnt < 3; cnt++)
                            triangles.Add(attachedMesh.triangles[index + cnt]);
                    }
                }
            }

        }


        // メッシュの情報を整理
        DecVertices(ref vertices, ref uvs, ref normals, ref triangles);

        // メッシユ情報の更新
        attachedMesh.vertices = vertices.ToArray();
        attachedMesh.uv = uvs.ToArray();
        attachedMesh.triangles = triangles.ToArray();
        attachedMesh.normals = normals.ToArray();

        debugList = vertices;

        // 切断パスの更新
        for (int index = 0; index < attachedMesh.vertices.Length; index++)
        {
            // 終点座標 == メッシュの頂点の座標
            if (End.Equals(attachedMesh.vertices[index]))
            {
                // 切断パスに登録する
                cuttingPath.Add(End);
                cuttingVertexNumbers.Add(attachedMesh.triangles[index]);
            }
        }


        // 始点と終点を計算する
        // 終点が外周の辺だった場合、オブジェクトを二つに分ける
        bool bCut = SetStartOrEndPoint(tempCrosses, first, End);

        // 切断パスの整理
        DecCuttingPath(ref cuttingPath);

        // 紙の破れ更新
        UpdateBreakLine(bDivide, bCut);

        // 切断パスが切断パスと交差した場合
        if(MakeHole(Start, End))
        {
            // 切断パスをクリア
            cuttingPath.Clear();

            // マウス座標リストをクリア
            MousePoints.Clear();

            return true;
        }

        // カット
        if (bCut)
        {
            // 始点がある場合
            if(exsitStartPoint)
            {
                // オブジェクトを真っ二つに
                Cut();
                // 破れるフラグON
                bDivide = true;
            }

            cuttingPath.Clear();

            // マウス座標リストをクリア
            MousePoints.Clear();
        }

        return bDivide;
    }

    // オブジェクトを二つに分ける
    void Cut()
    {
        List<Vector3> objOutline1 = new List<Vector3>();
        List<Vector3> objOutline2 = new List<Vector3>();
        // このオブジェクトの外周の頂点リストを取得
        var Outlines = GetComponent<OutLinePath>().OutLineVertices;

        var uvs1 = new List<Vector2>();
        var normals1 = new List<Vector3>();
        var uvs2 = new List<Vector2>();
        var normals2 = new List<Vector3>();

        // 始点とぶつかったか終点とぶつかったか
        bool start = true;

        // デバッグ------
        var d = new List<Vector3>();
        int crossNum = 0;
        foreach (var vec in Outlines)
            foreach (var cutting in cuttingPath)
                if (vec.Equals(cutting))
                {
                    crossNum++;
                    d.Add(cutting);
                }
        // 切断パスとアウトラインが3つ以上かぶっている
        if (crossNum != 2)
        {
            foreach (var v in d) Debug.LogWarning(v + "   ");
            Debug.LogError(crossNum);
        }
        //-------------


        #region ---１つ目のアウトラインを作る
        for (int i = 0; i < Outlines.Count; i++)
        {
            // 切断パスと被った場合
            if (cuttingPath[0].Equals(Outlines[i]))
            {
                //Debug.Log("パターン①"); Debug.Log(cuttingPath[0]); Debug.Log(cuttingPath[cuttingPath.Count - 1]);
                // 切断パスの"始点"と外周の頂点が等しいです
                start = true;

                // 切断パスをなぞる 先頭 から 最後尾
                objOutline1.AddRange(cuttingPath);

                // 最後尾と同じアウトラインの座標と被ったらi++やめる
                while (cuttingPath[cuttingPath.Count - 1].Equals(Outlines[i]) == false)
                {
                    // 二つ目のアウトラインに追加しておく（切断パスの先頭、最後尾も含み追加しているはず）
                    objOutline2.Add(Outlines[i]);
                    // 次の座標へ
                    i++;
                    if (i >= Outlines.Count) { Debug.LogError("外周の頂点情報がおかしい"); break; }
                }
                // 二つ目のアウトラインに追加しておく
                objOutline2.Add(Outlines[i]);
            }
            else if (cuttingPath[cuttingPath.Count - 1].Equals(Outlines[i]))
            {
                //Debug.Log("パターン②"); Debug.Log(cuttingPath[0]); Debug.Log(cuttingPath[cuttingPath.Count - 1]);
                // 切断パスの"終点"と外周の頂点が等しいです
                start = false;

                // 切断パスをなぞる 最後尾 から 先頭
                for (int cuttingPathNum = cuttingPath.Count - 1; cuttingPathNum >= 0; cuttingPathNum--)
                    objOutline1.Add(cuttingPath[cuttingPathNum]);

                // 先頭と同じアウトラインの座標と被ったらi++やめる
                while (cuttingPath[0].Equals(Outlines[i]) == false)
                {
                    // 二つ目のアウトラインに追加しておく（切断パスの先頭、最後尾も含み追加しているはず）
                    objOutline2.Add(Outlines[i]);

                    // 次の座標へ
                    i++;
                    if (i >= Outlines.Count) { Debug.LogError("外周の頂点情報がおかしい");  break; }
                }
                // 二つ目のアウトラインに追加しておく
                objOutline2.Add(Outlines[i]);
            }
            else
            {
                // 新しいオブジェクト用のアウトラインリストに追加していく
                objOutline1.Add(Outlines[i]);
            }
        }
        #endregion

        #region ---２つ目のアウトラインを作る
        if (start)
        {
            // 最後尾の１つ前 ～ 先頭の1つ次
            for (int i = cuttingPath.Count - 2; i >= 1; i--)
                objOutline2.Add(cuttingPath[i]);
        }
        else
        {
            // 先頭の1つ次 ～ 最後尾の１つ前
            for (int i = 1; i < cuttingPath.Count - 1; i++)
                objOutline2.Add(cuttingPath[i]);
        }
        #endregion


        

        // エラー対応
        //if (objOutline1.Count < 3) { Debug.LogError("エラー objOutline1.Count"); return; }
        //if (objOutline2.Count < 3) { Debug.LogError("エラー objOutline2.Count"); return; }

        // ---１つ目のuv、法線リストを作成
        for (int i = 0; i < objOutline1.Count; i++)
        {
            for (int index = 0; index < attachedMesh.vertices.Length; index++)
            {
                // アウトラインの頂点座標と頂点群が等しい場合その頂点のuvと法線の値を使う
                if (objOutline1[i].Equals(attachedMesh.vertices[index]))
                {
                    normals1.Add(attachedMesh.normals[index]);
                    uvs1.Add(attachedMesh.uv[index]);
                    break;
                }
            }
        }

        // ---2つ目のuv、法線リストを作成
        for (int i = 0; i < objOutline2.Count; i++)
        {
            for (int index = 0; index < attachedMesh.vertices.Length; index++)
            {
                // アウトラインの頂点座標と頂点群が等しい場合その頂点のuvと法線の値を使う
                if (objOutline2[i].Equals(attachedMesh.vertices[index]))
                {
                    normals2.Add(attachedMesh.normals[index]);
                    uvs2.Add(attachedMesh.uv[index]);
                    break;
                }
            }
        }


        #region １つ目のカットされたオブジェクトを作成
        GameObject obj1 = GetComponent<DrawMesh>().CreateMesh(objOutline1);
        // ---Components
        obj1.AddComponent<DrawMesh>();
        obj1.AddComponent<DivideTriangle>();
        var collider1 = obj1.AddComponent<MeshCollider>();
        var outline1 = obj1.AddComponent<OutLinePath>();
        var meshFilter1 = obj1.GetComponent<MeshFilter>();
        var trun = obj1.AddComponent<Turn_Shader>();
        // ---Settings
        // uv
        meshFilter1.mesh.uv = uvs1.ToArray();
        // 法線
        meshFilter1.mesh.normals = normals1.ToArray();
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
        var trun2 = obj2.AddComponent<Turn_Shader>();
        // ---Settings
        // uv
        meshFilter2.mesh.uv = uvs2.ToArray();
        // 法線
        meshFilter2.mesh.normals = normals2.ToArray();
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
                    if(Has(defaultOutline[(j + 1) % defaultOutline.Count], defaultOutline[j], objOutline1[i]))
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
        if(oldFlag)
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
                    if(Has(defaultOutline[(j + 1) % defaultOutline.Count], defaultOutline[j], objOutline2[i]))
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
        if(oldFlag2)
        {
            // ブレークライン生成
            var line2 = PaperBreakLineManager.Instance.CreateBreakLine(breakLine2, obj2);
            line2.name = "broken paper line";
            line2.GetComponent<LineRendererOperator>().hoge();
        }
        #endregion

        

        // 切断パスをクリア
        cuttingPath.Clear();

        // 現在のオブジェクトを消す
        Destroy(gameObject);



        //--- 原点から中心へのベクトルで飛ばす
        Vector3 pos1 = obj1.GetComponent<Renderer>().bounds.center;
        Vector3 pos2 = obj2.GetComponent<Renderer>().bounds.center;

        //--- 面積が小さい方を飛ばす
        Bounds bounds1 = obj1.GetComponent<MeshFilter>().mesh.bounds;
        Bounds bounds2 = obj2.GetComponent<MeshFilter>().mesh.bounds;
        var width1 = bounds1.max.x - bounds1.min.x;
        var height1 = bounds1.max.y - bounds1.min.y;
        var width2 = bounds2.max.x - bounds2.min.x;
        var height2 = bounds2.max.y - bounds2.min.y;

        if (width1 * height1 < width2 * height2)
        {
            // obj1を飛ばして消す
            //var move = obj1.AddComponent<PaperMove>();
            // 飛ばす方向
            //move.SetDirection(pos1 - pos2);
            // タグの変更（廃棄する紙）
            obj1.tag = "waste";
            // 数秒後にデリート
            Destroy(obj1, 2.0f);

            // ステージの更新
            CollisionField.Instance.UpdateStage(checkCollisionPoints(obj1, CollisionField.Instance.cellPoints()));


            obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;

            var BreakPaper = obj1.AddComponent<BreakingPaper>();
            //alpha.SetMaterial(GetComponent<MeshRenderer>().material);
            BreakPaper.SetMaterial(GameManager.Instance._mats[number - 1]);
            // めくる方向を決める
            if (pos1.x >= 0.0f)
                BreakPaper.SetRight();
            else if (pos1.x < 0.0f)
                BreakPaper.SetLeft();

            // 紙の破れにもAlphaを適用させる
            for (int i = 0; i < obj1.transform.childCount; i++)
            {
                var breakline = obj1.transform.GetChild(i).gameObject.AddComponent<BreakLine>();
                var move2 = obj1.transform.GetChild(i).gameObject.AddComponent<PaperMove>();
                Material breaklineMat = (Material)Resources.Load("Effects/SecondBreakLine");
                breakline.SetMaterial(breaklineMat);
                // めくる方向を決める
                if (pos1.x >= 0.0f)
                {
                    breakline.SetRightLine();
                    //move2.SetDirection(pos1 - pos2);
                }
                else if (pos1.x < 0.0f)
                {
                    breakline.SetLeftLine();
                    //move2.SetDirection(pos1 - pos2);
                }
            }

            //obj1の方のアウトラインをセットする
            //GameObject cursor = GameObject.Find("cursor");
            //cursor.GetComponent<OutSide_Paper_Script_Second>().SetMoveLine(objOutline2, pos2);
        }
        else
        {
            // obj2の方が遠い位置にある
            //var move = obj2.AddComponent<PaperMove>();
            // 飛ばす方向
            //move.SetDirection(pos2 - pos1);
            // タグの変更（廃棄する紙）
            obj2.tag = "waste";
            // 数秒後にデリート
            Destroy(obj2, 3.0f);

            // ステージの更新
            CollisionField.Instance.UpdateStage(checkCollisionPoints(obj2, CollisionField.Instance.cellPoints()));

            var BreakPaper = obj2.AddComponent<BreakingPaper>();
            BreakPaper.SetMaterial(GameManager.Instance._mats[number - 1]);
            //alpha.SetMaterial(GetComponent<MeshRenderer>().material);
            // めくる方向を決める
            if (pos2.x >= 0.0f)
                BreakPaper.SetRight();
            else if (pos2.x < 0.0f)
                BreakPaper.SetLeft();

            // 紙の破れにもAlphaを適用させる
            for (int i = 0; i < obj2.transform.childCount; i++)
            {
                //Vector3 line = new Vector3(400.0f, 100.0f, 0.0f);
                var breakline = obj2.transform.GetChild(i).gameObject.AddComponent<BreakLine>();
                Material breaklineMat = (Material)Resources.Load("Effects/SecondBreakLine");
                breakline.SetMaterial(breaklineMat);
                if (pos2.x >= 0.0f)
                {
                    breakline.SetRightLine();
                    //move.SetDirection(pos2 - pos1 + line);
                }
                else if (pos2.x < 0.0f)
                {
                    breakline.SetLeftLine();
                    //move.SetDirection(pos1 - pos2 + line);
                }
                //breakline.SetAlpha();
            }

            //obj1の方のアウトラインをセットする
            //GameObject cursor = GameObject.Find("cursor");
            //cursor.GetComponent<OutSide_Paper_Script_Second>().SetMoveLine(objOutline1,pos1);

            Partition.CreatePartition(obj1, objOutline1, GetComponent<DrawMesh>(), transform.position.z);
        }

        // 破れるSE
        SoundManager.Instance.PlaySeByName("RipUpPaper07");

    }

    // 現在の破るラインが切断パスと被った場合
    bool MakeHole(Vector3 Start, Vector3 End)
    {
        if (!exsitStartPoint) return false;
        bool ret = false;

        float f1, f2;

        Vector3 currentLine = End - Start;

        // 切断パスと被っているか確認
        for (int edgeIndex = 0; edgeIndex < cuttingPath.Count - 3; edgeIndex++) // cuttingPathのサイズが3以下の場合は何もなし
        {
            // 切断パス
            Vector3 cuttingEdge = cuttingPath[(edgeIndex + 1) % cuttingPath.Count] - cuttingPath[edgeIndex];

            if(CalcCrossVertex(Start, cuttingPath[edgeIndex], currentLine, cuttingEdge, out f1, out f2))
            {
                Vector3 cross = Start + (currentLine * f1);
                Vector2 uv, uv1, uv2;
                Vector3 normal = Vector3.zero;
                uv1 = uv2 = Vector2.zero;
                for (int i = 0; i < attachedMesh.triangles.Length; i++)
                {
                    Vector3 p = attachedMesh.vertices[attachedMesh.triangles[i]];
                    if (p.Equals(cuttingPath[(edgeIndex + 1) % cuttingPath.Count]))
                    {
                        uv2 = attachedMesh.uv[attachedMesh.triangles[i]];
                        normal = attachedMesh.normals[attachedMesh.triangles[i]];
                    }
                }
                for (int i = 0; i < attachedMesh.triangles.Length; i++)
                {
                    Vector3 p = attachedMesh.vertices[attachedMesh.triangles[i]];
                    if (p.Equals(cuttingPath[edgeIndex]))
                    {
                        uv1 = attachedMesh.uv[attachedMesh.triangles[i]];
                    }
                }
                uv = Vector2.Lerp(uv1, uv2, f2);



                return true;
                //Debug.Log("クロス   " + cross + "uv  " + uv);
                //
                //// この場合、切る処理は行うが
                //// 切断パスが共通でないのでCut()関数では正しく動かない
                //// よって、ここでそれぞれのアウトラインを作成する
                //
                //List<Vector3> outline1 = new List<Vector3>();
                //List<Vector3> outline2 = new List<Vector3>();
                //var uvs1 = new List<Vector2>();
                //var normals1 = new List<Vector3>();
                //var uvs2 = new List<Vector2>();
                //var normals2 = new List<Vector3>();
                //
                //var outline = GetComponent<OutLinePath>().OutLineVertices;
                //
                //// 始点に当たるまで外周をなぞる
                //// 始点と重なった時に要素を保存しておく
                //int saveOutlineIndex = 0;
                //for (int index = 0; index < outline.Count; index++)
                //{
                //    // 始点かcheck
                //    if (outline[index].Equals(cuttingPath[0])) { Debug.LogWarning("第1関門突破"); saveOutlineIndex = index; break; }
                //
                //    // obj1に追加①
                //    outline1.Add(outline[index]);
                //}
                //
                //
                //
                //// 交点と２回重なるまで切断パスをなぞる
                //int cnt = 0;
                //for (int index = 0; index < cuttingPath.Count; index++)
                //{
                //    // obj1に追加②
                //    outline1.Add(cuttingPath[index]);
                //
                //    // 交点座標を持っているか
                //    if(Has(cuttingPath[(index + 1) % cuttingPath.Count], cuttingPath[index], cross))
                //    {
                //        cnt++;
                //    }
                //    if (cnt == 2) { Debug.LogWarning("第2関門突破"); break; }
                //}
                //
                //
                //
                //
                //// 始点の位置をずらす
                //Vector3 newStart = cuttingPath[0] + (outline[saveOutlineIndex] - outline[saveOutlineIndex - 1]).normalized * 0.5f;
                //Vector3 newCross = cross + (outline[saveOutlineIndex] - outline[saveOutlineIndex - 1]).normalized * 0.5f;
                //// 交点を追加する
                //outline1.Add(newCross);
                //
                //// edgeIndexの値をもとに逆の順番でなぞる
                //for (int index = edgeIndex; index >= 1; index--)
                //    outline1.Add(cuttingPath[index]);
                //
                //outline1.Add(newStart);
                //
                ////outline1.Add(cuttingPath[0]);
                //
                //// 残りの外周をなぞる
                //for (int index = saveOutlineIndex + 1; index < outline.Count; index++)
                //    outline1.Add(outline[index]);
                //
                //// ---１つ目のuv、法線リストを作成
                //for (int i = 0; i < outline1.Count; i++)
                //{
                //    bool add = false;
                //    for (int index = 0; index < attachedMesh.vertices.Length; index++)
                //    {
                //        // アウトラインの頂点座標と頂点群が等しい場合その頂点のuvと法線の値を使う
                //        if (outline1[i].Equals(attachedMesh.vertices[index]))
                //        {
                //            add = true;
                //            normals1.Add(attachedMesh.normals[index]);
                //            uvs1.Add(attachedMesh.uv[index]);
                //            break;
                //        }
                //        else if(outline1[i].Equals(cross))
                //        {
                //            add = true;
                //            uvs1.Add(uv);
                //            normals1.Add(normal);
                //            break;
                //        }
                //    }
                //
                //    if(!add)
                //    {
                //        Debug.LogWarning("");
                //        uvs1.Add(Vector2.zero);
                //        normals1.Add(Vector3.zero);
                //    }
                //}
                //
                ////Debug.LogError("外周サイズ   " + outline1.Count + "   uvサイズ" + uvs1.Count);
                //
                //#region １つ目のカットされたオブジェクトを作成
                //GameObject obj1 = GetComponent<DrawMesh>().CreateMesh(outline1);
                //// ---Components
                //obj1.AddComponent<DrawMesh>();
                //obj1.AddComponent<DivideTriangle>();
                //var collider1 = obj1.AddComponent<MeshCollider>();
                //var outlinePath1 = obj1.AddComponent<OutLinePath>();
                //var meshFilter1 = obj1.GetComponent<MeshFilter>();
                //var trun = obj1.AddComponent<Turn_Shader>();
                //// ---Settings
                //// uv
                //meshFilter1.mesh.uv = uvs1.ToArray();
                //// 法線
                //meshFilter1.mesh.normals = normals1.ToArray();
                //// メッシュコライダーにメッシュをセット
                //collider1.sharedMesh = meshFilter1.mesh;
                //// アウトラインを格納
                //outlinePath1.UpdateOutLine(outline1);
                //// タグ
                //obj1.tag = "paper";
                //// 現在のマテリアルを受け継ぐ
                //obj1.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
                //// レイヤー番号も引き継ぐ
                //obj1.GetComponent<DivideTriangle>().number = this.number;
                //// z座標を引き継ぐ
                //obj1.transform.position += new Vector3(0, 0, transform.position.z);
                //#endregion
                //
                //Destroy(gameObject);
                //
                //foreach (var vec in outline1)
                //    Debug.Log(vec);
                //
                //ret = true;
            }

        }

        return ret;
    }

    // 線分上に点が存在するか
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

    // 終点のuv座標を計算する
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


    // 外周と切断パスを比較し、始点と終点を計算する
    // 戻り値 : 切断パスの終点が設定されたらtrue
    private bool SetStartOrEndPoint(List<Vector3> crossVertices, bool first, Vector3 EndPoint)
    {
        // アウトラインの線上に交点が存在するか調べる
        // 存在する場合は、その交点が"切断パスの始点"又は"切断パスの終点となる"
        // 終点の場合は、切断フラグをtrueに


        // きるフラグ
        bool cut = false;
        // 外周上の交点の数
        int findNum = 0;
        // 始点、終点の頂点番号
        int nStart, nEnd;
        // 始点、終点の座標
        Vector3 start, end;

        // 初期化
        nStart = nEnd = -1;
        start = end = Vector3.zero;

        // 現在の外周の頂点
        var outlinePath = GetComponent<OutLinePath>();
        var outline = outlinePath.OutLineVertices;

        
        for (int i = 0; i < outline.Count; i++)     // オブジェクトの外周の頂点の数
        {
            // 外周の辺のベクトル
            Vector3 outlineEdge = outline[(i + 1) % (outline.Count)] - outline[i];

            for (int j = 0; j < crossVertices.Count; j++)   // 今回の交点の数
            {
                // 外周の頂点-交点ベクトル
                Vector3 vec = crossVertices[j] - outline[i];

                // 2つのベクトルの外積が0だったら、外周上の点である
                float judge = Vector3.Cross(outlineEdge.normalized, vec.normalized).sqrMagnitude;

                // 0.0001未満の誤差を許す
                if (-0.0001f < judge && judge < 0.00001f)
                {

                    // 辺よりvecの方が大きい場合外分点
                    if (Has(outline[(i + 1) % (outline.Count)], outline[i], crossVertices[j]))
                    {
                        // 初回呼び出しの場合、その交点は切断パスの始点とみなす
                        if (first)
                        {
                            // 外周上の点だった場合、カウントする
                            findNum++;

                            // 初回呼び出しで外周上の頂点が2つある場合、切断パスの終点も設定する
                            if (findNum == 1)
                            {
                                //Debug.LogWarning(outline[i] + "  " + outline[(i + 1) % (outline.Count)] + "     " + crossVertices[j] + "  始点");
                                // 始点追加
                                cuttingPath.Insert(0, crossVertices[j]);
                                // アウトラインの番号を保存
                                nStart = i + 1;

                                exsitStartPoint = true;
                            }
                            else if (findNum == 2)
                            {
                                //Debug.LogWarning(outline[i] + "  " + outline[(i + 1) % (outline.Count)] + "     " + crossVertices[j] + "  終点");
                                //Debug.Log("１度で切りました");

                                // 終点追加
                                cuttingPath.Add(crossVertices[j]);
                                // アウトラインの番号を保存
                                nEnd = i + 2;
                                // 切断フラグ
                                cut = true;
                            }
                            else
                            {
                                //Debug.LogWarning(outline[i] + "  " + outline[(i + 1) % (outline.Count)] + "     " + crossVertices[j] + "  外分点");
                                Debug.LogWarning("3個目の外周上の点が見つかりました");
                                return false;
                            }

                            cuttingVertexNumbers.Add(0);

                        }
                        // 初回呼び出しではない場合、その交点は切断パスの終点とみなす
                        else
                        {
                            //Debug.LogWarning(outline[i] + "  " + outline[(i + 1) % (outline.Count)] + "     " + crossVertices[j] + "  終点");
                            // 終点追加
                            cuttingPath.Add(crossVertices[j]);
                            // アウトラインの番号を保存
                            nEnd = i + 1;
                            // 切断フラグ
                            cut = true;
                        }
                    }
                }
            }
        }

        // 始点or終点が見つかった場合
        // ここでアウトラインも追加したい(順番通りに)
        if (nStart != -1)
        {
            // 切断パスが追加されている
            outlinePath.InsertPoint(cuttingPath[0], nStart);
        }

        if (nEnd != -1)
        {
            outlinePath.InsertPoint(cuttingPath[cuttingPath.Count - 1], nEnd);
        }

        return cut;
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

    // 紙の破れを生成 又は 更新する
    private void UpdateBreakLine(bool bDivide, bool bCut)
    {
        // 破るフラグが立っている場合
        // かつ、このオブジェクトの子オブジェクトにpaper breakが存在しない場合
        // 紙の破れを生成する
        // paper break が存在している場合はやぶれを更新する
        if (bDivide || bCut)
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
                    breakLine.gameObject.GetComponent<LineRendererOperator>().SetPoints(cuttingPath);
                }
                // 破れが存在しない場合、新しく生成する
                else
                {
                    PaperBreakLineManager.Instance.CreateBreakLine(cuttingPath, gameObject);
                }
            }
            else
            {
                // 破れが存在しない場合、新しく生成する
                PaperBreakLineManager.Instance.CreateBreakLine(cuttingPath, gameObject);
            }
        }
    }

    // 番号の設定
    public void SetNumber(int num) { number = num; }
    // 番号の取得
    public int GetNumber() { return number; }


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
    * @fn		CalcCrossVertex
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

        //線上は外側とみなします。
        Vector3 Area1 = p2 - p1; Vector3 AreaP1 = checkPoint - p1;
        Vector3 Area2 = p3 - p2; Vector3 AreaP2 = checkPoint - p2;
        Vector3 Area3 = p1 - p3; Vector3 AreaP3 = checkPoint - p3;
        Vector3 cr1 = Vector3.Cross(Area1, AreaP1);
        Vector3 cr2 = Vector3.Cross(Area2, AreaP2);
        Vector3 cr3 = Vector3.Cross(Area3, AreaP3);
        //内積で順方向か逆方向か調べる
        float dot_12 = Vector3.Dot(cr1, cr2);
        float dot_13 = Vector3.Dot(cr1, cr3);
        if (dot_12 > 0 && dot_13 > 0)
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

    // 1つの三角形を”４つ”の三角形に分割する
    void Dividing_one_triangle_into_Four_triangles(ref List<int> triangles, bool[] IsCrossing, int[] triangle, int center, int cross)
    {
        // 三角形の頂点座標３つ、「交差した座標」、「終点」の計5つの頂点で三角形を「３つ」作る

        int num = 0;
        // 頂点のつなげ方を決める

        // p1-p2の辺1が交差していた場合
        if (IsCrossing[0]) num = 0;
        // p2-p3の辺1が交差していた場合
        else if (IsCrossing[1]) num = 1;
        // p3-p1の辺1が交差していた場合
        else if (IsCrossing[2]) num = 2;
        else Debug.LogError("エラー発生");
        

        // １個目の三角形
        triangles.Add(center); triangles.Add(triangle[Percent3(num + 0)]); triangles.Add(cross);
        // ２個目の三角形
        triangles.Add(center); triangles.Add(cross); triangles.Add(triangle[Percent3(num + 1)]);
        // ３個目の三角形
        triangles.Add(center); triangles.Add(triangle[Percent3(num + 1)]); triangles.Add(triangle[Percent3(num + 2)]);
        // ４個目の三角形
        triangles.Add(center); triangles.Add(triangle[Percent3(num + 2)]); triangles.Add(triangle[Percent3(num + 0)]);
    }


    // 1つの三角形を”二つの交点によって３つ”の三角形に分割する
    void Dividing_one_triangle_into_Three_triangles_by_two_intersections(ref List<int> triangles, bool[] IsCrossing, int[] triangle, int cross1, int cross2)
    {
        // 三角形の頂点座標３つ、交差した座標２つの計5つの頂点で三角形を「３つ」作る


        // 辺１、辺２で交差
        if (IsCrossing[0] && IsCrossing[1])
        {
            //Debug.Log("辺p1-p2と辺p2-p3で交わっている");
        
            // １個目の三角形
            triangles.Add(cross2); triangles.Add(cross1); triangles.Add(triangle[1]);
            // ２個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[0]); triangles.Add(cross1);
            // ３個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[2]); triangles.Add(triangle[0]);
        }
        // 辺２、辺３で交差
        else if (IsCrossing[1] && IsCrossing[2])
        {
            //Debug.Log("辺p2-p3と辺p3-p1で交わっている");
        
            // １個目の三角形
            triangles.Add(cross2); triangles.Add(cross1); triangles.Add(triangle[2]);
            // ２個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[1]); triangles.Add(cross1);
            // ３個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[0]); triangles.Add(triangle[1]);
        }
        // 辺３、辺１で交差
        else if (IsCrossing[2] && IsCrossing[0])
        {
            //Debug.Log("辺p3-p1と辺p1-p2で交わっている");
            // １個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[0]); triangles.Add(cross1);
            // ２個目の三角形
            triangles.Add(cross2); triangles.Add(cross1); triangles.Add(triangle[1]);
            // ３個目の三角形
            triangles.Add(cross2); triangles.Add(triangle[1]); triangles.Add(triangle[2]);
        }
        else Debug.LogError("エラー発生");
    }


    // 1つの三角形を”点を基準として３つ”の三角形に分割する
    void Dividing_one_triangle_into_Three_triangles_by_center(ref List<int> triangles, int[] triangle, int center)
    {
        // 頂点のつなげ方を決める
        // １個目の三角形
        triangles.Add(center); triangles.Add(triangle[0]); triangles.Add(triangle[1]);
        // ２個目の三角形
        triangles.Add(center); triangles.Add(triangle[1]); triangles.Add(triangle[2]);
        // ３個目の三角形
        triangles.Add(center); triangles.Add(triangle[2]); triangles.Add(triangle[0]);
    }


    // 1つの三角形を”一つの交点によって２つ”の三角形に分割する
    void Dividing_one_triangle_into_Two_triangles_by_intersection(ref List<int> triangles, bool[] IsCrossing, int[] triangle, int cross)
    {
        int num = 0;
        if (IsCrossing[0]) num = 0;
        if (IsCrossing[1]) num = 1;
        if (IsCrossing[2]) num = 2;

        // １つ目
        triangles.Add(cross); triangles.Add(triangle[Percent3(num + 2)]); triangles.Add(triangle[Percent3(num + 0)]);
        // ２つ目
        triangles.Add(cross); triangles.Add(triangle[Percent3(num + 1)]); triangles.Add(triangle[Percent3(num + 2)]);
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