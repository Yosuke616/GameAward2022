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

    void Start()
    {
        currentNumber = 1;

        // ���̑�
        papers = new List<GameObject>();
        papers.AddRange(GameObject.FindGameObjectsWithTag("paper"));
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ƃ肠�������L�[�i�������͌�ŕύX���Ă��������j
        if(Input.GetKeyDown(KeyCode.UpArrow))
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
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // ���݌����Ă��鎆�̎擾
            var topPaper = papers[currentNumber - 1];
            var turnShader = topPaper.GetComponent<Turn_Shader>();
            // �߂�
            turnShader.SetPaperSta(2);

            // �߂������������J�E���g
            currentNumber--;
            // �߂��閇���̏��
            if (currentNumber < 1) currentNumber = 1;
        }

        if(currentNumber == 1)
        {
            // �^�C�g���̎��̂Ƃ��ɂ�肽������
            
        }
        else if(currentNumber == 2)
        {
            // �ݒ�̎��̂Ƃ��ɂ�肽������
        }
    }
}
