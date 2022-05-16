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
		// 変数定義
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
			// ヒエラルキービュー上では、便宜上透明度を0.0fに設定しているので、元に戻す処理
			BGobjects[i].GetComponent<Image>().color = color;

			// 非表示化処理
			BGobjects[i].SetActive(false);
		}

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
			bStartTutorial = true;
			Debug.LogWarning("Tuto:初回操作");
		}

		// 次のページへ
		if (Input.GetMouseButtonDown(0) && bStartTutorial == true && bEndTutorial == false)
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
				Debug.LogWarning($"Tuto:end[{bEndTutorial}]");
				bEndTutorial = true;
				cursor.SetActive(true);
			}

		}
	}
}
