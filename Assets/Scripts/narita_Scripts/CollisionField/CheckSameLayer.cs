using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSameLayer : MonoBehaviour
{
    [SerializeField] private int m_nOriginalLayer = 1;
    [SerializeField] private int m_nCurrentLayer = 0;
    MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // 現在のレイヤーを更新
        m_nCurrentLayer = CurrentLayer(transform.position);

        if(m_nOriginalLayer != m_nCurrentLayer)
        {
            Debug.Log("コライダーなし");
            // レイヤーが違う場合、false
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
        else
        {
            Debug.Log("コライダー");

            // 同一レイヤの場合
            meshRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }


   // あたり判定オブジェクトの紙の中にいるのか、破られた場所にいるのか
    public int CurrentLayer(Vector3 pos)
    {
        // カメラを見つける
        GameObject cameraObj = GameObject.Find("SubCamera0");
        pos = cameraObj.transform.InverseTransformPoint(pos);

        // マスの横・縦の数
        float gridSizeX = CreateGridScript.gridSizeX;
        float gridSizeY = CreateGridScript.gridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        // rayを飛ばす座標
        Vector2 start;
        start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        start.y =  gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);
        // グリッドの横幅と高さ
        Vector2 length;
        length.x = gridSizeX * gridNumX;
        length.y = gridSizeY * gridNumY;
        // キョリを求める
        Vector2 distance;
        distance.x = pos.x - start.x;
        distance.y = start.y - pos.y;
        // 距離から比率を求める
        Vector2 rate;
        rate.x = distance.x / length.x;
        rate.y = distance.y / length.y;
        // 比率から現在のマスの番号を求める
        int massX, massY;
        massX = (int)(gridNumX * rate.x);
        massY = (int)(gridNumY * rate.y);
        //Debug.LogWarning("x:" + massX + "   y:" + massY);
        massY++;
        if (massX < 0 || massY < 0) return -1;

        // paperで、指定された番号のグリッドにレイを飛ばしてそこの紙のレイヤー番号を取得する


        // 紙のグリッドの大きさ
        float paperGridSizeX = CreateGridScript.paperGridSizeX;
        float paperGridSizeY = CreateGridScript.paperGridSizeY;

        //ｶﾒﾗをヒエラルキーから引っ張り出してくる
        GameObject mainCamera = GameObject.Find("MainCamera");

        // 描画開始位置 = カメラ座標 - gridの横幅 * 数の半分
        Vector3 StartPoint;
        StartPoint.x = mainCamera.transform.position.x - paperGridSizeX * gridNumX * 0.5f + (paperGridSizeX * 0.5f);
        StartPoint.y = mainCamera.transform.position.y + paperGridSizeY * gridNumY * 0.5f - (paperGridSizeY * 0.5f);

        float x, y;
        x = StartPoint.x + (paperGridSizeX * massX);
        y = StartPoint.y - (paperGridSizeY * massY);
        // マスごとにあたり判定を取る用のオブジェクトを生成
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, new Vector3(x, y, 10.2f), out hit))
        {
            if(hit.collider.gameObject.tag == "paper")
            {
                //Debug.DrawRay(mainCamera.transform.position, new Vector3(x, y, 10.2f));
                //Debug.LogError("");
                //Debug.LogWarning("yes" + hit.collider.gameObject.GetComponent<DivideTriangle>().GetNumber());
                return hit.collider.gameObject.GetComponent<DivideTriangle>().GetNumber();
            }
        }

        return 0;
    }

    public void SetLayer(int layer)
    {
        m_nOriginalLayer = layer;
    }
}
