using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBreakLineManager : SingletonMonoBehaviour<PaperBreakLineManager>
{
    public Material[] mats = new Material[1];

    void Start()
    {
    }

    void Update()
    {
    }


    // 紙の破れを生成
    public GameObject CreateBreakLine(Vector3 startPoint)
    {
        // 生成
        GameObject obj = new GameObject("break line");
        // コンポーネントの追加
        var lineRenderer = obj.AddComponent<LineRenderer>();
        var lineRendererOperater = obj.AddComponent<LineRendererOperator>();
        // 設定
        //lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRendererOperater.AddPoint(startPoint);
        lineRenderer.materials = mats;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.2f;

        return obj;
    }

    // ラインレンダラーの操作をやめる
    public void Remove(GameObject obj)
    {
        // ラインレンダラーの操作する機能をOFF
        obj.GetComponent<LineRendererOperator>().enabled = false;
    }
}
