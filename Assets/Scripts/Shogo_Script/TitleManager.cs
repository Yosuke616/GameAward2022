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

    void Start()
    {
        currentNumber = 1;

        // 紙の束
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
    }

    // Update is called once per frame
    void Update()
    {
        // ※とりあえず↑キー（条件式は後で変更してくだちい）
        if(Input.GetKeyDown(KeyCode.UpArrow))
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
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 現在見えている紙の取得
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();
            // 戻す
            turnShader.SetPaperSta(2);

            // めくった枚数をカウント
            currentNumber--;
            // めくる枚数の上限
            if (currentNumber < 1) currentNumber = 1;
        }

        if(currentNumber == 1)
        {
            // タイトルの紙のときにやりたい処理
            
        }
        else if(currentNumber == 2)
        {
            // 設定の紙のときにやりたい処理
        }
    }
}
