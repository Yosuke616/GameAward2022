using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> papers;

    // 現在の見えている紙 1:タイトル   2:設定画面
    [SerializeField] private int currentNumber;
    // 紙の枚数
    private int maxPaper = 2;

    private bool bStart1 = false;
    private bool bStart2 = false;
    private bool bStart3 = false;

    // Start is called before the first frame update
    void Start()
    {
        currentNumber = 1;

        bStart1 = false;
        bStart2 = false;
        bStart3 = false;

        // 紙の束
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
    }

    // Update is called once per frame
    void Update()
    {
        // ※とりあえず↑キー（条件式は後で変更してくだちい）
        if(bStart1)
        {
            // 現在見えている紙の取得
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();
            
            // めくる
            turnShader.SetPaperSta(1);
            // めくった枚数をカウント
            currentNumber++;

            // めくる枚数の上限
            if (currentNumber > maxPaper) currentNumber = maxPaper;

            bStart1 = false;
            //topPaper.SetActive(false);
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<StageSelect>().enabled = true;
        }
        else if (bStart2)
        {
            // 現在見えている紙の取得
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();

            // めくる
            turnShader.SetPaperSta(1);
            // めくった枚数をカウント
            currentNumber++;

            // めくる枚数の上限
            if (currentNumber > maxPaper) currentNumber = maxPaper;
        }
        //else if(bStart3)
        //{
        //    // 現在見えている紙の取得
        //    var topPaper = papers[currentNumber - 2];
        //    var turnShader = topPaper.GetComponent<Turn_Shader>();
        //    // 戻す
        //    turnShader.SetPaperSta(2);

        //    // めくった枚数をカウント
        //    currentNumber--;
        //    // めくる枚数の上限
        //    if (currentNumber < 1) currentNumber = 1;

        //    bStart3 = false;
        //}

        if(currentNumber == 1)
        {
            // タイトルの紙のときにやりたい処理
            var topPaper = papers[currentNumber - 1];
            topPaper.SetActive(false);
        }
        //else if(bStart2 && currentNumber == 2)
        //{
        //    // 設定の紙のときにやりたい処理
        //    var topPaper = papers[currentNumber - 1];
        //    topPaper.SetActive(true);
        //    bStart2 = false;
        //}
    }

    public void StartPaper()
    {
        bStart1 = true;
    }

    public void ContinuePaper()
    {
        bStart1 = true;
    }

    public void OptionPaper()
    {
        bStart2 = true;
    }

    //public void BackPaper()
    //{
    //    bStart3 = true;
    //}
}
