using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRabbit : MonoBehaviour
{
    // 180“x‰ñ“]

    const float MAX_ROT_VALUE = 180.0f;
    const float ROT_VALUE = 3.0f;

    private float currentRot;

    void Start()
    {
        currentRot = 0.0f;
    }

    void Update()
    {
        transform.Rotate(0, ROT_VALUE, 0);

        currentRot += ROT_VALUE;

        if (currentRot >= 180)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {

                if(transform.GetChild(i).name == "Usagi_Anim_Run")
                {
                    transform.GetChild(i).gameObject.GetComponent<Animator>().enabled = false;
                }
            }

            this.enabled = false;
        }
    }
}
