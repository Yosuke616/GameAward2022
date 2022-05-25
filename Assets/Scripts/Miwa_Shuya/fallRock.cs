using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallRock : MonoBehaviour
{
    private Rigidbody rb;

    // 最初の位置
    private Vector3 StartPos;
    //最初の時間
    private float Timer;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初期位置情報の取得
        StartPos = transform.position;

        Timer = Random.Range(0.0f, 1.0f);
        rb.drag = Random.Range(0.0f, 1.0f);
    }

    void Update()
    {
        Timer += Time.deltaTime;

        if(Timer >= 3.0f)
        {
            transform.position = StartPos;

            Timer = Random.Range(0.0f, 1.0f);
            rb.drag = Random.Range(0.0f, 1.0f);
        }

    }

}
