using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{

    // 動きをまねる対象のオブジェクト
    public GameObject HomingTarget;

    private Vector3 OldPos;
    private Vector3 MoveValue;

    void Start()
    {
        OldPos = HomingTarget.transform.position;
    }

    void Update()
    {
        // 対象のオブジェクトの移動量
        MoveValue = HomingTarget.transform.position - OldPos;

        // このオブジェクトにその移動量を加算
        transform.position += MoveValue;

        // 過去座標を更新
        OldPos = HomingTarget.transform.position;
    }
}
