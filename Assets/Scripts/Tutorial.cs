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

	[SerializeField] private GameObject txt0;

	[SerializeField] private int nCnt;
	[SerializeField] private bool bStartTutorial;
	[SerializeField] private bool bEndTutorial;

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
		BGobjects.Add(TutorialPanel.transform.Find("BackGround1").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround2").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround3").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround4").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround5").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround6").gameObject);
		BGobjects.Add(TutorialPanel.transform.Find("BackGround7").gameObject);

		txt0 = GameObject.Find("txt_tutorial_5");


		var color = new Color(0.0f, 0.0f, 0.0f, 128.0f / 255.0f);
		// ヒエラルキービュー上では、便宜上透明度を0.0fに設定しているので、元に戻す処理
		BGobjects[0].GetComponent<Image>().color = color;

		for (int i = 0; i < BGobjects.Count; i++)
		{
			// 非表示化処理
			BGobjects[i].SetActive(false);
		}

		txt0.SetActive(false);

		nCnt = 0;
		bStartTutorial = false;
		bEndTutorial = false;
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
			txt0.SetActive(true);
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

					// 現在の要素数がリストの要素数より小さいとき（説明中）の処理
					if (nCnt + 1 < BGobjects.Count)
					{
						BGobjects[nCnt + 1].SetActive(true);
						nCnt++;
					}

					// 説明中ではない時、切断操作を有効にする
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
						case 1:
							bStop = true;
							turnPaper.SetActive(false);

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
					if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						bStop = false;
						turnPaper.SetActive(false);
					}
				}
			}

		}
	}
}
