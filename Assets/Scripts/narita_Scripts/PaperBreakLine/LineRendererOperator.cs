using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ラインレンダラーにマウス座標を入れていく
[RequireComponent(typeof(LineRenderer))]
public class LineRendererOperator : MonoBehaviour
{
    // ラインレンダラー
    [SerializeField] private LineRenderer lineRenderer;
    // 座標リストのサイズ
    [SerializeField] private int positionCount;
    // カメラとの距離
    private float posZ;
    // 設定したカメラの前に線を引いていく
    private Camera mainCamera;

    // 初期化
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        positionCount = 0;

        // メインカメラ
        mainCamera = Camera.main;

        posZ = 10.0f;
    }

    // 更新
    void Update()
    {
        // 座標保存
        if (Input.GetMouseButtonDown(0))
        {
            // このラインオブジェクトを、位置はカメラ前方10m、回転はカメラと同じになるようキープさせる
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * posZ;
            transform.rotation = mainCamera.transform.rotation;

            // 座標指定の設定をローカル座標系にしたため、与える座標にも手を加える
            Vector3 pos = Input.mousePosition;
            pos.z = posZ;
            // マウススクリーン座標をワールド座標に直す
            pos = mainCamera.ScreenToWorldPoint(pos);
            // さらにそれをローカル座標に直す。
            pos = transform.InverseTransformPoint(pos);
            // 得られたローカル座標をラインレンダラーに追加する
            positionCount++;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(positionCount - 1, pos);
        }
    }

    // カメラとの距離
    public void SetZ(float z)
    {
        posZ = z;
    }

    // このコンポーネントの機能をOFFにする
    public void Remove(GameObject obj)
    {
        obj.GetComponent<LineRendererOperator>().enabled = false;
    }

    // 座標リストに座標を追加
    public void AddPoint(Vector3 pos)
    {
        positionCount++;
        lineRenderer.positionCount = positionCount;
        lineRenderer.SetPosition(positionCount - 1, pos);
    }
}