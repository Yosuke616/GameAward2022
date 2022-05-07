using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translate_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 moveValue = Vector3.zero;

            moveValue.x = 1.0f;

            transform.Translate(moveValue);
        }
    }
}
