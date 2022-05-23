using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Alice_Script : MonoBehaviour
{
    private Animator animation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Alice_Anim");

        animation = obj.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        animation.CrossFade("walk", 0);
    }
}
