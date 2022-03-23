using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collsion_test : MonoBehaviour
{
    [SerializeField] private bool exist = true;

    // Start is called before the first frame update
    void Start()
    {
        exist = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //void OnCollisionStay(Collision collision)
    //{
    //    //Debug.Log(collision.gameObject.tag);
    //    //if (collision.gameObject.tag == "Ground")
    //    //{
    //    //    exist = false;
    //    //    // 地面タグ
    //    //    this.tag = "Ground";
    //    //}
    //
    //    if(collision.gameObject.tag == "waste") { Debug.LogWarning("ある"); }
    //
    //    if (collision.gameObject.tag != "none")
    //    {
    //        exist = false;
    //        // 地面タグ
    //        this.tag = collision.gameObject.tag;
    //    }
    //}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "paper")
        {
            return;
        }

        if (collider.gameObject.tag != "none")
        {
            // タグを設定
            this.tag = collider.gameObject.tag;
        }
    }

    public bool IsExisted()
    {
        return exist;
    }
}
