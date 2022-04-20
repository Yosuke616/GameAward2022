using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public Camera MainCamera;
	public List<Camera> SubCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.enabled = true;
		GameObject[] objects = GameObject.FindGameObjectsWithTag("SubCamera");
		for (int i = 0; i < objects.Length; i++)
		{
			SubCamera[i] = objects[i].GetComponent<Camera>();
			SubCamera[i].enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        // êÿÇËë÷Ç¶
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MainCamera.enabled = true;
			for(int i = 0; i < SubCamera.Count; i++)
			{
				SubCamera[i].enabled = false;
			}
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            MainCamera.enabled = false;
			for (int i = 0; i < SubCamera.Count; i++)
			{
				SubCamera[i].enabled = false;
			}
			SubCamera[0].enabled = true;
		}
        else if(Input.GetKeyDown(KeyCode.F3))
        {
			MainCamera.enabled = false;
			for (int i = 0; i < SubCamera.Count; i++)
			{
				SubCamera[i].enabled = false;
			}
			SubCamera[1].enabled = true;
		}
		else if (Input.GetKeyDown(KeyCode.F4))
        {
			MainCamera.enabled = false;
			for (int i = 0; i < SubCamera.Count; i++)
			{
				SubCamera[i].enabled = false;
			}
			SubCamera[2].enabled = true;
		}
		else if (Input.GetKeyDown(KeyCode.F5))
        {
			MainCamera.enabled = false;
			for (int i = 0; i < SubCamera.Count; i++)
			{
				SubCamera[i].enabled = false;
			}
			SubCamera[3].enabled = true;
		}
	}

}
