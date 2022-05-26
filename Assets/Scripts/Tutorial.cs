// Tutorialシーン専用スクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	private GameObject cursor;          // 切断処理無効化用
	private GameObject turnPaper;       // 紙めくり処理無効化用
	private GameObject Yousei1;         // １枚目妖精
	private GameObject Yousei2;         // ２枚目妖精
	private InputTrigger Inputer;       // JoyCon LT_Key検知

	private List<GameObject> BGobjects = new List<GameObject>(); // 説明パネルList

	private GameObject txt_ComeOn;		// 妖精吹き出し「こっちにきて！」
	private GameObject txt_CutStart;	// 　　　　　　「切りはじめたよ！」
	private GameObject txt_Koko;		// 　　　　　　「ここまで切れてるよ」
	private GameObject txt_SonoChoshi;  // 　　　　　　「その調子」

	[SerializeField] private int nCnt;  // 説明パネル カウント

	private bool bStop;                 // 紙めくりフラグ
	private bool bStartZoomout;			// ZoomOut開始フラグ
	private bool bStartTutorial;		// チュートリアル開始フラグ
	private bool bStartCut;				// 切断開始フラグ
	private bool bCutting;				// 切断中フラグ
	private bool bEndTutorial;          // チュートリアル終了フラグ

	private bool bTxt;

	private float WeitTime = 2.0f;		// 待機時間定義
	private float elapsedTime;			// 比較用時間


	void Start()
	{
		//--- 変数定義部
		{
			cursor = GameObject.Find("Folder").gameObject.transform.Find("cursor").gameObject;
			turnPaper = GameObject.Find("System").gameObject.transform.Find("Cursor").gameObject;
			Yousei1 = GameObject.Find("SubCamera1").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
			Yousei2 = GameObject.Find("SubCamera2").gameObject.transform.Find("d1").gameObject.transform.Find("Yousei1").gameObject;
			Inputer = GameObject.Find("MainCamera").gameObject.GetComponent<InputTrigger>();

			BGobjects.Add(transform.Find("BackGround0").gameObject);        // ２枚目誘導
			BGobjects.Add(transform.Find("BackGround1").gameObject);        // １枚目戻り
			BGobjects.Add(transform.Find("BackGround2").gameObject);        // クリック開始説明
			BGobjects.Add(transform.Find("BackGround3").gameObject);        // クリック箇所
			BGobjects.Add(transform.Find("BackGround3").gameObject);        // クリック箇所
			BGobjects.Add(transform.Find("BackGround4").gameObject);        // 右へ進め

			txt_ComeOn = GameObject.Find("txt_ComeOn");
			txt_CutStart = GameObject.Find("txt_CutStart");
			txt_SonoChoshi = GameObject.Find("txt_SonoChoshi");
			txt_Koko = GameObject.Find("txt_Koko");
		}

		//--- 初期化処理
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
		//--- 初回操作
		if (!bStartTutorial)
			Initialized();

		// チュートリアル実行時かつ終了していないとき
		if (bStartTutorial && !bEndTutorial)
		{
			// チュートリアル中は常に切断動作を無効化
			Yousei1.GetComponent<Fiary_Script>().enabled = false;
			Yousei1.GetComponent<Fiary_Move>().enabled = false;
			Yousei2.GetComponent<Fiary_Script>().enabled = false;
			Yousei2.GetComponent<Fiary_Move>().enabled = false;

			// めくり中処理
			if (bStop)
				TurnPaper();

			// 切断中の処理
			else if (bCutting)
				Cutting();

			// 説明パネル表示処理（めくり中かつ切断中でないとき）
			else
			{
				// 決定ボタン押下時、次の説明パネルへ
				if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 1"))
				{
					// 現在の説明パネルを非表示化する
					BGobjects[nCnt].SetActive(false);

					// 次の説明パネルへ
					nCnt++;

					// 各説明パネルごとの処理
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

						case 6:		// チュートリアル終了
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

		// ZoomOut終了時実行
		else if (bStartZoomout == true && bStartTutorial == false)
		{
			BGobjects[0].SetActive(true);
			cursor.SetActive(false);        // 説明中は切断操作を無効にする
			turnPaper.SetActive(false);     // 説明中は紙めくりを無効にする
			bStartTutorial = true;
			Debug.LogWarning("Tuto:Initialized");
			txt_ComeOn.SetActive(true);
		}
	}

	private void TurnPaper()
	{
		// 初回処理（elapsedTimeに現在の実行時間を代入）
		if (elapsedTime == 0 &&
			((nCnt == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))) ||
			(nCnt == 2 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))) ||
			nCnt == 5))
			elapsedTime = Time.time;

		// めくり終了時処理（初回処理時からWeitTime分経過したとき）
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
		// 切断処理終了フラグを取得
		bool bCut = cursor.GetComponent<OutSide_Paper_Script_Second>().GetFirstFlg();

		// 切断処理が始まっていない時
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

		// 切断処理終了時
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
