using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> MousePoints;

    // スクリーン座標
    Vector3 screenPoint;

    void Start()
    {
        MousePoints = new List<Vector3>();
    }

    void Update()
    {
        // このオブジェクトのワールド座標をスクリーン座標に変換した値を代入
        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        // マウス座標のzの値を0にする
        Vector3 cursor = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        // マウス座標をワールド座標に変換する
        transform.position = Camera.main.ScreenToWorldPoint(cursor);

        // 座標保存
        if (Input.GetMouseButtonDown(0))
        {
            // 座標リストに追加
            MousePoints.Add(transform.position);


            if (MousePoints.Count >= 2)
            {
                // 紙のリストを作る
                List<GameObject> objects = new List<GameObject>();
                objects.AddRange(GameObject.FindGameObjectsWithTag("paper"));
                if (objects != null)
                {
                    if(objects.Count >= 2)
                    {
                        objects.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());
                    }

                    foreach(GameObject obj in objects)
                    {
                        Debug.Log("", obj);
                    }

                    // 一番手前の紙しか切りたくない
                    bool bDivide = false;

                    for (int i = 0; i < objects.Count; i++)
                    {
                        var divideTriangle = objects[i].GetComponent<DivideTriangle>();
                        if (divideTriangle)
                        {
                            bDivide = divideTriangle.Divide(ref MousePoints);

                            if (bDivide)
                            {
                                break;
                            }
                        }
                    }
                }
                // 頂点分割
                
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            MousePoints.Clear();
        }
    }
}
