using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> MousePoints;

    public int cnt = 0;

    private bool bDivide;

    int Select;

    // スクリーン座標
    Vector3 screenPoint;

    [SerializeField] private List<GameObject> papers;

    void Start()
    {
        MousePoints = new List<Vector3>();

        papers = new List<GameObject>();

        Select = 0;
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
    }

    void Update()
    {
        // debug用
        if (Input.GetKeyDown(KeyCode.X))
        {
            List<bool> a = new List<bool>();
            for (int i = 0; i < 2880; i++)
            {
                a.Add(true);
            }
            CollisionField.Instance.UpdateStage(a);
        }

        if (Camera.main == null) { return; }


        if(Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown("joystick button 4"))
        {
            UpdatePage();
            // めくる
            var a = papers[Select]; if (!a) Debug.LogError("a");
            var b = a.GetComponent<Turn_Shader>(); if(!b) Debug.LogError("b");
            b.SetPaperSta(1);
            Select++;
            if (Select > 2) Select = 2;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 5"))
        {
            UpdatePage();
            // めくるのを戻す
            var a = papers[Select]; if (!a) Debug.LogError("a");
            var b = a.GetComponent<Turn_Shader>(); if (!b) Debug.LogError("b");
            b.SetPaperSta(2);

            Select--;
            if (Select < 0) Select = 0;
        }


        // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // マウス座標のzの値を0にする
        Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        // マウス座標をワールド座標に変換する
        transform.position = Camera.main.ScreenToWorldPoint(cursor);

        //カーソルの座標を送る
        GameObject Cursor = GameObject.Find("cursor");
        GameObject obj = GameObject.Find("CTRLCur");
        GameObject camera = GameObject.Find("MainCamera");

        //座標の保存用の変数
        Vector3 SavePos = new Vector3(0.0f,0.0f,0.0f);

        //送るものの座標を変える
        if (Cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg())
        {
            SavePos = obj.transform.position;
        }
        else {

            SavePos = Cursor.transform.position;
        }

        // 座標保存
        if (Input.GetMouseButtonDown(0) || camera.GetComponent<InputTrigger>().GetOneTimeDown())
        {

            cnt = 0;          

            // 座標リストに追加
            MousePoints.Add(SavePos);


            if (MousePoints.Count >= 2)
            {
                papers.Clear();
                // 紙のリストを作る
                //List<GameObject> objects = new List<GameObject>();
                papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

                if (papers != null)
                {
                    // ソート
                    if(papers.Count >= 2)
                    {
                        // 紙の番号、昇順
                        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
                    }

                    
                    bDivide = false;

                    for (int i = 0; i < papers.Count; i++)
                    {
                        var divideTriangle = papers[i].GetComponent<DivideTriangle>();
                        if (divideTriangle)
                        {
                            // 破る
                            bDivide = divideTriangle.Divide(ref MousePoints);
                            cnt++;

                            // 一度破ったら、奥の紙は切らない
                            if (bDivide) break;
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            MousePoints.Clear();
        }

    }

    //切っているかどうかのフラグをゲットするための関数
    public bool GetBreakFlg() {
        return bDivide;
    }


    private void UpdatePage()
    {
        papers.Clear();

        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));

        // 紙の番号、昇順
        papers.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
    }
}
