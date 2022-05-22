using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メッシュのアウトラインの頂点情報を保持するクラス

public class OutLinePath : MonoBehaviour
{
    [SerializeField] public List<Vector3> OutLineVertices = new List<Vector3>();
    [SerializeField] public List<Vector2> OutLineUVs = new List<Vector2>();

    [SerializeField]
    Vector3 CurrentPoint, StartPoint, DownPoint;

    void Start()
    {
    }

    // 一番最初しか使わない
    public void SetOutLineVertecies()
    {
        OutLineVertices.Add(new Vector3(-CreateTriangle.paperSizeX, CreateTriangle.paperSizeY, 0.0f));
        OutLineVertices.Add(new Vector3(CreateTriangle.paperSizeX, CreateTriangle.paperSizeY, 0.0f));
        OutLineVertices.Add(new Vector3(CreateTriangle.paperSizeX, -CreateTriangle.paperSizeY, 0.0f));
        OutLineVertices.Add(new Vector3(-CreateTriangle.paperSizeX, -CreateTriangle.paperSizeY, 0.0f));


        // uv
        OutLineUVs.Add(new Vector2(0, 1));
        OutLineUVs.Add(new Vector2(1, 1));
        OutLineUVs.Add(new Vector2(1, 0));
        OutLineUVs.Add(new Vector2(0, 0));

        //OutLineVertices.Clear();
        //Numbers.Clear();

        ////現在のmeshの取得
        //MeshFilter attachedMeshFilter;
        //Mesh attachedMesh;
        //attachedMeshFilter = GetComponent<MeshFilter>();
        //attachedMesh = attachedMeshFilter.mesh;

        //Vector3 p;
        //Vector3 next1, next2;
        //int CurrentNumber;                          // 現在扱っている頂点番号
        //List<int> SameVertexNumbers = new List<int>();  // 同じ頂点を持つ座標の番号リスト


        //StartPoint = attachedMesh.vertices[attachedMesh.triangles[0]];
        //#region ---いちばん左上の点
        //for (int i = 0; i < attachedMesh.triangles.Length; i++)
        //{
        //    //三角形の頂点を取得
        //    p = attachedMesh.vertices[attachedMesh.triangles[i]];
        //    if (p.y > StartPoint.y)
        //    {
        //        // 変更あり
        //        StartPoint = p;
        //    }
        //    // 高さが同じ場合、左の方を採用する
        //    else if (p.y == StartPoint.y)
        //    {
        //        if (p.x < StartPoint.x)
        //        {
        //            // 変更あり
        //            StartPoint = p;
        //        }
        //    }

        //}
        //#endregion

        //DownPoint = attachedMesh.vertices[attachedMesh.triangles[0]];
        //#region ---いちばん右下の点
        //for (int i = 0; i < attachedMesh.triangles.Length; i++)
        //{
        //    //三角形の頂点を取得
        //    p = attachedMesh.vertices[attachedMesh.triangles[i]];

        //    if (p.y <= DownPoint.y)    // 今見てる座標の方が↓
        //    {
        //        if (p.y == DownPoint.y && p.x < DownPoint.x) continue;    //同じ高さの場合、右側
        //        DownPoint = p;
        //    }
        //}
        //#endregion

        //// 現在の座標をセットしておく
        //CurrentPoint = StartPoint;
        //CurrentNumber = 0;

        //// 上から下
        //int cnt = 0;
        //while (CurrentPoint.Equals(DownPoint) == false)
        //{
        //    #region ---エラー
        //    cnt++;
        //    if (cnt > 50)
        //    {
        //        Debug.Log("エラー　アウトライン");
        //        break;
        //    }
        //    #endregion

        //    // リストを空に
        //    SameVertexNumbers.Clear();

        //    // 同じ頂点の座標を持つ頂点番号を保存
        //    for (int i = 0; i < attachedMesh.triangles.Length; i++)
        //    {
        //        //三角形の頂点を取得
        //        p = attachedMesh.vertices[attachedMesh.triangles[i]];
        //        // 同じ座標かどうか
        //        if (CurrentPoint.Equals(p))
        //        {
        //            SameVertexNumbers.Add(i);
        //        }
        //    }

        //    // 同じ座標の頂点があった場合
        //    // その三角形の次の頂点の座標で扱う頂点を決める
        //    if (SameVertexNumbers.Count >= 2)
        //    {
        //        for (int i = 0; i < SameVertexNumbers.Count - 1; i++)
        //        {
        //            // 頂点番号をもとに次に接続する頂点を格納
        //            int n1 = SameVertexNumbers[i] % 3;
        //            int n2 = SameVertexNumbers[i + 1] % 3;
        //            switch (n1)
        //            {
        //                case 0: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] + 1]]; break;  // 0→1番目に
        //                case 1: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] + 1]]; break;  // 1→2番目に
        //                case 2: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] - 2]]; break;  // 2→0番目に
        //                default: Debug.Log("swich case default"); next1 = Vector3.zero; break;
        //            }
        //            switch (n2)
        //            {
        //                case 0: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] + 1]]; break;  // 0→1番目に
        //                case 1: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] + 1]]; break;  // 1→2番目に
        //                case 2: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] - 2]]; break;  // 2→0番目に
        //                default: Debug.Log("swich case default"); next2 = Vector3.zero; break;
        //            }

        //            // ③next1とnext2のx軸が等しい時、上の方を採用
        //            if (next1.x == next2.x)
        //            {
        //                if (next1.y > next2.y)
        //                {
        //                    // 現在の候補をnext1にしておく
        //                    CurrentNumber = SameVertexNumbers[i];
        //                    // next2を候補から外す
        //                    SameVertexNumbers.RemoveAt(i + 1);
        //                    // やりなおし
        //                    i--; continue;
        //                }
        //                else
        //                {
        //                    // 現在の候補をnext2にしておく
        //                    CurrentNumber = SameVertexNumbers[i + 1];
        //                    // next1を候補から外す
        //                    SameVertexNumbers.RemoveAt(i);
        //                    // やりなおし
        //                    i--; continue;
        //                }
        //            }
        //            // ④next1の方が右側にある場合、next1を採用
        //            if (next1.x > next2.x)
        //            {
        //                // 現在の候補をnext1にしておく
        //                CurrentNumber = SameVertexNumbers[i];
        //                // next2を候補から外す
        //                SameVertexNumbers.RemoveAt(i + 1);
        //                // やりなおし
        //                i--; continue;
        //            }
        //            // ⑤next2の方が右側にある場合、next2を採用
        //            else
        //            {
        //                // 現在の候補をnext2にしておく
        //                CurrentNumber = SameVertexNumbers[i + 1];
        //                // next1を候補から外す
        //                SameVertexNumbers.RemoveAt(i);
        //                // やりなおし
        //                i--; continue;
        //            }
        //        }
        //    }
        //    // 次の頂点の候補が1つだったら
        //    else CurrentNumber = SameVertexNumbers[0];

        //    //*** 外周の頂点が確定
        //    CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber]];
        //    OutLineVertices.Add(CurrentPoint);  // 座標を保存
        //    Numbers.Add(CurrentNumber);         // 番号を保存

        //    // 次の座標も確定してる
        //    switch (CurrentNumber % 3)
        //    {
        //        case 0: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber + 1]]; break;
        //        case 1: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber + 1]]; break;
        //        case 2: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber - 2]]; break;
        //        default: Debug.Log("swich case default"); CurrentPoint = Vector3.zero; break;
        //    }
        //    switch (CurrentNumber % 3)
        //    {
        //        case 0: CurrentNumber = CurrentNumber + 1; break;
        //        case 1: CurrentNumber = CurrentNumber + 1; break;
        //        case 2: CurrentNumber = CurrentNumber - 2; break;
        //        default: break;
        //    }
        //}




        //// 下から上
        //while (CurrentPoint.Equals(StartPoint) == false)
        //{
        //    cnt++;
        //    if (cnt > 50)
        //    {
        //        Debug.Log("エラー　アウトライン");
        //        break;
        //    }
        //    SameVertexNumbers.Clear();

        //    // 同じ頂点の座標を持つ頂点番号を保存
        //    for (int i = 0; i < attachedMesh.triangles.Length; i++)
        //    {
        //        //三角形の頂点を取得
        //        p = attachedMesh.vertices[attachedMesh.triangles[i]];
        //        // 同じ座標かどうか
        //        if (CurrentPoint.Equals(p))
        //        {
        //            SameVertexNumbers.Add(i);
        //        }
        //    }

        //    // 同じ座標の頂点があった場合
        //    // その三角形の次の頂点の座標が
        //    // より右にある方の頂点を採用する
        //    if (SameVertexNumbers.Count >= 2)
        //    {
        //        // 同じ座標がある
        //        for (int i = 0; i < SameVertexNumbers.Count - 1; i++)
        //        {
        //            // リストに入っている三角形のつなぎ方チェック
        //            int n1 = SameVertexNumbers[i] % 3;
        //            int n2 = SameVertexNumbers[i + 1] % 3;
        //            // 頂点番号をもとに次の頂点を決める
        //            switch (n1)
        //            {
        //                case 0: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] + 1]]; break;  // i + 1
        //                case 1: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] + 1]]; break;  // i + 1
        //                case 2: next1 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i] - 2]]; break;  // i - 2
        //                default: Debug.Log("swich case default"); next1 = Vector3.zero; break;
        //            }
        //            switch (n2)
        //            {
        //                case 0: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] + 1]]; break;  // i + 1
        //                case 1: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] + 1]]; break;  // i + 1
        //                case 2: next2 = attachedMesh.vertices[attachedMesh.triangles[SameVertexNumbers[i + 1] - 2]]; break;  // i - 2
        //                default: Debug.Log("swich case default"); next2 = Vector3.zero; break;
        //            }


        //            //// next1が現在の頂点より下にある場合
        //            //if (next1.y < CurrentPoint.y)
        //            //{
        //            //    // 現在の候補をnext2にしておく
        //            //    CurrentNumber = SameVertexNumbers[i + 1];
        //            //    // next1を候補から外す
        //            //    SameVertexNumbers.RemoveAt(i);
        //            //    // やりなおし
        //            //    i--;  continue;
        //            //}
        //            //// next2が現在の頂点より下にある場合
        //            //if (next2.y < CurrentPoint.y)
        //            //{
        //            //    // 現在の候補をnext1にしておく
        //            //    CurrentNumber = SameVertexNumbers[i];
        //            //    // next2を候補から外す
        //            //    SameVertexNumbers.RemoveAt(i + 1);
        //            //    // やりなおし
        //            //    i--; continue;
        //            //}

        //            // x軸の大きさが等しい場合、下にあるものを採用
        //            if (next1.x == next2.x)
        //            {
        //                if (next1.y < next2.y)
        //                {
        //                    // 現在の候補をnext1にしておく
        //                    CurrentNumber = SameVertexNumbers[i];
        //                    // next2を候補から外す
        //                    SameVertexNumbers.RemoveAt(i + 1);
        //                    // やりなおし
        //                    i--; continue;
        //                }
        //                else
        //                {
        //                    // 現在の候補をnext2にしておく
        //                    CurrentNumber = SameVertexNumbers[i + 1];
        //                    // next1を候補から外す
        //                    SameVertexNumbers.RemoveAt(i);
        //                    // やりなおし
        //                    i--; continue;
        //                }
        //            }

        //            // より左にある頂点を採用
        //            if (next1.x < next2.x)
        //            {
        //                // 現在の候補をnext1にしておく
        //                CurrentNumber = SameVertexNumbers[i];
        //                // next2を候補から外す
        //                SameVertexNumbers.RemoveAt(i + 1);
        //                // やりなおし
        //                i--; continue;
        //            }
        //            else
        //            {
        //                // 現在の候補をnext2にしておく
        //                CurrentNumber = SameVertexNumbers[i + 1];
        //                // next1を候補から外す
        //                SameVertexNumbers.RemoveAt(i);
        //                // やりなおし
        //                i--; continue;
        //            }
        //        }
        //    }
        //    // 次の頂点の候補が1つだったら
        //    else CurrentNumber = SameVertexNumbers[0];

        //    //*** 外周の頂点が確定
        //    CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber]];
        //    OutLineVertices.Add(CurrentPoint);  // 座標を保存
        //    Numbers.Add(CurrentNumber);         // 番号を保存

        //    // 次の座標も確定してる
        //    switch (CurrentNumber % 3)
        //    {
        //        case 0: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber + 1]]; break;
        //        case 1: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber + 1]]; break;
        //        case 2: CurrentPoint = attachedMesh.vertices[attachedMesh.triangles[CurrentNumber - 2]]; break;
        //        default:CurrentPoint = Vector3.zero; break;
        //    }
        //    switch (CurrentNumber % 3)
        //    {
        //        case 0: CurrentNumber = CurrentNumber + 1; break;
        //        case 1: CurrentNumber = CurrentNumber + 1; break;
        //        case 2: CurrentNumber = CurrentNumber - 2; break;
        //        default: break;
        //    }
        //}
    }


    // 
    public void UpdateOutLine(List<Vector3> outline)
    {
        // 外周上の頂点を更新
        OutLineVertices = outline;

        OutLineUVs.Clear();

        // 外周上の頂点のuv座標を更新する
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        for (int outlineIndex = 0; outlineIndex < OutLineVertices.Count; outlineIndex++)
        {
            for (int uvIndex = 0; uvIndex < mesh.vertices.Length; uvIndex++)
            {
                // 座標が等しかったら
                if (mesh.vertices[uvIndex].Equals(OutLineVertices[outlineIndex]))
                {
                    // uvゲット
                    OutLineUVs.Add(mesh.uv[uvIndex]);
                    continue;
                }
            }
        }

        //if(OutLineVertices.Count == OutLineUVs.Count)
        //{
        //    Debug.LogWarning("よい");
        //}
        //else
        //{
        //    Debug.LogWarning("だめ");
        //}
    }

    public void InsertPoint(int index, Vector3 cross, Vector2 uv)
    {
        OutLineVertices.Insert(index, cross);
        OutLineUVs.Insert(index, uv);
    }

    public List<Vector2> Getuvs()
    {
        return OutLineUVs;
    }
}
