using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // シーン遷移用

public class StageSelect : MonoBehaviour
{
    // 現在の進捗
    public static int ProgressStage;

    // 各ステージパネル
    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject s5;
    public GameObject s6;
    public GameObject s7;
    public GameObject s8;

    // パネル移動量
    public float MovePanel;

    // 今現在の選択パネル
    int Select = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 進捗状況によって最初の状況変化（クリア演出含む）
        switch(ProgressStage)
        {
            case 0:
                break;

            // チュートリアル突破
            case 1:
                s1.transform.position = new Vector3(0, 0, 300);
                break;


        }


        // ステージにダイブ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (ProgressStage)
            {
                case 0:
                SceneManager.LoadScene("test_Stage1");
                break;

                case 1:
                SceneManager.LoadScene("1-2");
                break;

                case 2:
                SceneManager.LoadScene("1-3");
                break;

                case 3:
                SceneManager.LoadScene("1-4");
                break;
            }
        }

        //switch (Select)
        //{
        //    case 1:
        //        s1.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 2:
        //        SceneManager.LoadScene("1-2");
        //        s2.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 3:
        //        s3.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 4:
        //        s4.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 5:
        //        s5.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 6:
        //        s6.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 7:
        //        s7.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;

        //    case 8:
        //        s8.transform.position += new Vector3(MovePanel, 0, 0) * Time.deltaTime;
        //        break;
        //}
    }


    // ステージクリア時に進捗保存関数
    public static void UpdateProgress(string name)
    {
        if (name == "test_Stage1") 
        {
            ProgressStage = 1;
        }

        if (name == "1-2")
        {
            ProgressStage = 2;
        }

        if (name == "1-3")
        {
            ProgressStage = 3;
        }

        if (name == "1-4")
        {
            ProgressStage = 4;
        }
    }
}
