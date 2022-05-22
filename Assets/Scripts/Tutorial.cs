// Tutorialシーン専用スクリプト
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

	[SerializeField] private CursorSystem CuttingCheck;

	[SerializeField] private int nCnt;
	[SerializeField] private bool bStartTutorial;
	[SerializeField] private bool bEndTutorial;

	[SerializeField] private float WeitTime = 2.0f;
	[SerializeField] private float elapsedTime;

	// Start is called before the first frame update
	void Start()
	{
		// 変数定義
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
		//BGobjects.Add(TutorialPanel.transform.Find("BackGround1").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround2").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround3").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround4").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround8").gameObject);

		txt_ComeOn = GameObject.Find("txt_ComeOn");
		txt_CutStart = GameObject.Find("txt_CutStart");
		txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
		txt_Koko = GameObject.Find("txt_KokomadeKireteruyo");

		CuttingCheck = turnPaper.GetComponent<CursorSystem>();

		//var color = new Color(0.0f, 0.0f, 0.0f, 128.0f / 255.0f);
		//// ヒエラルキービュー上では、便宜上透明度を0.0fに設定しているので、元に戻す処理
		//BGobjects[0].GetComponent<Image>().color = color;

		for (int i = 0; i < BGobjects.Count; i++)
		{
			// 非表示化処理
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
		// 初回操作
		if (openingCamera.enabled == false &&
			bStartTutorial == false)
		{
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
			if (bStop == false)
			{
				// 次のページへ
				if (Input.GetMouseButtonDown(0))
				{
					// 現在のページを非表示化する
					BGobjects[nCnt].SetActive(false);

					// 切断中のとき
					//var b = CuttingCheck.CheckNextPaperDividing(0);
					//if (b)
					//{
					//	Debug.LogWarning($"b:{b}");
					//	BGobjects[nCnt + 1].SetActive(true);
					//}

					// 説明中ではない時、切断操作を有効にする
					//else
					if (nCnt + 1 >= 3 && nCnt + 1 != 7)
					{
						cursor.SetActive(true);
						turnPaper.SetActive(true);
						Yousei1.GetComponent<Fiary_Script>().enabled = true;
						Yousei1.GetComponent<Fiary_Move>().enabled = true;
						Yousei2.GetComponent<Fiary_Script>().enabled = true;
						Yousei2.GetComponent<Fiary_Move>().enabled = true;
					}
					nCnt++;

					switch (nCnt)
					{
						case 0:
							break;

						// 0~1
						case 1:
							Debug.LogWarning($"{nCnt}");
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						case 2:
							Debug.LogWarning($"{nCnt}");
							bStop = true;
							turnPaper.SetActive(true);
							BGobjects[nCnt].SetActive(false);
							break;

						// 2~3
						case 3:
							break;

						case 4:
							Debug.LogWarning($"{nCnt}");
							txt_CutStart.SetActive(false);
							break;

						case 5:
							txt_SonoChoshi.SetActive(true);
							break;

						default:
							switch (nCnt % 2)
							{
								case 0:
									Debug.LogWarning($"{nCnt}");
									txt_SonoChoshi.SetActive(true);
									txt_Koko.SetActive(false);
									break;
								case 1:
									Debug.LogWarning($"{nCnt}");
									txt_SonoChoshi.SetActive(false);
									txt_Koko.SetActive(true);
									break;
							}
							break;
					}

				}

			}
			else
			{
				if (nCnt == 1)
				{
					//Debug.LogWarning($"time[{Time.time}], elTime[{elapsedTime}]");
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
				if (nCnt == 2)
				{
					//Debug.LogWarning($"time[{Time.time}], elTime[{elapsedTime}]");
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

		//if (bStartTutorial == true && bEndTutorial == true)
		//{
		//	// 次のページへ
		//	if (Input.GetMouseButtonDown(0))
		//	{
		//		txt_CutStart.SetActive(false);
		//		nCnt++;
		//		switch (nCnt % 2)
		//		{
		//			case 0:
		//				Debug.LogWarning($"{nCnt}");
		//				txt_SonoChoshi.SetActive(true);
		//				txt_Koko.SetActive(false);
		//				break;
		//			case 1:
		//				Debug.LogWarning($"{nCnt}");
		//				txt_SonoChoshi.SetActive(false);
		//				txt_Koko.SetActive(true);
		//				break;
		//		}
		//	}
		//}
	}
}
