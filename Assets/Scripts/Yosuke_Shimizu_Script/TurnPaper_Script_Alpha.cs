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

    float i = 1;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

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

                    //���ɂ߂��邽�߂̃R���|�[�l���g����������
                    PageList[g_nCountPage].AddComponent<Turn_Shader>();

                    //�J�E���g�𑝂₵�Ă���
                    g_nCountPage++;
                }
              
            }

            //�\�[�g���ĕ��ѕς���
            PageList.Sort((a, b) => a.GetComponent<DivideTriangle>().GetNumber() - b.GetComponent<DivideTriangle>().GetNumber());

            g_bFirst_Load = true;
        }

        //�y�[�W�؂�ւ��֐��Ăяo��
        SlidPaper();

        //A�{�^���������琔�l��ς���
        if (Input.GetKeyDown(KeyCode.C)) {
            PageList[0].GetComponent<Turn_Shader>().SetPaperSta(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PageList[0].GetComponent<Turn_Shader>().SetPaperSta(2);
        }
    }




    //�y�[�W�؂�ւ��p�̊֐�
    private void SlidPaper() {

        if (Input.GetKeyDown(KeyCode.UpArrow)){
            //�|�C���^�I�Ȏg����(���X�g�̐擪�̃I�u�W�F�N�g�����������)
            SaveObject = PageList[0];
            //�擪�̃��X�g���폜����
            PageList.RemoveAt(0);

            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(0.0f, 0.0f,(0.05f * (g_nCountPage)));
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
                pos.z -= 0.05f;
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

            //�Ō�ɃV�F�[�_�[�̏������ɖ߂�
            //SaveObject.GetComponent<Renderer>().material.SetFloat("_Flip", 1);

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

        //�G���^�[������������я������ʂ�ɖ߂�
        if (Input.GetKeyDown(KeyCode.Space) && g_nFirstPage != 0)
        {

            //���������点��悤�ȃJ�E���g
            int ShiftCount = 0;

            //���W��ύX�ł���p�̕ϐ���p�ӂ���
            Vector3 pos;

            //��Ԏ�O(����)�̃y�[�W�������̐l�������܂��͈ړ�������
            for (int i = g_nFirstPage; i < g_nCountPage; i++, ShiftCount++)
            {
                //���X�g�̏���ޔ�������
                SaveObject = PageList[i];

                //�����̃��X�g�̏�������
                PageList.RemoveAt(i);

                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = SaveObject.transform.position;

                //�ԍ��ɉ��������̋������ړ�������
                pos.z += (0.05f * g_nFirstPage);

                //�v�Z����������K�p������
                SaveObject.transform.position = pos;


                //���X�g�̕��я����ς��Ȃ���΂Ȃ�Ȃ�
                //���X�g�̐擪�ɒǉ�����
                PageList.Insert(ShiftCount, SaveObject);

            }

            //��Ԏ�O(����)�̃y�[�W�����O�ɂ���l�������ړ�������
            for (int i = (g_nCountPage - g_nFirstPage); i < g_nCountPage; i++, ShiftCount++)
            {
                //���X�g�̈ړ��ׂ̈ɑ������
                SaveObject = PageList[i];

                //���X�g�ɂ��������̂��폜����
                PageList.RemoveAt(i);

                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = SaveObject.transform.position;

                //�ԍ��ɉ��������̋������ړ�������(�����y�[�W�̂������ʒu������)
                pos.z -= (0.05f * (g_nCountPage - g_nFirstPage));

                //�v�Z����������K�p������
                SaveObject.transform.position = pos;

                //���X�g�̕��я����ς��Ȃ���΂Ȃ�Ȃ�
                //���X�g�̍Ō���ɒǉ�����
                PageList.Insert(ShiftCount, SaveObject);
            }

            //�y�[�W�̓��Z�b�g����邽��0�ɂ���
            g_nFirstPage = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            for (int i = 0;i < g_nCountPage ;i++) {
                Debug.Log(i,PageList[i]);
                Debug.Log(99,SaveObject);
            }
        }
    }
}
