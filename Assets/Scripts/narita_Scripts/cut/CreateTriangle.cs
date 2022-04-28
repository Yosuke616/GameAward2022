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

    void Start()
    {
        // 紙の生成
        GameObject paper = CreateMesh();

        // 仕切りの生成
        CreatePartition(paper);
    }

    // 紙の生成
    GameObject CreateMesh()
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();    // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 頂点座標
        vertices1.Add(new Vector3(-paperSizeX,  paperSizeY, 0.0f));  // 左上
        vertices1.Add(new Vector3( paperSizeX,  paperSizeY, 0.0f));  // 右上
        vertices1.Add(new Vector3( paperSizeX, -paperSizeY, 0.0f));  // 右下
        vertices1.Add(new Vector3(-paperSizeX, -paperSizeY, 0.0f));  // 左下
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

        // 頂点座標
        vertices1.Add(new Vector3(-paperSizeX, paperSizeY, 0.0f));  // 左上
        vertices1.Add(new Vector3(paperSizeX, paperSizeY, 0.0f));  // 右上
        vertices1.Add(new Vector3(paperSizeX, -paperSizeY, 0.0f));  // 右下
        vertices1.Add(new Vector3(-paperSizeX, -paperSizeY, 0.0f));  // 左下
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
