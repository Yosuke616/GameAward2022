using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Title_anim : MonoBehaviour
{
    private Animator animation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Fealy_Anim_Idle");

        animation = obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //animation.SetAnim("Idle");
        animation.CrossFade("Idle",0);
        
    }
}
