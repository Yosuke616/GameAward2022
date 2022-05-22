using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class YouseiEffect : MonoBehaviour
{
    EffekseerEmitter emitter;

    Vector3 pos;

    private int nTime;

    private int nCreateTime = 20;

    // Start is called before the first frame update
    void Start()
    {
        //pos = GameObject.Find("Yousei1").transform.position;
        pos = transform.parent.position;
        emitter = gameObject.GetComponent<EffekseerEmitter>();
        nTime = 0;
        nCreateTime = 20;
    }

    // Update is called once per frame
    void Update()
    {
        //pos = GameObject.Find("Yousei1").transform.position;
        pos = transform.parent.position;
        transform.position = new Vector3(pos.x, pos.y + 1.0f, pos.z);
        nTime++;
        if (nTime > 20)
        {
            nTime = 0;
            emitter.Play();
        }
    }
}
