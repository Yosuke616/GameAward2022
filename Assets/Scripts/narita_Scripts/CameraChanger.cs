using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public Camera MainCamera;
    public Camera SubCamera0;
    public Camera SubCamera1;
    public Camera SubCamera2;
    public Camera SubCamera3;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.enabled = true;
        SubCamera0.enabled = false;
        SubCamera1.enabled = false;
        SubCamera2.enabled = false;
        SubCamera3.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        // êÿÇËë÷Ç¶
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MainCamera.enabled = true;
            SubCamera0.enabled = false;
            SubCamera1.enabled = false;
            SubCamera2.enabled = false;
            SubCamera3.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            MainCamera.enabled = false;
            SubCamera0.enabled = true;
            SubCamera1.enabled = false;
            SubCamera2.enabled = false;
            SubCamera3.enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.F3))
        {
            MainCamera.enabled = false;
            SubCamera0.enabled = false;
            SubCamera1.enabled = true;
            SubCamera2.enabled = false;
            SubCamera3.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            MainCamera.enabled = false;
            SubCamera0.enabled = false;
            SubCamera1.enabled = false;
            SubCamera2.enabled = true;
            SubCamera3.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            MainCamera.enabled = false;
            SubCamera0.enabled = false;
            SubCamera1.enabled = false;
            SubCamera2.enabled = false;
            SubCamera3.enabled = true;
        }
    }

}
