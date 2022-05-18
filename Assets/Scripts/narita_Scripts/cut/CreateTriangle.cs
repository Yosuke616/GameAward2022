using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    public float z = 0.0f;
    public int number = 1;
    public bool partition = true;

    static public float paperSizeX = 9.0f;
    static public float paperSizeY = 5.0f;
    //static public float paperSizeX = 10.25f;
    //static public float paperSizeY = 5.75f;

    // Quadの枚数
    static private int QuadNum = 2;

    void Awake()
    {
        // 紙の生成
        GameObject paper = CreateMesh();

        // 仕切りの生成
        //if (partition) {
        //    CreatePartition(paper);
        //}
    }

    // 紙の生成
    GameObject CreateMesh()
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();    // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 2022/05/01 ShimizuShogo------------------------------------------------------------------------------
        int i = 0;
        int length = (QuadNum + 1) * (QuadNum + 1);
        int x, y = 0;

        // 頂点座標、uv座標、法線の設定
        for (i = 0; i < length; ++i)
        {
            x = i % (QuadNum + 1);
            y = i / (QuadNum + 1);
            // 頂点座標
            vertices1.Add(new Vector3(paperSizeX * ((float)(-QuadNum / 2 + x) / (float)(QuadNum / 2)),
                                        paperSizeY * ((float)(QuadNum / 2 - y) / (float)(QuadNum / 2)),
                                        0.0f));

            // uv座標
            uvs1.Add(new Vector2(((float)x / (float)QuadNum), ((float)(QuadNum - y) / (float)QuadNum)));

            // 法線
            normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        }
        // 頂点インデックス
        for (i = 0; i < length - (QuadNum + 1); ++i)
        {
            if ((i % (QuadNum + 1)) == QuadNum)
                continue;

            triangles1.Add(i);
            triangles1.Add(i + 1);
            triangles1.Add(i + QuadNum + 1);

            triangles1.Add(i + 1);
            triangles1.Add(i + QuadNum + 2);
            triangles1.Add(i + QuadNum + 1);
        }
        // ----------------------------------------------------------------------------------------------------------

        // 頂点座標
        //vertices1.Add(new Vector3(-paperSizeX,  paperSizeY, 0.0f));  // 左上
        //vertices1.Add(new Vector3( paperSizeX,  paperSizeY, 0.0f));  // 右上
        //vertices1.Add(new Vector3( paperSizeX, -paperSizeY, 0.0f));  // 右下
        //vertices1.Add(new Vector3(-paperSizeX, -paperSizeY, 0.0f));  // 左下
        //// uv
        //uvs1.Add(new Vector2(0, 1));
        //uvs1.Add(new Vector2(1, 1));
        //uvs1.Add(new Vector2(1, 0));
        //uvs1.Add(new Vector2(0, 0));
        //// 法線
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //// 頂点インデックス
        //triangles1.Add(0);
        //triangles1.Add(1);
        //triangles1.Add(3);
        //triangles1.Add(1);
        //triangles1.Add(2);
        //triangles1.Add(3);


        //カット後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("paper",
            typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider),
            typeof(DivideTriangle), typeof(OutLinePath), typeof(DrawMesh),
            typeof(Turn_Shader));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();    // 頂点情報
        mesh.triangles = triangles1.ToArray();  // 頂点の数
        mesh.uv = uvs1.ToArray();               // uv
        mesh.normals = normals1.ToArray();      // 法線
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshFilter>().mesh = mesh;                                             // メッシュフィルターにメッシュをセット
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;                                     // メッシュコライダーにメッシュをセット                                                    //?
        obj.GetComponent<OutLinePath>().SetOutLineVertecies();
        obj.tag = "paper";
        obj.transform.position += new Vector3(0, 0, z);

        obj.GetComponent<DivideTriangle>().SetNumber(number);

        return obj;
    }

    // 仕切り生成
    void CreatePartition(GameObject parent)
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();    // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 2022/05/01 ShimizuShogo------------------------------------------------------------------------------
        int i = 0;
        int length = (QuadNum + 1) * (QuadNum + 1);
        int x, y = 0;
        
        // 頂点座標、uv座標、法線の設定
        for (i = 0; i < length; ++i)
        {
            x = i % (QuadNum + 1);
            y = i / (QuadNum + 1);
            // 頂点座標
            vertices1.Add(new Vector3(paperSizeX * ((float)(-QuadNum / 2 + x) / (float)(QuadNum / 2)),
                                        paperSizeY * ((float)(QuadNum / 2 - y) / (float)(QuadNum / 2)),
                                        0.0f));

            // uv座標
            uvs1.Add(new Vector2(((float)x / (float)QuadNum), ((float)(QuadNum - y) / (float)QuadNum)));

            // 法線
            normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        }
        // 頂点インデックス
        for (i = 0; i < length - (QuadNum + 1); ++i)
        {
            if ((i % (QuadNum + 1)) == QuadNum)
                continue;

            triangles1.Add(i);
            triangles1.Add(i + 1);
            triangles1.Add(i + QuadNum + 1);

            triangles1.Add(i + 1);
            triangles1.Add(i + QuadNum + 2);
            triangles1.Add(i + QuadNum + 1);
        }
        // ----------------------------------------------------------------------------------------------------------

        // 頂点座標
        //vertices1.Add(new Vector3(-paperSizeX, paperSizeY, 0.0f));  // 左上
        //vertices1.Add(new Vector3(paperSizeX, paperSizeY, 0.0f));  // 右上
        //vertices1.Add(new Vector3(paperSizeX, -paperSizeY, 0.0f));  // 右下
        //vertices1.Add(new Vector3(-paperSizeX, -paperSizeY, 0.0f));  // 左下
        //// uv
        //uvs1.Add(new Vector2(0, 1));
        //uvs1.Add(new Vector2(1, 1));
        //uvs1.Add(new Vector2(1, 0));
        //uvs1.Add(new Vector2(0, 0));
        //// 法線
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        //// 頂点インデックス
        //triangles1.Add(0);
        //triangles1.Add(1);
        //triangles1.Add(3);
        //triangles1.Add(1);
        //triangles1.Add(2);
        //triangles1.Add(3);

        //カット後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("partition",
            typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();    // 頂点情報
        mesh.triangles = triangles1.ToArray();  // 頂点の数
        mesh.uv = uvs1.ToArray();               // uv
        mesh.normals = normals1.ToArray();      // 法線
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshFilter>().mesh = mesh;                                             // メッシュフィルターにメッシュをセット
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;                                     // メッシュコライダーにメッシュをセット                                                    //?
        obj.tag = "partition";
        obj.transform.position += new Vector3(0, 0, z + 0.025f);
        obj.transform.SetParent(parent.transform);
        obj.GetComponent<Renderer>().material = GameManager.Instance.GetPartitionMaterial();
    }


    
}
