﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーの移動クラス
public class PlayerMove2 : MonoBehaviour
{
    Rigidbody rb;
    //bool bGround;
    public float speed = 1.0f;

    public Text tex;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //bGround = false;
    }

    // 更新
    void Update()
    {
        // プレイヤーの向き
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Rotate(0, -0.3f, 0);
            transform.position += -transform.right * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed;
            //transform.Rotate(0, 0.3f, 0);
        }

       
    }

    // 衝突処理
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Ground")
        //{
        //    bGround = true;
        //}
        if (collision.gameObject.gameObject.tag == "goal")
        {
            tex.text = "ゴール！";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.gameObject.tag == "goal")
        {
            tex.text = "ゴール！";
        }
    }

    //public void CreateSphere()
    //{
    //   var Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);    // メッシュ関連とコライダー
    //
    //   Sphere.transform.Translate(transform.position);
    //
    //}
    //
    //
    //
    //void CreateMesh()
    //{
    //    var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
    //    var vertices1 = new List<Vector3>();   // 新しく生成するオブジェクトの頂点のリスト
    //    var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
    //    var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト
    //
    //    // 頂点座標
    //    vertices1.Add(new Vector3(0.0f, 0.0f, 0.0f));
    //    vertices1.Add(new Vector3(10.0f, 10.0f, 0.0f));
    //    vertices1.Add(new Vector3(10.0f, 0.0f, 0.0f));
    //    // 
    //    uvs1.Add(new Vector2(0, 0));
    //    uvs1.Add(new Vector2(0, 0));
    //    uvs1.Add(new Vector2(0, 0));
    //
    //    normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
    //    normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
    //    normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
    //
    //    triangles1.Add(0);
    //    triangles1.Add(1);
    //    triangles1.Add(2);
    //
    //    //カット後のオブジェクト生成、いろいろといれる
    //    GameObject obj = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
    //    var mesh = new Mesh();
    //    mesh.vertices = vertices1.ToArray();    // 頂点情報
    //    mesh.triangles = triangles1.ToArray();  // 頂点の数
    //    mesh.uv = uvs1.ToArray();               // uv
    //    mesh.normals = normals1.ToArray();      // 法線
    //    obj.GetComponent<MeshFilter>().mesh = mesh;                                             // メッシュフィルターにメッシュをセット
    //    obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;    // メッシュフィルターにマテリアルをセット
    //    obj.GetComponent<MeshCollider>().sharedMesh = mesh;                                     // メッシュコライダーにメッシュをセット
    //    //obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;   //?
    //    //obj.GetComponent<MeshCollider>().convex = true;                                                         //?
    //    obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;          // メッシュコライダーにマテリアルをセット
    //    //obj.transform.position = transform.position;         // 座標
    //    //obj.transform.rotation = transform.rotation;         // 回転
    //    //obj.transform.localScale = transform.localScale;     // 拡縮
    //    //obj.GetComponent<Rigidbody>().freezeRotation = true; // 回転させない
    //    //obj.GetComponent<Rigidbody>().useGravity = false;    // 重力ON
    //}
}
