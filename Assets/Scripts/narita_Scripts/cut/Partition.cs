// 紙と紙の間の仕切りを生成するクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partition : MonoBehaviour
{

    static public void CreatePartition(GameObject parent, List<Vector3> outline, DrawMesh drawMesh, float z)
    {
        GameObject obj1 = drawMesh.CreateMesh(outline);
        // ---Components
        obj1.AddComponent<DrawMesh>();
        obj1.AddComponent<DivideTriangle>();
        var collider1 = obj1.AddComponent<MeshCollider>();
        var meshFilter1 = obj1.GetComponent<MeshFilter>();
        // 名前
        obj1.name = "partition";
        // タグ
        obj1.tag = "partition";
        // 親オブジェクトの設定
        obj1.transform.SetParent(parent.transform);
        // その紙の奥に仕切りを置く
        obj1.transform.position += new Vector3(0, 0, z + 0.025f);
        // マテリアルを指定する
        obj1.GetComponent<Renderer>().material = GameManager.Instance.GetPartitionMaterial();
    }
}
