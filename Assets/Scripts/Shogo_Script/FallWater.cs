using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWater : Looks
{
    // 画面下にいったら消すライン
    float deletePosY = -22.0f;

    void Start()
    {
        // 生成する空間
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // 生成
        createFrame = 20;

        // ランダムのシード値初期化
        Random.InitState(Time.frameCount);
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
        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            // 水滴落下
            looksObjects[i].transform.position += new Vector3(0.0f, -0.3f, 0.0f); // Random.Range(0.5f,-1.5f)

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

        foreach (var camera in cameras)
        {
            // オブジェクト生成
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // コンポーネントの追加
            water.GetComponent<Renderer>().materials = mats;
            // x軸をカメラと合わせる
            water.transform.position = camera.transform.position;
            // あたり判定を消す
            water.GetComponent<Collider>().enabled = false;

            water.transform.position += new Vector3(
                rundumPosX,  // 画面内でランダム
                CreateSpace.y * 0.5f,           // カメラから画面半分の高さを足す
                22.0f + rundumPosZ);   // z軸を0に

            // リストに追加
            AddLooksObject(water);
        }

    }
}
