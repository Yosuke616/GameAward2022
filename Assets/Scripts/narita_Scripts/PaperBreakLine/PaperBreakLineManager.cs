using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBreakLineManager : SingletonMonoBehaviour<PaperBreakLineManager>
{
    public Material[] mats = new Material[1];

    

    // 紙の破れを生成
    public GameObject CreateBreakLine(List<Vector3> cuttingPath, GameObject parent)
    {
        // 生成
        GameObject obj = new GameObject("breaking paper line");
        // コンポーネントの追加
        var lineRenderer = obj.AddComponent<LineRenderer>();
        var lineRendererOperater = obj.AddComponent<LineRendererOperator>();
        // 設定
        //lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        // 座標
        lineRendererOperater.SetPoints(cuttingPath);
        // マテリアル
        lineRenderer.materials = mats;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.2f;

        // 親のオブジェクトを設定
        obj.transform.SetParent(parent.transform);

        return obj;
    }

    // ラインレンダラーの操作をやめる
    //public void Remove(GameObject obj)
    //{
    //    // ラインレンダラーの操作する機能をOFF
    //    obj.GetComponent<LineRendererOperator>().enabled = false;
    //}

    // 破れの更新
    //public void UpdateBreakLine(List<Vector3> cttingPath)
    //{
    //
    //}
}
