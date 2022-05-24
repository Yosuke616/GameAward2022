// Tutorial�V�[����p�X�N���v�g
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	public GameObject StartTutorial;					// �`���[�g���A���X�^�[�g�p
	public bool bStop;

	[SerializeField] private GameObject cursor;			//
	[SerializeField] private GameObject turnPaper;		//
	[SerializeField] private GameObject Yousei1;		// �P���ڗd��
	[SerializeField] private GameObject Yousei2;        // �Q���ڗd��

	[SerializeField] private List<GameObject> BGobjects = new List<GameObject>();

	[SerializeField] private GameObject txt_ComeOn;
	[SerializeField] private GameObject txt_CutStart;
	[SerializeField] private GameObject txt_SonoChoshi;
	[SerializeField] private GameObject txt_Koko;

	[SerializeField] private CursorSystem CuttingCheck;

	[SerializeField] private int nCnt;
	[SerializeField] private bool bStartZoomout;		// �`���[�g���A���J�n�t���O
	[SerializeField] private bool bStartTutorial;       // �`���[�g���A���J�n�t���O
	[SerializeField] private bool bCutting;
	[SerializeField] private bool bEndTutorial;         // �`���[�g���A���I���t���O

	[SerializeField] private float WeitTime = 2.0f;		// �ҋ@����
	[SerializeField] private float elapsedTime;

	// Start is called before the first frame update
	void Start()
	{
		// �ϐ���`
		cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
		turnPaper = GameObject.Find("Folder").gameObject.transform.Find("System").gameObject.transform.Find("Cursor").gameObject;
		Yousei1 = GameObject.Find("Folder").gameObject.transform.Find("SubCamera1").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
		Yousei2 = GameObject.Find("Folder").gameObject.transform.Find("SubCamera2").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;

		BGobjects.Add(transform.Find("BackGround0").gameObject);
		//BGobjects.Add(transform.Find("BackGround1").gameObject);
		BGobjects.Add(transform.Find("BackGround1").gameObject);
		BGobjects.Add(transform.Find("BackGround2").gameObject);
		BGobjects.Add(transform.Find("BackGround3").gameObject);
		BGobjects.Add(transform.Find("BackGround4").gameObject);		// �N���b�N�ӏ�
		//BGobjects.Add(transform.Find("BackGround4").gameObject);
		//BGobjects.Add(transform.Find("BackGround4").gameObject);
		BGobjects.Add(transform.Find("BackGround5").gameObject);		// �E�֐i��

		txt_ComeOn = GameObject.Find("txt_ComeOn");
		txt_CutStart = GameObject.Find("txt_CutStart");
		txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
		txt_Koko = GameObject.Find("txt_KokomadeKireteruyo");

		CuttingCheck = turnPaper.GetComponent<CursorSystem>();

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
		bCutting = false;
		bEndTutorial = false;
		elapsedTime = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		// ���񑀍�
		if (StartTutorial.GetComponent<ZoomOut>().GetZoomStart() == true)
		{
			bStartZoomout = true;
		}
		else if (bStartZoomout == true && bStartTutorial == false)
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

			// �߂��蒆�łȂ��Ƃ�
			if (!bStop)
			{
				// �ؒf���łȂ��Ƃ����̃y�[�W��
				if (Input.GetMouseButtonDown(0) && !bCutting)
				{
					// ���݂̃y�[�W���\��������
					BGobjects[nCnt].SetActive(false);
					nCnt++;
					Debug.LogWarning($"{nCnt}");

					// �������ł͂Ȃ����A�ؒf�����L���ɂ���
					if (nCnt >= 3)
					{
						cursor.SetActive(true);
						turnPaper.SetActive(true);
						Yousei1.GetComponent<Fiary_Script>().enabled = true;
						Yousei1.GetComponent<Fiary_Move>().enabled = true;
						Yousei2.GetComponent<Fiary_Script>().enabled = true;
						Yousei2.GetComponent<Fiary_Move>().enabled = true;
						bCutting = true;
						Debug.LogWarning($"cut:true");
					}

					switch (nCnt)
					{
						case 0:
							break;

						// 0~1
						case 1:
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						case 2:
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						// 2~3
						case 3:
							break;

						case 4:
							txt_CutStart.SetActive(false);
							break;

						case 5:
							txt_SonoChoshi.SetActive(true);
							break;

						default:
							//switch (nCnt % 2)
							//{
							//	case 0:
							//		txt_SonoChoshi.SetActive(true);
							//		txt_Koko.SetActive(false);
							//		break;
							//	case 1:
							//		txt_SonoChoshi.SetActive(false);
							//		txt_Koko.SetActive(true);
							//		break;
							//}
							break;
					}
				}
			}

			// �߂��蒆����
			else
			{
				// ���񏈗�
				if (elapsedTime == 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
					elapsedTime = Time.time;

				// �߂���I��������
				if (elapsedTime != 0 && Time.time - elapsedTime >= WeitTime)
				{
					bStop = false;
					turnPaper.SetActive(false);
					BGobjects[nCnt].SetActive(true);
					elapsedTime = 0;
					if (nCnt == 2)
						txt_CutStart.SetActive(true);
				}
			}

			if (bCutting)
			{
				if(Dividing())
				{

				}
			}
		}
	}
}
