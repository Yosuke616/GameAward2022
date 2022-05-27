using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeaf : Looks
{
    // 画面下にいったら消すライン
    float deletePosY = -22.0f;

    private bool bMove;

    private float x;

    void Start()
    {
        // 生成する空間
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // 0.5秒ごとに生成
        createFrame = 30;

        bMove = false;

        x = 0.0f;
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

            bMove = true;
        }
    }

    // 動きの更新
    public override void Updatelooks()
    {
        //foreach (var looksObject in looksObjects)
        //{
        //    // とりあえず落下させる
        //    looksObject.transform.position += new Vector3(0.0f, -0.2f, 0.0f); // Random.Range(0.5f,-1.5f)
        //
		//	// 画面下にいったら消す
        //    //if (looksObject.transform.position.y < deletePosY)
        //    //{
        //    //    looksObjects.Remove(looksObject);
        //    //    Destroy(looksObject);
        //    //}
        //}

        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            if(bMove)
            {
                // とりあえず落下させる
                //x = Random.Range(-10.0f, 10.0f);
                x = (Random.Range(-10.0f, 10.0f) - 5.0f) * 0.003f;
                bMove = false;
            }
            looksObjects[i].transform.position += new Vector3(x, -0.15f, 0.0f); // Random.Range(0.5f,-1.5f)


            // 画面下にいったら消す
            if (looksObjects[i].transform.position.y < deletePosY)
            {
                Destroy(looksObjects[i]);
                looksObjects.RemoveAt(i);
                looksObjects[i].gameObject.SetActive(false);
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
