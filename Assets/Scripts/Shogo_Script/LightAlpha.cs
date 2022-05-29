using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAlpha : Looks
{
    // 画面下にいったら消すライン
    //float deletePosY = -22.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 生成する空間
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // 生成
        //createFrame = 20;

        // ランダムのシード値初期化
        Random.InitState(Time.frameCount);

        for (int i = 0; i < 20; ++i)
            CreateKira();

        // ランダムのシード値初期化
        //Random.InitState(Time.frameCount);
    }

    public override void Createlooks()
    {
        frameCnt++;

        //if (frameCnt > createFrame)
        //{
        //    // オブジェクト生成
        //    CreateKira();
        //
        //    // カウントリセット
        //    frameCnt = 0;
        //}
    }

    // 動きの更新
    public override void Updatelooks()
    {
        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            looksObjects[i].GetComponent<DeleteTime>().UpdateKira();
        }
    }

    private void CreateKira()
    {
        // サブカメラを探し、そのカメラ内にオブジェクトを生成する
        List<GameObject> cameras = new List<GameObject>();
        cameras.AddRange(GameObject.FindGameObjectsWithTag("SubCamera"));

        // for文で回す前にRandumを使用
        float rundumPosX = Random.Range(-CreateSpace.x * 0.5f, CreateSpace.x * 0.5f);
        float rundumPosY = Random.Range(-CreateSpace.y * 0.5f, CreateSpace.y * 0.5f);
        float rundumPosZ = Random.Range(-6.0f, 6.0f);
        int nTime = Random.Range(30, 90);
        int rundum = Random.Range(0, 1);
        float add = Random.Range(0.0f, 0.08f);

        foreach (var camera in cameras)
        {
            // オブジェクト生成
            GameObject kira = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // コンポーネントの追加
            kira.GetComponent<Renderer>().materials = mats;
            // x軸をカメラと合わせる
            kira.transform.position = camera.transform.position;
            // あたり判定を消す
            kira.GetComponent<Collider>().enabled = false;

            // 時間設定
            kira.AddComponent<DeleteTime>().SetTime(nTime);
            // α値設定
            if (rundum == 0)
            {
                kira.GetComponent<DeleteTime>().SetAlpha(1.0f, false);
            }
            else
            {
                kira.GetComponent<DeleteTime>().SetAlpha(0.0f, true);
            }

            // 加算設定
            kira.GetComponent<DeleteTime>().SetAdd(add);

            // 座標設定
            kira.transform.position += new Vector3(
                rundumPosX,             // 画面内でランダム
                rundumPosY,             // カメラから画面半分の高さを足す
                22.0f + rundumPosZ);    // z軸を0に

            // 大きさ設定
            kira.transform.localScale = new Vector3(
                3.0f,
                3.0f,
                3.0f
                );

            // リストに追加
            AddLooksObject(kira);
        }

    }
}
