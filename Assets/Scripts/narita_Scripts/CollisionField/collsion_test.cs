using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collsion_test : MonoBehaviour
{
    [SerializeField] private bool exist = true;

    //GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        exist = true;

        //obj = new GameObject();
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
    //    //    // �n�ʃ^�O
    //    //    this.tag = "Ground";
    //    //}
    //
    //    if(collision.gameObject.tag == "waste") { Debug.LogWarning("����"); }
    //
    //    if (collision.gameObject.tag != "none")
    //    {
    //        exist = false;
    //        // �n�ʃ^�O
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
            //Debug.Log(collider.gameObject.tag);

            // �^�O��ݒ�
            this.tag = collider.gameObject.tag;

            transform.Rotate(collider.gameObject.transform.localEulerAngles);

            //obj = collider.gameObject;

            // �ւ�������
            if (collider.gameObject.tag == "enemy")
            {
                if(collider.gameObject.GetComponent<Enemy>() == null)
                    collider.gameObject.AddComponent<Enemy>();
            }
        }

    }

    //public GameObject Get()
    //{
    //    return obj;
    //}

    public bool IsExisted()
    {
        return exist;
    }
}
