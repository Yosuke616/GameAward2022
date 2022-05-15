using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	public Camera openingCamera;

	private GameObject TutorialPanel;
	private GameObject cursor;
	private List<GameObject> BGobjects = new List<GameObject>();
	//private GameObject BGobject1;
	//private GameObject BGobject2;
	//private GameObject BGobject3;

	private int nCnt;
	private bool bStartTutorial;
	private bool bEndTutorial;

	// Start is called before the first frame update
	void Start()
	{
		// �ϐ���`
		TutorialPanel = GameObject.Find("TutorialPanel");
		cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
		BGobjects.Add(TutorialPanel.transform.Find("BackGround1").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround2").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround3").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround4").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround6").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround7").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround8").gameObject);

		var color = new Color(0.0f, 0.0f, 0.0f, 128.0f / 255.0f);
		for(int i = 0; i < BGobjects.Count; i++)
		{
			// �q�G�����L�[�r���[��ł́A�֋X�㓧���x��0.0f�ɐݒ肵�Ă���̂ŁA���ɖ߂�����
			BGobjects[i].GetComponent<Image>().color = color;

			// ��\��������
			BGobjects[i].SetActive(false);
		}

		nCnt = 0;
		bStartTutorial = false;
		bEndTutorial = false;
	}

	// Update is called once per frame
	void Update()
	{
		// ���񑀍�
		if (openingCamera.enabled == false &&
			bStartTutorial == false)
		{
			BGobjects[0].SetActive(true);
			cursor.SetActive(false);        // �������͐ؒf����𖳌��ɂ���
			bStartTutorial = true;
			Debug.LogWarning("Tuto:���񑀍�");
		}

		// ���̃y�[�W��
		if (Input.GetMouseButtonDown(0) && bEndTutorial == false)
		{
			Debug.LogWarning($"Tuto:false[{nCnt}], true[{nCnt + 1}]");
			BGobjects[nCnt].SetActive(false);

			if (BGobjects[nCnt + 1] != null)
			{
				BGobjects[nCnt + 1].SetActive(true);
				nCnt++;
			}
			else	// �������ł͂Ȃ����A�ؒf�����L���ɂ���
			{
				nCnt++;
				Debug.LogWarning($"Tuto:end[{bEndTutorial}]");
				bEndTutorial = true;
				cursor.SetActive(true);
			}


			//// 1���ځ�2����
			//if (BGobject1.activeSelf == true)
			//{
			//	BGobject1.SetActive(false);
			//	BGobject2.SetActive(true);
			//}

			//// 2���ځ�3����
			//else if (BGobject2.activeSelf == true)
			//{
			//	BGobject2.SetActive(false);
			//	BGobject3.SetActive(true);
			//}

			//// 3���ځ�n����
			//else if (BGobject3.activeSelf == true)
			//{
			//	BGobject3.SetActive(false);
			//}
		}

		//if (cursor.activeSelf == false && bEndTutorial == true)
		//{
		//	cursor.SetActive(true);
		//}
	}
}
