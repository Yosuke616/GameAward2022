using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallRock : MonoBehaviour
{
    private Rigidbody rb;

    // Å‰‚ÌˆÊ’u
    private Vector3 StartPos;
    //Å‰‚ÌŠÔ
    private float Timer;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ‰ŠúˆÊ’uî•ñ‚Ìæ“¾
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
