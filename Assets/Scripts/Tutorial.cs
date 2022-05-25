// Tutorialシーン専用スクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	public GameObject StartTutorial;					// チュートリアルスタート用
	public bool bStop;

	[SerializeField] private GameObject cursor;			//
	[SerializeField] private GameObject turnPaper;		//
	[SerializeField] private GameObject Yousei1;		// １枚目妖精
	[SerializeField] private GameObject Yousei2;        // ２枚目妖精

	[SerializeField] private List<GameObject> BGobjects = new List<GameObject>();

	[SerializeField] private GameObject txt_ComeOn;
	[SerializeField] private GameObject txt_CutStart;
	[SerializeField] private GameObject txt_SonoChoshi;
	[SerializeField] private GameObject txt_Koko;

	[SerializeField] private CursorSystem CuttingCheck;

	[SerializeField] private int nCnt;
	[SerializeField] private bool bStartZoomout;		// チュートリアル開始フラグ
	[SerializeField] private bool bStartTutorial;       // チュートリアル開始フラグ
	[SerializeField] private bool bCutting;
	[SerializeField] private bool bStartCut;
	[SerializeField] private bool bEndTutorial;         // チュートリアル終了フラグ

	[SerializeField] private float WeitTime = 2.0f;		// 待機時間
	[SerializeField] private float elapsedTime;
	[SerializeField] private GameObject[] papers;

	// Start is called before the first frame update
	void Start()
	{
		// 変数定義
		cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
		turnPaper = GameObject.Find("Folder").gameObject.transform.Find("System").gameObject.transform.Find("Cursor").gameObject;
		Yousei1 = GameObject.Find("Folder").gameObject.transform.Find("SubCamera1").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
		Yousei2 = GameObject.Find("Folder").gameObject.transform.Find("SubCamera2").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;

		BGobjects.Add(transform.Find("BackGround0").gameObject);		// ２枚目誘導
		BGobjects.Add(transform.Find("BackGround1").gameObject);		// １枚目戻り
		BGobjects.Add(transform.Find("BackGround2").gameObject);        // クリック開始説明
		BGobjects.Add(transform.Find("BackGround3").gameObject);        // クリック箇所
		BGobjects.Add(transform.Find("BackGround3").gameObject);        // クリック箇所
		BGobjects.Add(transform.Find("BackGround4").gameObject);		// 右へ進め

		txt_ComeOn = GameObject.Find("txt_ComeOn");
		txt_CutStart = GameObject.Find("txt_CutStart");
		txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
		txt_Koko = GameObject.Find("txt_KokomadeKireteruyo");

		CuttingCheck = turnPaper.GetComponent<CursorSystem>();

		// 非表示化処理
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
		//bCutting = cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg();
		bEndTutorial = false;
		elapsedTime = 0.0f;
		papers = null;
	}

	// Update is called once per frame
	void Update()
	{
		// 初回操作
		if (StartTutorial.GetComponent<ZoomOut>().GetZoomStart() == true)
		{
			bStartZoomout = true;
		}
		else if (bStartZoomout == true && bStartTutorial == false)
		{
			papers = GameObject.FindGameObjectsWithTag("paper");
			BGobjects[0].SetActive(true);
			cursor.SetActive(false);        // 説明中は切断操作を無効にする
			turnPaper.SetActive(false);     // 説明中は紙めくりを無効にする
			bStartTutorial = true;
			Debug.LogWarning("Tuto:初回操作");
			txt_ComeOn.SetActive(true);
		}

		if (bStartTutorial == true && bEndTutorial == false)
		{
			// チュートリアル中は常に実行
			Yousei1.GetComponent<Fiary_Script>().enabled = false;
			Yousei1.GetComponent<Fiary_Move>().enabled = false;
			Yousei2.GetComponent<Fiary_Script>().enabled = false;
			Yousei2.GetComponent<Fiary_Move>().enabled = false;

			// めくり中でないとき
			if (!bStop)
			{
				// 切断中でないとき次のページへ
				if (Input.GetMouseButtonDown(0) && !bCutting)
				{
					// 現在のページを非表示化する
					BGobjects[nCnt].SetActive(false);
					nCnt++;
					Debug.LogWarning($"nCnt:{nCnt}");

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
						// 説明中ではない時、切断操作を有効にする
						case 3:
							BGobjects[nCnt].SetActive(true);
							cursor.SetActive(true);
							turnPaper.SetActive(true);
							Yousei1.GetComponent<Fiary_Script>().enabled = true;
							Yousei1.GetComponent<Fiary_Move>().enabled = true;
							Yousei2.GetComponent<Fiary_Script>().enabled = true;
							Yousei2.GetComponent<Fiary_Move>().enabled = true;
							bCutting = true;
							Debug.LogWarning($"cut:true");
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

				else if (bCutting)
				{
					bool bCut = cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg();
					if (bStartCut && !bCut)
					{
						Debug.LogWarning($"処理終了");
						bCutting = false;
						nCnt++;
						BGobjects[nCnt - 1].SetActive(false);
						BGobjects[nCnt].SetActive(true);
						cursor.SetActive(false);
						turnPaper.SetActive(false);
						Yousei1.GetComponent<Fiary_Script>().enabled = false;
						Yousei1.GetComponent<Fiary_Move>().enabled = false;
						Yousei2.GetComponent<Fiary_Script>().enabled = false;
						Yousei2.GetComponent<Fiary_Move>().enabled = false;
						bStop = true;
					}

					if (Input.GetMouseButtonDown(0) && nCnt == 3 && !bStartCut)
					{
						nCnt++;
						bStartCut = true;
					}
				}
			}

			// めくり中処理
			else
			{
				// 初回処理
				if (elapsedTime == 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
					elapsedTime = Time.time;

				// めくり終了時処理
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
		}
	}
}
