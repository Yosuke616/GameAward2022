using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> papers;

    // ���݂̌����Ă��鎆 1:�^�C�g��   2:�ݒ���
    [SerializeField] private int currentNumber;
    // ���̖���
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

        // ���̑�
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ƃ肠�������L�[�i�������͌�ŕύX���Ă��������j
        if(bStart1)
        {
            // ���݌����Ă��鎆�̎擾
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();
            
            // �߂���
            turnShader.SetPaperSta(1);
            // �߂������������J�E���g
            currentNumber++;

            // �߂��閇���̏��
            if (currentNumber > maxPaper) currentNumber = maxPaper;

            bStart1 = false;
            //topPaper.SetActive(false);
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<StageSelect>().enabled = true;
        }
        else if (bStart2)
        {
            // ���݌����Ă��鎆�̎擾
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();

            // �߂���
            turnShader.SetPaperSta(1);
            // �߂������������J�E���g
            currentNumber++;

            // �߂��閇���̏��
            if (currentNumber > maxPaper) currentNumber = maxPaper;
        }
        //else if(bStart3)
        //{
        //    // ���݌����Ă��鎆�̎擾
        //    var topPaper = papers[currentNumber - 2];
        //    var turnShader = topPaper.GetComponent<Turn_Shader>();
        //    // �߂�
        //    turnShader.SetPaperSta(2);

        //    // �߂������������J�E���g
        //    currentNumber--;
        //    // �߂��閇���̏��
        //    if (currentNumber < 1) currentNumber = 1;

        //    bStart3 = false;
        //}

        if(currentNumber == 1)
        {
            // �^�C�g���̎��̂Ƃ��ɂ�肽������
            var topPaper = papers[currentNumber - 1];
            topPaper.SetActive(false);
        }
        //else if(bStart2 && currentNumber == 2)
        //{
        //    // �ݒ�̎��̂Ƃ��ɂ�肽������
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
