using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMove : MonoBehaviour
{
    public float speed = 0.05f;
    public Vector3 direct;

    void Start()
    {
        //direct = Vector3.zero;
    }

    void Update()
    {
        transform.position += direct * speed;
    }

    // 飛ばす方向の設定
    public void SetDirection(Vector3 dir)
    {
        direct = dir.normalized;
    }
}
