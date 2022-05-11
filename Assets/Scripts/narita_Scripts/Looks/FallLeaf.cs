using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeaf : Looks
{
    // 画面下にいったら消すライン
    float deletePosY = -22.0f;

    void Start()
    {
        // 生成する空間
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // 0.5秒ごとに生成
        createFrame = 30;
    }

    public override void Createlooks()
    {
        frameCnt++;

        if (frameCnt > createFrame)
        {
            // オブジェクト生成
            CreateLeaf();

            // カウントリセット
            frameCnt = 0;
        }
    }

    // 動きの更新
    public override void Updatelooks()
    {
        foreach (var looksObject in looksObjects)
        {
            // とりあえず落下させる
            looksObject.transform.position += new Vector3(0.0f, -0.2f, 0.0f); // Random.Range(0.5f,-1.5f)

			// 画面下にいったら消す
            if (looksObject.transform.position.y < deletePosY)
            {
                looksObjects.Remove(looksObject);
                Destroy(looksObject);
            }
        }
    }

    private void CreateLeaf()
    {
        // サブカメラを探し、そのカメラ内にオブジェクトを生成する
        List<GameObject> cameras = new List<GameObject>();
        cameras.AddRange(GameObject.FindGameObjectsWithTag("SubCamera"));

        // for文で回す前にRandumを使用
        float rundumPosX = Random.Range(-CreateSpace.x * 0.5f, CreateSpace.x * 0.5f);
        float rundumPosZ = Random.Range(-6.0f, 6.0f);

        foreach (var camera  in cameras)
        {
            // オブジェクト生成
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // コンポーネントの追加
            leaf.GetComponent<Renderer>().materials = mats;
            // x軸をカメラと合わせる
            leaf.transform.position = camera.transform.position;


            leaf.transform.position += new Vector3(
                rundumPosX,  // 画面内でランダム
                CreateSpace.y * 0.5f,           // カメラから画面半分の高さを足す
                22.0f + rundumPosZ);   // z軸を0に

            // リストに追加
            AddLooksObject(leaf);
        }

    }
}
