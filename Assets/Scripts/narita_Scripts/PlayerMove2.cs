﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの移動クラス
public class PlayerMove2 : MonoBehaviour
{
    Rigidbody rb;
    bool bGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        bGround = false;
    }

    // 更新
    void Update()
    {
        // プレイヤーの移動
        if (Input.GetKey(KeyCode.W))
        {
            // 前進
            transform.position += transform.forward * 0.05f;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            // 後退
            transform.position -= transform.forward * 0.05f;
        }

        // プレイヤーの向き
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -0.3f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0.3f, 0);
        }

       // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && bGround)
        {
            // 上方向に力を加える
            rb.AddForce(Vector3.up * 300.0f);

            // 地面にいるフラグOFF
            bGround = false;
        }

        // 武器作成
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CreateSphere();
        }

        // メッシュを作る
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateMesh();
        }
    }

    // 衝突処理
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            bGround = true;
        }
    }

    public void CreateSphere()
    {
       var Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);    // メッシュ関連とコライダー

       Sphere.transform.Translate(transform.position);

    }


    
    void CreateMesh()
    {
        var uvs1 = new List<Vector2>();         // 新しく生成するオブジェクトのUV座標のリスト
        var vertices1 = new List<Vector3>();   // 新しく生成するオブジェクトの頂点のリスト
        var triangles1 = new List<int>();       // 新しく生成するオブジェクトの頂点数のリスト
        var normals1 = new List<Vector3>();     // 新しく生成するオブジェクトの法線情報のリスト

        // 頂点座標
        vertices1.Add(new Vector3(0.0f, 0.0f, 0.0f));
        vertices1.Add(new Vector3(10.0f, 10.0f, 0.0f));
        vertices1.Add(new Vector3(10.0f, 0.0f, 0.0f));
        // 
        uvs1.Add(new Vector2(0, 0));
        uvs1.Add(new Vector2(0, 0));
        uvs1.Add(new Vector2(0, 0));

        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals1.Add(new Vector3(0.0f, 0.0f, -1.0f));

        triangles1.Add(0);
        triangles1.Add(1);
        triangles1.Add(2);

        //カット後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();    // 頂点情報
        mesh.triangles = triangles1.ToArray();  // 頂点の数
        mesh.uv = uvs1.ToArray();               // uv
        mesh.normals = normals1.ToArray();      // 法線
        obj.GetComponent<MeshFilter>().mesh = mesh;                                             // メッシュフィルターにメッシュをセット
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;    // メッシュフィルターにマテリアルをセット
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;                                     // メッシュコライダーにメッシュをセット
        //obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;   //?
        //obj.GetComponent<MeshCollider>().convex = true;                                                         //?
        obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;          // メッシュコライダーにマテリアルをセット
        //obj.transform.position = transform.position;         // 座標
        //obj.transform.rotation = transform.rotation;         // 回転
        //obj.transform.localScale = transform.localScale;     // 拡縮
        //obj.GetComponent<Rigidbody>().freezeRotation = true; // 回転させない
        //obj.GetComponent<Rigidbody>().useGravity = false;    // 重力ON
    }
}
