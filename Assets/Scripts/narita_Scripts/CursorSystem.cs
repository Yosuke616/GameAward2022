using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> MousePoints;

    public int cnt = 0;

    // スクリーン座標
    Vector3 screenPoint;

    void Start()
    {
        MousePoints = new List<Vector3>();
    }

    void Update()
    {
        // debug用
        if (Input.GetKeyDown(KeyCode.X))
        {
            List<bool> a = new List<bool>();
            for (int i = 0; i < 2000; i++)
            {
                a.Add(true);
            }
            CollisionField.Instance.UpdateStage(a);
        }

        if (Camera.main == null) { return; }

        // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // マウス座標のzの値を0にする
        Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        // マウス座標をワールド座標に変換する
        transform.position = Camera.main.ScreenToWorldPoint(cursor);

        // 座標保存
        if (Input.GetMouseButtonDown(0))
        {
            cnt = 0;

            // 座標リストに追加
            MousePoints.Add(transform.position);


            if (MousePoints.Count >= 2)
            {
                // 紙のリストを作る
                List<GameObject> objects = new List<GameObject>();
                objects.AddRange(GameObject.FindGameObjectsWithTag("paper"));

                if (objects != null)
                {
                    // ソート
                    if(objects.Count >= 2)
                    {
                        objects.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
                    }

                    
                    bool bDivide = false;

                    for (int i = 0; i < objects.Count; i++)
                    {
                        var divideTriangle = objects[i].GetComponent<DivideTriangle>();
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
}
