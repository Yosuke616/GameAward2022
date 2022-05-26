// Tutorial�V�[����p�X�N���v�g
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	private GameObject cursor;          // �ؒf�����������p
	private GameObject turnPaper;       // ���߂��菈���������p
	private GameObject Yousei1;         // �P���ڗd��
	private GameObject Yousei2;         // �Q���ڗd��
	private InputTrigger Inputer;       // JoyCon LT_Key���m

	private List<GameObject> BGobjects = new List<GameObject>(); // �����p�l��List

	private GameObject txt_ComeOn;		// �d�������o���u�������ɂ��āI�v
	private GameObject txt_CutStart;	// �@�@�@�@�@�@�u�؂�͂��߂���I�v
	private GameObject txt_Koko;		// �@�@�@�@�@�@�u�����܂Ő؂�Ă��v
	private GameObject txt_SonoChoshi;  // �@�@�@�@�@�@�u���̒��q�v

	[SerializeField] private int nCnt;  // �����p�l�� �J�E���g

	private bool bStop;                 // ���߂���t���O
	private bool bStartZoomout;			// ZoomOut�J�n�t���O
	private bool bStartTutorial;		// �`���[�g���A���J�n�t���O
	private bool bStartCut;				// �ؒf�J�n�t���O
	private bool bCutting;				// �ؒf���t���O
	private bool bEndTutorial;          // �`���[�g���A���I���t���O

	private bool bTxt;

	private float WeitTime = 2.0f;		// �ҋ@���Ԓ�`
	private float elapsedTime;			// ��r�p����


	void Start()
	{
		//--- �ϐ���`��
		{
			cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
			turnPaper = GameObject.Find("System").gameObject.transform.Find("Cursor").gameObject;
			Yousei1 = GameObject.Find("SubCamera1").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
			Yousei2 = GameObject.Find("SubCamera2").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
			Inputer = GameObject.Find("MainCamera").gameObject.GetComponent<InputTrigger>();

			BGobjects.Add(transform.Find("BackGround0").gameObject);        // �Q���ڗU��
			BGobjects.Add(transform.Find("BackGround1").gameObject);        // �P���ږ߂�
			BGobjects.Add(transform.Find("BackGround2").gameObject);        // �N���b�N�J�n����
			BGobjects.Add(transform.Find("BackGround3").gameObject);        // �N���b�N�ӏ�
			BGobjects.Add(transform.Find("BackGround3").gameObject);        // �N���b�N�ӏ�
			BGobjects.Add(transform.Find("BackGround4").gameObject);        // �E�֐i��

			txt_ComeOn = GameObject.Find("txt_ComeOn");
			txt_CutStart = GameObject.Find("txt_CutStart");
			txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
			txt_Koko = GameObject.Find("txt_Koko");
		}

		//--- ����������
		{
			for (int i = 0; i < BGobjects.Count; i++)
				BGobjects[i].SetActive(false);

			txt_ComeOn.SetActive(false);
			txt_CutStart.SetActive(false);
			txt_SonoChoshi.SetActive(false);
			txt_Koko.SetActive(false);

			nCnt = 0;
			bStartTutorial = false;
			bCutting = false;
			bStartCut = false;
			bEndTutorial = false;
			bTxt = false;
			elapsedTime = 0.0f;
		}
	}

	void Update()
	{
		//--- ���񑀍�
		if (!bStartTutorial)
			Initialized();

		// �`���[�g���A�����s�����I�����Ă��Ȃ��Ƃ�
		if (bStartTutorial && !bEndTutorial)
		{
			// �`���[�g���A�����͏�ɐؒf����𖳌���
			Yousei1.GetComponent<Fiary_Script>().enabled = false;
			Yousei1.GetComponent<Fiary_Move>().enabled = false;
			Yousei2.GetComponent<Fiary_Script>().enabled = false;
			Yousei2.GetComponent<Fiary_Move>().enabled = false;

			// �߂��蒆����
			if (bStop)
				TurnPaper();

			// �ؒf���̏���
			else if (bCutting)
				Cutting();

			// �����p�l���\�������i�߂��蒆���ؒf���łȂ��Ƃ��j
			else
			{
				// ����{�^���������A���̐����p�l����
				if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 1"))
				{
					// ���݂̐����p�l�����\��������
					BGobjects[nCnt].SetActive(false);

					// ���̐����p�l����
					nCnt++;

					// �e�����p�l�����Ƃ̏���
					switch (nCnt)
					{
						case 0:
							break;

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

						case 3:
							cursor.SetActive(true);
							turnPaper.SetActive(true);
							Yousei1.GetComponent<Fiary_Script>().enabled = true;
							Yousei1.GetComponent<Fiary_Move>().enabled = true;
							Yousei2.GetComponent<Fiary_Script>().enabled = true;
							Yousei2.GetComponent<Fiary_Move>().enabled = true;

							BGobjects[nCnt].SetActive(true);
							bCutting = true;
							break;

						case 4:
							break;

						case 5:
							break;

						case 6:		// �`���[�g���A���I��
							cursor.SetActive(true);
							turnPaper.SetActive(true);
							Yousei1.GetComponent<Fiary_Script>().enabled = true;
							Yousei1.GetComponent<Fiary_Move>().enabled = true;
							Yousei2.GetComponent<Fiary_Script>().enabled = true;
							Yousei2.GetComponent<Fiary_Move>().enabled = true;
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
		}
	}

	private void Initialized()
	{
		if (GameObject.Find("OpeningCamera").GetComponent<ZoomOut>().GetZoomStart() == true)
			bStartZoomout = true;

		// ZoomOut�I�������s
		else if (bStartZoomout == true && bStartTutorial == false)
		{
			BGobjects[0].SetActive(true);
			cursor.SetActive(false);        // �������͐ؒf����𖳌��ɂ���
			turnPaper.SetActive(false);     // �������͎��߂���𖳌��ɂ���
			bStartTutorial = true;
			Debug.LogWarning("Tuto:Initialized");
			txt_ComeOn.SetActive(true);
		}
	}

	private void TurnPaper()
	{
		// ���񏈗��ielapsedTime�Ɍ��݂̎��s���Ԃ����j
		if (elapsedTime == 0 &&
			((nCnt == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))) ||
			(nCnt == 2 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))) ||
			nCnt == 5))
			elapsedTime = Time.time;

		// �߂���I���������i���񏈗�������WeitTime���o�߂����Ƃ��j
		if (elapsedTime != 0 && Time.time - elapsedTime >= WeitTime)
		{
			bStop = false;
			turnPaper.SetActive(false);
			BGobjects[nCnt].SetActive(true);
			elapsedTime = 0;

			//if (nCnt == 2)
			//	txt_CutStart.SetActive(true);
		}
	}

	private void Cutting()
	{
		// �ؒf�����I���t���O���擾
		bool bCut = cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg();

		// �ؒf�������n�܂��Ă��Ȃ���
		if (Input.GetMouseButtonDown(0) || Inputer.GetOneTimeDown())
		{
			if (nCnt == 3 && !bStartCut)
			{
				txt_CutStart.SetActive(true);
				nCnt++;
				bStartCut = true;
			}
			else
			{
				txt_CutStart.SetActive(false);
				txt_SonoChoshi.SetActive(bTxt);
				txt_Koko.SetActive(!bTxt);
				bTxt = !bTxt;
			}
		}

		// �ؒf�����I����
		else if (bStartCut && !bCut)
		{
			Debug.LogWarning("Tuto:CutEnd");
			bCutting = false;
			nCnt++;
			BGobjects[nCnt - 1].SetActive(false);
			cursor.SetActive(false);
			turnPaper.SetActive(false);
			txt_SonoChoshi.SetActive(false);
			txt_Koko.SetActive(false);
			Yousei1.GetComponent<Fiary_Script>().enabled = false;
			Yousei1.GetComponent<Fiary_Move>().enabled = false;
			Yousei2.GetComponent<Fiary_Script>().enabled = false;
			Yousei2.GetComponent<Fiary_Move>().enabled = false;
			bStop = true;
		}

	}
}
