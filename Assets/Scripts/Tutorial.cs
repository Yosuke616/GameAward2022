using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	public Camera openingCamera;
	public bool bStop;

	[SerializeField] private GameObject TutorialPanel;
	[SerializeField] private GameObject cursor;
	[SerializeField] private GameObject turnPaper;
	[SerializeField] private GameObject Yousei1;
	[SerializeField] private GameObject Yousei2;

	[SerializeField] private List<GameObject> BGobjects = new List<GameObject>();

	[SerializeField] private GameObject txt_ComeOn;
	[SerializeField] private GameObject txt_CutStart;
	[SerializeField] private GameObject txt_SonoChoshi;
	[SerializeField] private GameObject txt_Koko;

	[SerializeField] private int nCnt;
	[SerializeField] private bool bStartTutorial;
	[SerializeField] private bool bEndTutorial;

	[SerializeField] private float WeitTime = 2.0f;
	[SerializeField] private float elapsedTime;

	// Start is called before the first frame update
	void Start()
	{
		// �ϐ���`
		TutorialPanel = GameObject.Find("TutorialPanel");
		cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
		turnPaper = GameObject.Find("Folder").gameObject.transform.Find("System").gameObject.transform.Find("Cursor").gameObject;
		Yousei1 = GameObject.Find("Folder").gameObject.
			transform.Find("SubCamera1").gameObject.
			transform.Find("d1").gameObject.
			transform.Find("Yousei1").gameObject;
		Yousei2 = GameObject.Find("Folder").gameObject.
			transform.Find("SubCamera2").gameObject.
			transform.Find("d1").gameObject.
			transform.Find("Yousei1").gameObject;
		BGobjects.Add(TutorialPanel.transform.Find("BackGround0").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround1").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround2").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround3").gameObject);
		//BGobjects.Add(TutorialPanel.transform.Find("BackGround4").gameObject);
		//BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		//BGobjects.Add(TutorialPanel.transform.Find("BackGround6").gameObject);
		//BGobjects.Add(TutorialPanel.transform.Find("BackGround7").gameObject);

		txt_ComeOn = GameObject.Find("txt_ComeOn");
		txt_CutStart = GameObject.Find("txt_CutStart");
		txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
		txt_Koko = GameObject.Find("txt_KokomadeKireteruyo");


		var color = new Color(0.0f, 0.0f, 0.0f, 128.0f / 255.0f);
		// �q�G�����L�[�r���[��ł́A�֋X�㓧���x��0.0f�ɐݒ肵�Ă���̂ŁA���ɖ߂�����
		BGobjects[0].GetComponent<Image>().color = color;

		for (int i = 0; i < BGobjects.Count; i++)
		{
			// ��\��������
			BGobjects[i].SetActive(false);
		}

		txt_ComeOn.SetActive(false);
		txt_CutStart.SetActive(false);
		txt_SonoChoshi.SetActive(false);
		txt_Koko.SetActive(false);

		nCnt = 0;
		bStartTutorial = false;
		bEndTutorial = false;
		elapsedTime = 0.0f;
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
			turnPaper.SetActive(false);     // �������͎��߂���𖳌��ɂ���
			bStartTutorial = true;
			Debug.LogWarning("Tuto:���񑀍�");
			txt_ComeOn.SetActive(true);
		}

		if (bStartTutorial == true && bEndTutorial == false)
		{
			// �`���[�g���A�����͏�Ɏ��s
			Yousei1.GetComponent<Fiary_Script>().enabled = false;
			Yousei1.GetComponent<Fiary_Move>().enabled = false;
			Yousei2.GetComponent<Fiary_Script>().enabled = false;
			Yousei2.GetComponent<Fiary_Move>().enabled = false;
			if (bStop == false)
			{
				// ���̃y�[�W��
				if (Input.GetMouseButtonDown(0))
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
						cursor.SetActive(true);
						turnPaper.SetActive(true);
						Yousei1.GetComponent<Fiary_Script>().enabled = true;
						Yousei1.GetComponent<Fiary_Move>().enabled = true;
						Yousei2.GetComponent<Fiary_Script>().enabled = true;
						Yousei2.GetComponent<Fiary_Move>().enabled = true;
						bEndTutorial = true;
						Debug.LogWarning($"Tuto:end[{bEndTutorial}]");
					}

					switch (nCnt)
					{
						// 0~1
						case 1:
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						// 2~3
						case 3:
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						case 4:
							txt_SonoChoshi.SetActive(true);
							break;

						case 5:
							txt_Koko.SetActive(true);
							break;

						default:
							break;
					}

				}

			}
			else
			{
				if (nCnt == 1)
				{
					Debug.LogWarning($"time[{Time.time}], elTime[{elapsedTime}]");
					if (elapsedTime != 0 && Time.time - elapsedTime >= WeitTime)
					{
						bStop = false;
						turnPaper.SetActive(false);
						BGobjects[nCnt].SetActive(true);
						elapsedTime = 0;
					}

					if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						elapsedTime = Time.time;
					}
				}
				if (nCnt == 3)
				{
					Debug.LogWarning($"time[{Time.time}], elTime[{elapsedTime}]");
					if (elapsedTime != 0 && Time.time - elapsedTime >= WeitTime)
					{
						bStop = false;
						turnPaper.SetActive(false);
						BGobjects[nCnt].SetActive(true);
						elapsedTime = 0;
						txt_CutStart.SetActive(true);
					}

					if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						elapsedTime = Time.time;
					}
				}
			}

		}
	}
}
