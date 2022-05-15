using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	public Camera openingCamera;

	private GameObject TutorialPanel;
	private GameObject BGobject1;
	private GameObject BGobject2;

	private bool bInput1;

	// Start is called before the first frame update
	void Start()
	{
		TutorialPanel = GameObject.Find("TutorialPanel");
		BGobject1 = TutorialPanel.transform.Find("BackGround1").gameObject;
		BGobject2 = TutorialPanel.transform.Find("BackGround2").gameObject;

		BGobject1.SetActive(false);
		BGobject2.SetActive(false);
		bInput1 = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (openingCamera.enabled == false && !bInput1)
			BGobject1.SetActive(true);


		if (Input.GetMouseButton(0))
		{
			BGobject1.SetActive(false);
			bInput1 = true;
		}

	}
}
