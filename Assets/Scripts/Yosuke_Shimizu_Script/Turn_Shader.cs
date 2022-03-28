using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Shader : MonoBehaviour { 

    //��Ԃ�ۑ�����
    private int Mat_Sta;

    //�}�e���A���̓���ׂ̕ϐ�
    Material mat;

    //Flip�̐��l�������邽�߂̕ϐ�
    private float Flip_Num;

    // Start is called before the first frame update
    void Start()
    {
        //���̃R���|�[�l���g�������I�t�W�F�N�g�̃}�e���A���̏��𓾂�
        mat = this.GetComponent<MeshRenderer>().material;
        //�X�e�[�^�X���ŏ��͉������Ȃ�
        Mat_Sta = 0;
        //�ŏ��͂P�ł����ƌ�����
        Flip_Num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mat_Sta) {
            case 0:         //�Ȃɂ����Ȃ�
                break;
            case 1:         //�����ĂȂ��Ȃ�
                mat.SetFloat("_Flip",Flip_Num);
                if (Flip_Num >= -1) {
                    Flip_Num -= 0.05f;
                }
                break;
            case 2:         //�����
                mat.SetFloat("_Flip", Flip_Num);
                if (Flip_Num  <= 1)
                {
                    Flip_Num += 0.05f;
                }
                break;
            default:break;
        }
    }

    //�{�^�����������Ƃ��ɂ߂����悤�Ȋ֐������(Setter)
    public void SetPaperSta(int Sta) {
        Mat_Sta = Sta;
    }
}
