using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private bool bOut;
    private LineRenderer lineRenderer;  // ラインレンダラー
    private int positionCount;          // 座標の数
    private Camera mainCamera;          // カメラ

    void Start()
    {
        bOut = false;

        lineRenderer = GetComponent<LineRenderer>();
        // ラインの座標指定を、このラインオブジェクトのローカル座標系を基準にするよう設定を変更
        // この状態でラインオブジェクトを移動・回転させると、描かれたラインもワールド空間に取り残されることなく、一緒に移動・回転
        lineRenderer.useWorldSpace = false;
        positionCount = 0;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // このラインオブジェクトを、位置はカメラ前方10m、回転はカメラと同じになるようキープさせる
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 10;
        transform.rotation = mainCamera.transform.rotation;

        // 座標保存
        if (Input.GetMouseButtonDown(0))
        {
            // 座標指定の設定をローカル座標系にしたため、与える座標にも手を加える
            Vector3 pos = Input.mousePosition;
            pos.z = 10.0f;

            // マウススクリーン座標をワールド座標に直す
            pos = mainCamera.ScreenToWorldPoint(pos);

            // さらにそれをローカル座標に直す。
            pos = transform.InverseTransformPoint(pos);

            // 得られたローカル座標をラインレンダラーに追加する
            positionCount++;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(positionCount - 1, pos);


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // 内側
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (bOut == true)
                {
                    // 得られたローカル座標をラインレンダラーに追加する
                    lineRenderer.SetPosition(0, lineRenderer.GetPosition(positionCount - 2));
                    positionCount = 2;
                    lineRenderer.positionCount = positionCount;
                    lineRenderer.SetPosition(positionCount - 1, pos);
                }
                    bOut = false;
            }
            // 外側
            else
            {
                if (bOut == true)
                {
                    // 得られたローカル座標をラインレンダラーに追加する
                    positionCount = 1;
                    lineRenderer.positionCount = positionCount;
                    lineRenderer.SetPosition(positionCount - 1, pos);
                }
                bOut = true;
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            // 線を非表示
            positionCount = 0;
            lineRenderer.positionCount = 0;
        }
    }
}
