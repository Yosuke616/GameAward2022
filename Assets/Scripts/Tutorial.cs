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

	public int nCnt;
	public bool bStartTutorial;
	public bool bEndTutorial;

	// Start is called before the first frame update
	void Start()
	{
		// �ϐ���`
		TutorialPanel = GameObject.Find("TutorialPanel");
		cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
		BGobjects.Add(TutorialPanel.transform.Find("BackGround0").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround1").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround2").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround3").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround4").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround6").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround7").gameObject);

		var color = new Color(0.0f, 0.0f, 0.0f, 128.0f / 255.0f);
		for (int i = 0; i < BGobjects.Count; i++)
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
		if (Input.GetMouseButtonDown(0) && bStartTutorial == true && bEndTutorial == false)
		{
			// ���݂̃y�[�W���\��������
			BGobjects[nCnt].SetActive(false);


			// ���݂̗v�f�������X�g�̗v�f����菬�����Ƃ��i�������j�̏���
			if (nCnt + 1 < BGobjects.Count)
			{
				BGobjects[nCnt + 1].SetActive(true);
				nCnt++;
			}

			// �������ł͂Ȃ����A�ؒf�����L���ɂ���
			else
			{
				Debug.LogWarning($"Tuto:end[{bEndTutorial}]");
				bEndTutorial = true;
				cursor.SetActive(true);
			}

		}
	}
}
