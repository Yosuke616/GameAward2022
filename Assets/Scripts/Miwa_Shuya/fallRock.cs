using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallRock : MonoBehaviour
{
    private Rigidbody rb;

    // �ŏ��̈ʒu
    private Vector3 StartPos;
    //�ŏ��̎���
    private float Timer;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // �����ʒu���̎擾
        StartPos = transform.position;

        Timer = Random.Range(0.0f, 1.0f);
        //rb.drag = Random.Range(0.0f, 1.0f);
    }

    void Update()
    {
        Timer += Time.deltaTime;

        if(Timer >= 3.0f)
        {
            transform.position = StartPos;

            Timer = Random.Range(0.0f, 1.0f);
            //rb.drag = Random.Range(0.0f, 1.0f);
        }

    }

}
