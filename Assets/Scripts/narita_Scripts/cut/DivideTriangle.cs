using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideTriangle : MonoBehaviour
{
    [SerializeField]
    private Material _material;
    [SerializeField]
    private int number = 0;

    MeshFilter attachedMeshFilter;
    Mesh attachedMesh;

    [SerializeField]
    List<Vector3> cuttingPath;      // 断面
    [SerializeField]
    List<int> cuttingVertexNumbers; // 頂点番号

    [SerializeField]
    List<Vector3> debugList = new List<Vector3>();
    

    void Start()
    {
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        cuttingPath = new List<Vector3>();
        cuttingVertexNumbers = new List<int>();
    }

    /* 破る処理
     * param Start  破る線分の始点
     * param End    破る線分の終点
     * param cutObject 破る対象
     * return       メッシュを一度でも三角形分割していたらtrue
     */
    public bool Divide(ref List<Vector3> MousePoints)
    {
        if (MousePoints.Count <= 1) return false;
        // 線分の始点・終点
        Vector3 Start, End;
        Start = MousePoints[MousePoints.Count - 2];
        End = MousePoints[MousePoints.Count - 1];

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
        Vector3 p1, p2, p3;
        // 辺と交わっているかどうか
        bool[] IsCrossing = new bool[3];
        // 線分の終点(End)が内側にあるかどうか
        bool bInside;

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
        List<Vector3> crossVertices = new List<Vector3>();
        List<int> crossVertexNumbers = new List<int>();


        // ---三角形の頂点から始めるかどうか
        first = true;
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //三角形の頂点を取得
            p1 = attachedMesh.vertices[attachedMesh.triangles[i]];
            p2 = attachedMesh.vertices[attachedMesh.triangles[i + 1]];
            p3 = attachedMesh.vertices[attachedMesh.triangles[i + 2]];

            if (Start.Equals(p1) || Start.Equals(p2) || Start.Equals(p3))
            {
                // 始点がメッシュの頂点の座標と等しい
                first = false;
            }
        }

        // １回目
        if (first == true)
        {
            // i=0の三角形 0,1,2    i=3の三角形 3,4,5   i=6の三角形 6,7,8
            for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
            {
                // 交差したかのフラグを初期化
                IsCrossing[0] = IsCrossing[1] = IsCrossing[2] = false;
                cross1 = cross2 = Vector3.zero;
                bInside = true;

                //三角形の頂点を取得
                p1 = attachedMesh.vertices[attachedMesh.triangles[i]];
                p2 = attachedMesh.vertices[attachedMesh.triangles[i + 1]];
                p3 = attachedMesh.vertices[attachedMesh.triangles[i + 2]];
                // 三角形の頂点番号取得
                int n1 = attachedMesh.triangles[i];
                int n2 = attachedMesh.triangles[i + 1];
                int n3 = attachedMesh.triangles[i + 2];

                #region ---①三角形と線分との「交点」を求める
                for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                {
                    // 線分のベクトル、辺のベクトル、始点１から始点２までのベクトル
                    Vector3 v1, v2, v;
                    // 交点までの距離の比率
                    float t1, t2;
                    // 線分のベクトル(マウスで決めた2点間のベクトルのため固定)
                    v1 = End - Start;

                    // 調べる辺を決める
                    switch (nVertexNum)
                    {
                        case 0: // ①p1-p2の辺をまたいでいるか
                            v = p1 - Start; // 始点１から始点２までのベクトル
                            v2 = p2 - p1;   // 頂点１から頂点２までのベクトル
                            break;
                        case 1: // ②p2-p3の辺をまたいでいるか
                            v = p2 - Start;
                            v2 = p3 - p2;   // 頂点２から頂点３までのベクトル
                            break;
                        case 2: // ③p3-p1の辺をまたいでいるか
                            v = p3 - Start;
                            v2 = p1 - p3;   // 頂点３から頂点１までのベクトル
                            break;

                        default: continue;
                    }

                    // それぞれの線分の始点から交点になりうる座標までのベクトルの比率を求める
                    t1 = Vector3.Cross(v, v2).z / Vector3.Cross(v1, v2).z;
                    t2 = Vector3.Cross(v, v1).z / Vector3.Cross(v1, v2).z;

                    // 交差判定
                    if ((0 <= t1 && t1 <= 1) && (0 <= t2 && t2 <= 1))
                    {
                        // ２つ目の交点の場合
                        if (IsCrossing[0] == true || IsCrossing[1] == true)
                        {
                            // ２つ目の交点の座標を求める
                            cross2 = Start + (t1 * v1);

                            // 交差フラグON
                            IsCrossing[nVertexNum] = true;
                            // 交点と始点が同じ位置の場合、交差判定を取り消す
                            if (Start.Equals(cross2)) IsCrossing[nVertexNum] = false;

                            // 頂点情報がかぶっていない場合、頂点リストに追加
                            bool sameVertex = false;
                            for (int n = 0; n < crossVertices.Count; n++)
                            {
                                if (crossVertices[n].Equals(cross2))
                                {
                                    // かぶっていた場合、元からある頂点の番号に設定する
                                    c2 = crossVertexNumbers[n];
                                    sameVertex = true;
                                    break;
                                }
                            }
                            if (!sameVertex)
                            {
                                // 交点2を追加
                                vertices.Add(cross2);
                                uvs.Add(CalcUV(nVertexNum, i, t2));
                                normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                                c2 = vertices.Count - 1;

                                // 交点を保存しておく
                                crossVertices.Add(cross2);
                                crossVertexNumbers.Add(c2);
                            }

                        }
                        // １つ目の交点の場合
                        else
                        {
                            // 交点を求める
                            cross1 = Start + (t1 * v1);
                            // 交差フラグON
                            IsCrossing[nVertexNum] = true;
                            // 交点と始点が同じ位置の場合、交差判定を取り消す
                            if (Start.Equals(cross1)) IsCrossing[nVertexNum] = false;

                            // 頂点情報がかぶっていない場合、頂点リストに追加
                            bool sameVertex = false;
                            for (int n = 0; n < crossVertices.Count; n++)
                            {
                                if (crossVertices[n].Equals(cross1))
                                {
                                    // かぶっていた場合、元からある頂点の番号に設定する
                                    c1 = crossVertexNumbers[n];
                                    sameVertex = true;
                                    break;
                                }
                            }
                            if (!sameVertex)
                            {
                                // 頂点リストにを追加
                                vertices.Add(cross1);
                                uvs.Add(CalcUV(nVertexNum, i, t2));
                                normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                                c1 = vertices.Count - 1;

                                // 交点を保存しておく
                                crossVertices.Add(cross1);
                                crossVertexNumbers.Add(c1);
                            }
                        }
                    }
                }
                #endregion

                #region ---②終点が三角形の内部にあるかないか判定
                //線上は外側とみなします。
                Vector3 Area1 = p2 - p1; Vector3 AreaP1 = End - p1;
                Vector3 Area2 = p3 - p2; Vector3 AreaP2 = End - p2;
                Vector3 Area3 = p1 - p3; Vector3 AreaP3 = End - p3;
                Vector3 cr1 = Vector3.Cross(Area1, AreaP1);
                Vector3 cr2 = Vector3.Cross(Area2, AreaP2);
                Vector3 cr3 = Vector3.Cross(Area3, AreaP3);
                //内積で順方向か逆方向か調べる
                float dot_12 = Vector3.Dot(cr1, cr2);
                float dot_13 = Vector3.Dot(cr1, cr3);
                if (dot_12 > 0 && dot_13 > 0)
                {
                    //***** 内側
                    bInside = true;

                    bDivide = true;

                    bool sameVertex = false;
                    for (int n = 0; n < crossVertices.Count; n++)
                    {
                        if (crossVertices[n].Equals(End))
                        {
                            // かぶっていた場合、元からある頂点の番号に設定する
                            e = crossVertexNumbers[n];
                            sameVertex = true;
                            break;
                        }
                    }
                    if(!sameVertex)
                    {
                        // 終点を追加
                        vertices.Add(End);
                        uvs.Add(CalcEndingPointUV(i, End));
                        normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                        e = vertices.Count - 1;
                    }
                }
                else
                {
                    //***** 外側
                    bInside = false;
                }
                #endregion

                #region ---③終点が「内側」の処理
                if (bInside == true)
                {
                    //*** 「交差していない」
                    if (!IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2])
                    {
                        // 現在の三角形の状態のまま保存
                        for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                        {
                            triangles.Add(attachedMesh.triangles[i + nVertexNum]);
                        }
                    }
                    //*** 「１辺」と交わっている
                    else
                    {
                        // 三角形の頂点座標３つ、「交差した座標」、「終点」の計5つの頂点で三角形を「３つ」作る

                        // 頂点のつなげ方を決める
                        if (IsCrossing[0])
                        {
                            //Debug.Log("p1-p2の辺が交差していいる");
                            // p1-p2の辺1が交差していた場合

                            // １個目の三角形
                            triangles.Add(e); triangles.Add(n1); triangles.Add(c1);
                            // ２個目の三角形
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n2);
                            // ３個目の三角形
                            triangles.Add(e); triangles.Add(n2); triangles.Add(n3);
                            // ４個目の三角形
                            triangles.Add(e); triangles.Add(n3); triangles.Add(n1);
                        }
                        else if (IsCrossing[1])
                        {
                            //Debug.Log("p2-p3の辺が交差している");

                            // １個目の三角形
                            triangles.Add(e); triangles.Add(n2); triangles.Add(c1);
                            // ２個目の三角形
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n3);
                            // ３個目の三角形
                            triangles.Add(e); triangles.Add(n3); triangles.Add(n1);
                            // ４個目の三角形
                            triangles.Add(e); triangles.Add(n1); triangles.Add(n2);
                        }
                        else if (IsCrossing[2])
                        {
                            //Debug.Log("p3-p1の辺が交差している");

                            // １個目の三角形
                            triangles.Add(e); triangles.Add(n3); triangles.Add(c1);
                            // ２個目の三角形
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n1);
                            // ３個目の三角形
                            triangles.Add(e); triangles.Add(n1); triangles.Add(n2);
                            // ４個目の三角形
                            triangles.Add(e); triangles.Add(n2); triangles.Add(n3);
                        }
                        else
                        {
                            Debug.LogError("エラー発生");
                            triangles.Add(n1); triangles.Add(n2); triangles.Add(n3);
                        }
                    }
                }
                #endregion

                #region ---④終点が「外側」の処理
                if (bInside == false)
                {
                    //*** 線分が三角形の「２辺」と交わっている
                    if ((IsCrossing[0] && IsCrossing[1]) || (IsCrossing[1] && IsCrossing[2]) || (IsCrossing[2] && IsCrossing[0]))
                    {
                        // 三角形の頂点座標３つ、交差した座標２つの計5つの頂点で三角形を「３つ」作る

                        // 頂点のつなぎ方を決める
                        if (IsCrossing[0] && IsCrossing[1])
                        {
                            //Debug.Log("辺p1-p2と辺p2-p3で交わっている");

                            // １個目の三角形
                            triangles.Add(c2); triangles.Add(c1); triangles.Add(n2);
                            // ２個目の三角形
                            triangles.Add(c2); triangles.Add(n1); triangles.Add(c1);
                            // ３個目の三角形
                            triangles.Add(c2); triangles.Add(n3); triangles.Add(n1);
                        }
                        else if (IsCrossing[1] && IsCrossing[2])
                        {
                            //Debug.Log("辺p2-p3と辺p3-p1で交わっている");

                            // １個目の三角形
                            triangles.Add(c2); triangles.Add(c1); triangles.Add(n3);
                            // ２個目の三角形
                            triangles.Add(c2); triangles.Add(n2); triangles.Add(c1);
                            // ３個目の三角形
                            triangles.Add(c2); triangles.Add(n1); triangles.Add(n2);
                        }
                        else if (IsCrossing[2] && IsCrossing[0])
                        {
                            //Debug.Log("辺p3-p1と辺p1-p2で交わっている");
                            // １個目の三角形
                            triangles.Add(c2); triangles.Add(n1); triangles.Add(c1);
                            // ２個目の三角形
                            triangles.Add(c2); triangles.Add(c1); triangles.Add(n2);
                            // ３個目の三角形
                            triangles.Add(c2); triangles.Add(n2); triangles.Add(n3);
                        }
                    }
                    //*** 線分が三角形と「交わっていない」
                    else
                    {
                        // 現在の三角形の状態のまま保存
                        for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                        {
                            triangles.Add(attachedMesh.triangles[i + nVertexNum]);
                        }
                    }
                }
                #endregion

            }
        }
        // 2回目から(始点が頂点)
        else
        {
            // i=0の三角形 0,1,2    i=3の三角形 3,4,5   i=6の三角形 6,7,8
            for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
            {
                // 交差したかのフラグを初期化
                IsCrossing[0] = IsCrossing[1] = IsCrossing[2] = false;
                cross1 = cross2 = Vector3.zero;
                bInside = true;

                //三角形の頂点を取得
                p1 = attachedMesh.vertices[attachedMesh.triangles[i]];
                p2 = attachedMesh.vertices[attachedMesh.triangles[i + 1]];
                p3 = attachedMesh.vertices[attachedMesh.triangles[i + 2]];
                // 三角形の頂点番号取得
                int n1 = attachedMesh.triangles[i];
                int n2 = attachedMesh.triangles[i + 1];
                int n3 = attachedMesh.triangles[i + 2];

                #region ---①三角形と線分との「交点」を求める
                for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                {
                    // 線分のベクトル、辺のベクトル、始点１から始点２までのベクトル
                    Vector3 v1, v2, v;
                    // 交点までの距離の比率
                    float t1, t2;
                    // 線分のベクトル(マウスで決めた2点間のベクトルのため固定)
                    v1 = End - Start;

                    // 調べる辺を決める
                    switch (nVertexNum)
                    {
                        case 0: // ①p1-p2の辺をまたいでいるか
                            v = p1 - Start; // 始点１から始点２までのベクトル
                            v2 = p2 - p1;   // 頂点１から頂点２までのベクトル
                            break;
                        case 1: // ②p2-p3の辺をまたいでいるか
                            v = p2 - Start;
                            v2 = p3 - p2;   // 頂点２から頂点３までのベクトル
                            break;
                        case 2: // ③p3-p1の辺をまたいでいるか
                            v = p3 - Start;
                            v2 = p1 - p3;   // 頂点３から頂点１までのベクトル
                            break;

                        default: continue;
                    }

                    // それぞれの線分の始点から交点になりうる座標までのベクトルの比率を求める
                    t1 = Vector3.Cross(v, v2).z / Vector3.Cross(v1, v2).z;
                    t2 = Vector3.Cross(v, v1).z / Vector3.Cross(v1, v2).z;

                    // 交差判定
                    if ((0 <= t1 && t1 <= 1) && (0 <= t2 && t2 <= 1))
                    {
                        // ２つ目の交点の場合
                        if (IsCrossing[0] == true || IsCrossing[1] == true)
                        {
                            // ２つ目の交点の座標を求める
                            cross2 = Start + (t1 * v1);

                            // 交差フラグON
                            IsCrossing[nVertexNum] = true;
                            // 交点と始点が同じ位置の場合、交差判定を取り消す
                            if (Start.Equals(cross2))
                            {
                                IsCrossing[nVertexNum] = false;

                                // 切断パスと被っているはずなので
                                for (int n = 0; n < cuttingPath.Count; n++)
                                {
                                    if (cuttingPath[n].Equals(cross2))
                                    {
                                        crossVertices.Add(cuttingPath[n]);
                                        crossVertexNumbers.Add(cuttingVertexNumbers[n]);
                                        break;
                                    }
                                }
                            }

                            // 頂点情報がかぶっていない場合、頂点リストに追加
                            bool sameVertex = false;
                            for (int n = 0; n < crossVertices.Count; n++)
                            {
                                if (crossVertices[n].Equals(cross2))
                                {
                                    // かぶっていた場合、元からある頂点の番号に設定する
                                    c2 = crossVertexNumbers[n];
                                    sameVertex = true;
                                    break;
                                }
                            }
                            // かぶっていなかったら追加
                            if (!sameVertex)
                            {
                                // 交点2を追加
                                vertices.Add(cross2);
                                uvs.Add(CalcUV(nVertexNum, i, t2));
                                normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                                c2 = vertices.Count - 1;

                                // 交点を保存しておく
                                crossVertices.Add(cross2);
                                crossVertexNumbers.Add(c2);

                                Debug.Log(cross2);
                            }

                        }
                        // １つ目の交点の場合
                        else
                        {
                            // 交点を求める
                            cross1 = Start + (t1 * v1);
                            // 交差フラグON
                            IsCrossing[nVertexNum] = true;
                            // 交点と始点が同じ位置の場合、交差判定を取り消す
                            if (Start.Equals(cross1))
                            {
                                IsCrossing[nVertexNum] = false;

                                // 切断パスと被っているはずなので
                                for (int n = 0; n < cuttingPath.Count; n++)
                                {
                                    if (cuttingPath[n].Equals(cross1))
                                    {
                                        crossVertices.Add(cuttingPath[n]);
                                        crossVertexNumbers.Add(cuttingVertexNumbers[n]);
                                        break;
                                    }
                                }
                            }

                            bool sameVertex = false;
                            // 頂点情報がかぶっていない場合、頂点リストに追加
                            for (int n = 0; n < crossVertices.Count; n++)
                            {
                                if (crossVertices[n].Equals(cross1))
                                {
                                    // かぶっていた場合、元からある頂点の番号に設定する
                                    c1 = crossVertexNumbers[n];
                                    sameVertex = true;
                                    break;
                                }
                            }
                            if (!sameVertex)
                            {
                                // 頂点リストにを追加
                                vertices.Add(cross1);
                                uvs.Add(CalcUV(nVertexNum, i, t2));
                                normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                                c1 = vertices.Count - 1;

                                // 交点を保存しておく
                                crossVertices.Add(cross1);
                                crossVertexNumbers.Add(c1);

                                Debug.Log(cross1);
                            }
                        }
                    }
                }
                #endregion

                #region ---②終点が三角形の内部にあるかないか判定
                //線上は外側とみなします。
                Vector3 Area1 = p2 - p1; Vector3 AreaP1 = End - p1;
                Vector3 Area2 = p3 - p2; Vector3 AreaP2 = End - p2;
                Vector3 Area3 = p1 - p3; Vector3 AreaP3 = End - p3;
                Vector3 cr1 = Vector3.Cross(Area1, AreaP1);
                Vector3 cr2 = Vector3.Cross(Area2, AreaP2);
                Vector3 cr3 = Vector3.Cross(Area3, AreaP3);
                //内積で順方向か逆方向か調べる
                float dot_12 = Vector3.Dot(cr1, cr2);
                float dot_13 = Vector3.Dot(cr1, cr3);
                if (dot_12 > 0 && dot_13 > 0)
                {
                    //***** 内側
                    bInside = true;

                    bDivide = true;

                    bool sameVertex = false;
                    for (int n = 0; n < crossVertices.Count; n++)
                    {
                        if (crossVertices[n].Equals(End))
                        {
                            // かぶっていた場合、元からある頂点の番号に設定する
                            e = crossVertexNumbers[n];
                            sameVertex = true;
                            break;
                        }
                    }
                    if (!sameVertex)
                    {
                        // 終点を追加
                        vertices.Add(End);
                        uvs.Add(CalcEndingPointUV(i, End));
                        normals.Add(attachedMesh.normals[attachedMesh.triangles[i]]);
                        e = vertices.Count - 1;
                    }
                }
                else
                {
                    //***** 外側
                    bInside = false;
                }
                #endregion

                #region ---③終点が「内側」の処理
                if (bInside == true)
                {
                    // パターン① 「交わっていない」
                    if (!IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2])
                    {
                        //Debug.Log("パターン①");

                        // 頂点のつなげ方を決める
                        // １個目の三角形
                        triangles.Add(e); triangles.Add(n1); triangles.Add(n2);
                        // ２個目の三角形
                        triangles.Add(e); triangles.Add(n2); triangles.Add(n3);
                        // ３個目の三角形
                        triangles.Add(e); triangles.Add(n3); triangles.Add(n1);
                    }
                    // パターン② 「１辺」と交わっている
                    else if (IsCrossing[0] || IsCrossing[1] || IsCrossing[2])
                    {
                        //Debug.Log("パターン②");

                        // 頂点のつなげ方を決める
                        if (IsCrossing[0])
                        {
                            // １つ目
                            triangles.Add(e); triangles.Add(n1); triangles.Add(c1);
                            // ２つ目
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n2);
                            // ３つ目
                            triangles.Add(e); triangles.Add(n2); triangles.Add(n3);
                            // ４つ目
                            triangles.Add(e); triangles.Add(n3); triangles.Add(n1);
                        }
                        else if (IsCrossing[1])
                        {
                            // １つ目
                            triangles.Add(e); triangles.Add(n2); triangles.Add(c1);
                            // ２つ目
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n3);
                            // ３つ目
                            triangles.Add(e); triangles.Add(n3); triangles.Add(n1);
                            // ４つ目
                            triangles.Add(e); triangles.Add(n1); triangles.Add(n2);
                        }
                        else if (IsCrossing[2])
                        {
                            // １つ目
                            triangles.Add(e); triangles.Add(n3); triangles.Add(c1);
                            // ２つ目
                            triangles.Add(e); triangles.Add(c1); triangles.Add(n1);
                            // ３つ目
                            triangles.Add(e); triangles.Add(n1); triangles.Add(n2);
                            // ４つ目
                            triangles.Add(e); triangles.Add(n2); triangles.Add(n3);
                        }
                    }
                    // 例外パターン
                    else
                    {
                        Debug.Log("エラー");
                        // 現在の三角形の状態のまま保存
                        for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                        {
                            triangles.Add(attachedMesh.triangles[i + nVertexNum]);
                        }
                    }
                }
                #endregion

                #region ---④終点が「外側」の処理
                else if (bInside == false)
                {
                    // パターン③ １辺だけが交わっている
                    if ((IsCrossing[0] && !IsCrossing[1] && !IsCrossing[2]) ||
                        (!IsCrossing[0] && IsCrossing[1] && !IsCrossing[2]) ||
                        (!IsCrossing[0] && !IsCrossing[1] && IsCrossing[2]))
                    {
                        // 三角形を「２つ」に分ける
                        //Debug.Log("パターン③");


                        // 頂点のつなげ方を決める
                        if (IsCrossing[0])
                        {
                            // １つ目
                            triangles.Add(c1); triangles.Add(n3); triangles.Add(n1);
                            // ２つ目
                            triangles.Add(c1); triangles.Add(n2); triangles.Add(n3);
                        }
                        else if (IsCrossing[1])
                        {
                            // １つ目
                            triangles.Add(c1); triangles.Add(n1); triangles.Add(n2);
                            // ２つ目
                            triangles.Add(c1); triangles.Add(n3); triangles.Add(n1);
                        }
                        else if (IsCrossing[2])
                        {
                            // １つ目
                            triangles.Add(c1); triangles.Add(n2); triangles.Add(n3);
                            // ２つ目
                            triangles.Add(c1); triangles.Add(n1); triangles.Add(n2);
                        }
                    }
                    // パターン④ ２辺が交わっているとき
                    else if ((IsCrossing[0] && IsCrossing[1] && !IsCrossing[2]) ||
                             (!IsCrossing[0] && IsCrossing[1] && IsCrossing[2]) ||
                             (IsCrossing[0] && !IsCrossing[1] && IsCrossing[2]))
                    {
                        // 三角形を「3つ」に分ける
                        //Debug.Log("パターン④");

                        // 頂点のつなげ方を決める
                        if (IsCrossing[0] && IsCrossing[1])
                        {
                            // １つ目
                            triangles.Add(c1); triangles.Add(n2); triangles.Add(c2);
                            // ２つ目
                            triangles.Add(c1); triangles.Add(c2); triangles.Add(n1);
                            // 3つ目
                            triangles.Add(c2); triangles.Add(n3); triangles.Add(n1);
                        }
                        else if (IsCrossing[1] && IsCrossing[2])
                        {
                            // １つ目
                            triangles.Add(c1); triangles.Add(n3); triangles.Add(c2);
                            // ２つ目
                            triangles.Add(c1); triangles.Add(c2); triangles.Add(n2);
                            // 3つ目
                            triangles.Add(c2); triangles.Add(n1); triangles.Add(n2);
                        }
                        else if (IsCrossing[2] && IsCrossing[0])
                        {
                            // １つ目
                            triangles.Add(n1); triangles.Add(c1); triangles.Add(c2);
                            // ２つ目
                            triangles.Add(c2); triangles.Add(c1); triangles.Add(n2);
                            // 3つ目
                            triangles.Add(c2); triangles.Add(n2); triangles.Add(n3);
                        }
                    }
                    // パターン⑤ 「交わっていない」
                    else
                    {
                        //Debug.Log("パターン⑤");
                        // 現在の三角形の状態のまま保存
                        for (int nVertexNum = 0; nVertexNum < 3; nVertexNum++)
                        {
                            triangles.Add(attachedMesh.triangles[i + nVertexNum]);
                        }
                    }
                }
                #endregion
            }
        }

        // メッシユ情報の更新
        attachedMesh.vertices = vertices.ToArray();
        attachedMesh.uv = uvs.ToArray();
        attachedMesh.triangles = triangles.ToArray();
        attachedMesh.normals = normals.ToArray();
        debugList = vertices;

        // 頂点群に終点が入っていたら座標と頂点番号を保存する
        for (int nVertexNum = 0; nVertexNum < attachedMesh.vertices.Length; nVertexNum++)
        {
            // 終点 == メッシュの頂点の座標
            if (End.Equals(attachedMesh.vertices[nVertexNum]))
            {
                cuttingPath.Add(attachedMesh.vertices[nVertexNum]);
                cuttingVertexNumbers.Add(attachedMesh.triangles[nVertexNum]);
            }
        }

        // オブジェクトを分断しているか
        bool bCut = false;
        // 始点と終点を計算する
        // 終点が外周の辺だった場合、オブジェクトを二つに分ける
        bCut = SetStartOrEndPoint(crossVertices, first);

        // ここまで来て破る処理が行われていない場合、カットされた可能性がある
        if(!bDivide)
        {
            //
        }

        // カット
        if(bCut)
        {
            Cut(ref cuttingPath);
            bDivide = true;

            MousePoints.Clear();
        }

        return bDivide;
    }

    // オブジェクトを二つに分ける
    void Cut(ref List<Vector3> CuttingPath)
    {

        int startNum = 0;
        int endNum = 0;
        int WorkNum = 0;
        List<Vector3> objOutline1 = new List<Vector3>();
        List<Vector3> objOutline2 = new List<Vector3>();
        var Outlines = GetComponent<OutLinePath>().OutLineVertices;

        //===== １つ目 =====
        var uvs1 = new List<Vector2>();
        var normals1 = new List<Vector3>();
        
        // 交点から新しく外周を計算する
        for (int i = 0; i < Outlines.Count; i++)
        {
            // 始点の交点を決める
            if (CuttingPath[0].Equals(Outlines[i]))
            {
                startNum = i;
            }

            // 始点の交点を決める
            if (CuttingPath[CuttingPath.Count - 1].Equals(Outlines[i]))
            {
                endNum = i;
            }
        }

        //*** １つ目のアウトラインを作る
        WorkNum = startNum;
        for (; ; )
        {
            // もう１つの交点になった場合終了
            if (WorkNum == endNum) break;

            // 外周リストにプッシュ
            objOutline1.Add(Outlines[WorkNum]);

            // 次の頂点
            WorkNum++;

            // 外周のリストのサイズより大きくなった場合、先頭に戻る
            if (WorkNum >= Outlines.Count) WorkNum = 0;
        }
        for (int i = CuttingPath.Count - 1; i > 0; i--)
        {
            objOutline1.Add(CuttingPath[i]);
        }

        // エラー対応
        if (objOutline1.Count < 3)
        {
            Debug.Log("エラー objOutline1.Count");
            Debug.Log(objOutline1.Count);
            return;
        }

        //*** startとendを入れ替えて２つ目のアウトラインを作る
        WorkNum = startNum;
        startNum = endNum;
        endNum = WorkNum;
        WorkNum = startNum;
        for (; ; )
        {
            // もう１つの交点になった場合終了
            if (WorkNum == endNum) break;

            // 外周リストにプッシュ
            objOutline2.Add(Outlines[WorkNum]);

            // 次の頂点
            WorkNum++;

            // 外周のリストのサイズより大きくなった場合、先頭に戻る
            if (WorkNum >= Outlines.Count) WorkNum = 0;
        }
        for (int i = 0; i < CuttingPath.Count - 1; i++)
        {
            objOutline2.Add(CuttingPath[i]);
        }

        // エラー対応
        if (objOutline2.Count < 3)
        {
            Debug.Log("エラー objOutline2.Count");
            return;
        }
       
        // ---１つ目のuv、法線リストを構築
        for (int i = 0; i < objOutline1.Count; i++)
        {
            for (int nVertexNum = 0; nVertexNum < attachedMesh.vertices.Length; nVertexNum++)
            {
                // アウトラインの頂点座標と頂点群が等しい場合その頂点のuvと法線の値を使う
                if (objOutline1[i].Equals(attachedMesh.vertices[nVertexNum]))
                {
                    normals1.Add(attachedMesh.normals[nVertexNum]);
                    uvs1.Add(attachedMesh.uv[nVertexNum]);
                    break;
                }
            }
        }

        #region １つ目のカットされたオブジェクトを作成
        GameObject obj = GetComponent<DrawMesh>().CreateMesh(objOutline1);
        // ---Components
        obj.AddComponent<DrawMesh>();
        obj.AddComponent<DivideTriangle>();
        var collider1   = obj.AddComponent<MeshCollider>();
        var outline1    = obj.AddComponent<OutLinePath>();
        var meshFilter1 = obj.GetComponent<MeshFilter>();
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
        obj.tag = "paper";
        // 現在のマテリアルを受け継ぐ
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        // レイヤー番号も引き継ぐ
        obj.GetComponent<DivideTriangle>().number = this.number;
        // z座標を引き継ぐ
        obj.transform.position += new Vector3(0, 0, transform.position.z);
        #endregion

        //===== ２つ目 =====
        var uvs2 = new List<Vector2>();
        var normals2 = new List<Vector3>();
        // ---１つ目のuv、法線リストを構築
        for (int i = 0; i < objOutline2.Count; i++)
        {
            for (int nVertexNum = 0; nVertexNum < attachedMesh.vertices.Length; nVertexNum++)
            {
                // アウトラインの頂点座標と頂点群が等しい場合その頂点のuvと法線の値を使う
                if (objOutline2[i].Equals(attachedMesh.vertices[nVertexNum]))
                {
                    normals2.Add(attachedMesh.normals[nVertexNum]);
                    uvs2.Add(attachedMesh.uv[nVertexNum]);
                    break;
                }
            }
        }

        #region ２つ目のカットされたオブジェクトを作成
        // オブジェクト生成
        GameObject obj2 = GetComponent<DrawMesh>().CreateMesh(objOutline2);
        // ---Components
        obj2.AddComponent<DrawMesh>();
        obj2.AddComponent<DivideTriangle>();
        var collider2 = obj2.AddComponent<MeshCollider>();
        var outline2 = obj2.AddComponent<OutLinePath>();
        var meshFilter2 = obj2.GetComponent<MeshFilter>();
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

        // 現在のオブジェクトを消す
        Destroy(gameObject);

        // 切断パスをクリア
        CuttingPath.Clear();


        //--- 原点から中心へのベクトルで飛ばす
        Vector3 pos1 = obj.GetComponent<Renderer>().bounds.center;
        Vector3 pos2 = obj2.GetComponent<Renderer>().bounds.center;

        //--- 面積が小さい方を飛ばす
        Bounds bounds1 = obj.GetComponent<MeshFilter>().mesh.bounds;
        Bounds bounds2 = obj2.GetComponent<MeshFilter>().mesh.bounds;
        var width1 = bounds1.max.x - bounds1.min.x;
        var height1 = bounds1.max.y - bounds1.min.y;
        var width2 = bounds2.max.x - bounds2.min.x;
        var height2 = bounds2.max.y - bounds2.min.y;

        if (width1 * height1 < width2 * height2)
        {
            // obj1の方が遠い位置にある
            var move = obj.AddComponent<PaperMove>();
            move.SetDirection(pos1 - Vector3.zero);
            obj.tag = "waste";
            Destroy(obj, 3.0f);

            CollisionField.Instance.UpdateStage(checkCollisionPoints(obj, CollisionField.Instance.cellPoints()));
        }
        else
        {
            // obj2の方が遠い位置にある
            var move = obj2.AddComponent<PaperMove>();
            move.SetDirection(pos2 - Vector3.zero);
            obj2.tag = "waste";
            Destroy(obj2, 3.0f);

            
            CollisionField.Instance.UpdateStage(checkCollisionPoints(obj2, CollisionField.Instance.cellPoints()));
        }

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


    // 始点と終点を計算する
    // 戻り値 : 切断パスの終点が設定されたらtrue
    bool SetStartOrEndPoint(List<Vector3> crossVertices, bool first)
    {
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
        var outlineVertices = GetComponent<OutLinePath>().OutLineVertices;

        for (int i = 0; i < outlineVertices.Count; i++)
        {
            // 外周の辺
            Vector3 outlineEdge = outlineVertices[(i + 1) % (outlineVertices.Count)] - outlineVertices[i];

            for (int j = 0; j < crossVertices.Count; j++)
            {
                // 外周の頂点から交点への線分
                Vector3 vec = crossVertices[j] - outlineVertices[i];


                // 2つのベクトルの外積が0だったら、外周上の点である
                float judge = Vector3.Cross(outlineEdge, vec).sqrMagnitude;
                Debug.LogWarning("  " + outlineEdge + "    " + vec + "    " + judge);
                if (judge == 0)
                {
                    findNum++;

                    // 初回呼び出しの場合、その交点は切断パスの始点とみなす
                    if (first)
                    {
                        // 初回呼び出しで外周上の頂点が2つある場合、切断パスの終点も設定する
                        if (findNum == 1)
                        {
                            Debug.Log("始点が見つかりました");
                            // 始点追加
                            cuttingPath.Insert(0, crossVertices[j]);

                            // アウトラインの番号を保存
                            nStart = i + 1;
                        }
                        else if(findNum == 2)
                        {
                            Debug.Log("終点がが見つかりました");
                            Debug.Log("１度で切りました");

                            // 終点追加
                            cuttingPath.Add(crossVertices[j]);

                            // アウトラインの番号を保存
                            nEnd = i + 2;

                            cut = true;
                        }
                        else
                        {
                            Debug.LogError("3個目の外周上の点が見つかりました");
                        }

                        cuttingVertexNumbers.Add(0);
                    }
                    // 初回呼び出しではない場合、その交点は切断パスの終点
                    else
                    {
                        Debug.Log("終点がが見つかりました");
                        // 終点追加
                        cuttingPath.Add(crossVertices[j]);

                        // アウトラインの番号を保存
                        nEnd = i + 1;

                        cut = true;
                    }
                }
            }
        }

        // ここでアウトラインも追加したい(順番通りに)
        

        if (nStart != -1)
        {
            // 切断パスが追加されている
            GetComponent<OutLinePath>().InsertPoint(cuttingPath[0], nStart);
        }

        if (nEnd != -1)
        {
            GetComponent<OutLinePath>().InsertPoint(cuttingPath[cuttingPath.Count - 1], nEnd);
        }

        return cut;
    }


    // 番号
    public void SetNumber(int num)
    {
        number = num;
    }

    public int GetNumber()
    {
        return number;
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