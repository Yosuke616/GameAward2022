/*
 2022/3/27 ShimizuYosuke 
 ��ג������烌�C���o���ď��ԂɃ��X�g�ɓo�^����
 ���㉺�ł߂����悤�ɂ���
 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaper_Script_Alpha : MonoBehaviour
{
    //��ԍŏ��̃y�[�W��ۑ����邽�߂̕ϐ�
    private int g_nFirstPage;

    //���I�u�W�F�N�g�������������J�E���g����ϐ�
    private int g_nCountPage;

    //�������̃I�u�W�F�N�g���i�[���邽�߂̃��X�g
    private List<GameObject> PageList = new List<GameObject>();

    //���X�g�̍ŏ����Ō��ۑ����邽�߂̕ϐ�
    private GameObject SaveObject;

    //��񂾂����������邽�߂̕ϐ�
    private bool g_bFirst_Load;

    //Ray���g�����߂̕ϐ�
    Ray ray;

    //���C���������������甽������悤�ɂ���
    RaycastHit Hit;

    // Start is called before the first frame update
    void Start()
    {
        //�J�E���g��0�ɂ���
        g_nCountPage = 0;
        //���X�g������������
        PageList.Clear();
        //�ŏ��͉����w���Ă��Ȃ�
        SaveObject = null;
        //�ŏ������������ł���悤�ɂ���
        g_bFirst_Load = false;
        //��ԍŏ��̃y�[�W��ۑ�����
        g_nFirstPage = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //�ŏ��ɏ��������Ă��Ƃ͒ʂ�Ȃ�
        if (!g_bFirst_Load) {
            //�J������������
            GameObject Camera = GameObject.Find("MainCamera");
            //�I�_��������
            GameObject EndPos = GameObject.Find("EndPosition");

            //�J��������܂������Ƀ��C���΂�
            ray = new Ray(
                origin: Camera.transform.position,      //�n�_
                direction: EndPos.transform.position    //�I�_
            );

            //��������̂ł�������X�g�Ɋi�[����
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                if (hit.collider.CompareTag("paper")) {
                    //���X�g�ɒǉ������
                    PageList.Add(hit.collider.gameObject);

                    if (PageList.Count == 1) {
                        //��ԍŏ������̏���������
                        g_nFirstPage = 0;
                    }

                    //�J�E���g�𑝂₵�Ă���
                    g_nCountPage++;
                }
              
            }

            g_bFirst_Load = true;
        }

        //�y�[�W�؂�ւ��֐��Ăяo��
        SlidPaper();

    }

    //�y�[�W�؂�ւ��p�̊֐�

    private void SlidPaper() {
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            //�|�C���^�I�Ȏg����(���X�g�̐擪�̃I�u�W�F�N�g�����������)
            SaveObject = PageList[0];
            //�擪�̃��X�g���폜����
            PageList.RemoveAt(0);

            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(0.0f, 0.0f,(0.05f * g_nCountPage));
            //���X�g�̍Ō���ɒǉ�����
            PageList.Add(SaveObject);

            //���W��ύX�ł���p�̕ϐ���p�ӂ���
            Vector3 pos;

            //�S�̂��������O�ɏo��
            for (int i = 0; i < g_nCountPage; i++)
            {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position;
                //�������炷
                pos.z += 0.05f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }

            //�y�[�W�̐擪���ړ������̂Ɠ����������l��ς���
            //��������ƌ��ɍs���������O�ɖ߂��Ă����̂Ń}�C�i�X���Ă���(�擪�̂����Ԍ��ɑ���)
            g_nFirstPage--;
            //0��菬�����Ȃ�����Ō���ɍs�����߂ɐ��l�𑽂�����
            if (g_nFirstPage < 0)
            {
                //�}�C�i�X1����̂�0����n�߂Ă��邽��
                g_nFirstPage = (g_nCountPage - 1);
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //�Ō���̃��X�g�̗v�f���|�C���^�I�ɓ��Ă͂߂�
            SaveObject = PageList[g_nCountPage - 1];
            //�Ō���̃��X�g�̂���폜����
            PageList.RemoveAt(g_nCountPage - 1);

            //���X�g�̐擪�ɒǉ�����
            PageList.Insert(0, SaveObject);

            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

            //�I�u�W�F�N�g�̏ꏊ��ۑ�����ׂ̕ϐ�
            Vector3 pos;

            //�y�[�W�S�̂����ɉ�����
            for (int i = g_nCountPage - 1; i > 0; i--)
            {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position;
                //�������炷
                pos.z -= 1.0f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }

            //�y�[�W�̐擪���ړ������̂Ɠ����������l��ς���
            //���������ƑO�ɍs�����������ɐi��ł����̂Ńv���X���Ă����i�Ō���̂��O�Ɏ����Ă���j
            g_nFirstPage++;
            if (g_nFirstPage > (g_nCountPage - 1))
            {
                g_nFirstPage = 0;
            }
        }
    }
}
