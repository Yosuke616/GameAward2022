/*
 2022/3/1 �u���z�S 
 �����߂��邽�߂̃X�N���v�g
 ���I�z��Ńy�[�W�ԍ��𐧌䂷��悤�ɂ���
 2022/3/5 �u���z�S
 �F�̕ύX�����}�e���A�������O�ɗp�ӂ�����������Ȃ���Ώo���Ȃ�
 ������e�N�X�`���������_���ɐݒ�ł���悤�ɂ�����
 2022/3/6 �u���z�S
 �{�^���ŕ��ёւ���������
 2022/3/8 �u���z�S
 ���ёւ����ł���悤�ɂȂ���
 2022/3/16 �u���z�S
 ��Ԏ�O�ɗ��Ă�����̂ɓ����蔻���t����
 ��肽�����ƁE�E�E�����̃I�u�W�F�N�g��z��ɓ���Ă�������X�g�ɓ���ē����ɂ߂����悤�ɂ���B
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaperScript : MonoBehaviour
{

    //�I�u�W�F�N�g�̖��O���擾����ׂ̃��m
    public GameObject g_Page;

    //���X�g�Ő��䂷�邽�߂ɓ��I�z������
    public List<GameObject> PageList = new List<GameObject>();

    //�y�[�W�������߂邽�߂̕ϐ�(�I�u�W�F�N�g�̐�)
    [SerializeField] private int m_nPageMax = 100;

    //���X�g�̍ŏ����Ō��ۑ����邽�߂̕ϐ�
    private GameObject SaveObject;

    //�z�u����}�e���A���̐ݒ�
    public Material[] ColorSet = new Material[1];

    //�y�[�W�ԍ���ۑ����邽�߂̕ϐ�
    private int m_nPageNum;

   // Start is called before the first frame update
   void Start()
    {
        for (int i = 0; i < m_nPageMax; i++)
        {
            //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
            GameObject Page = GameObject.Instantiate(g_Page) as GameObject;     //����
            Page.transform.position = new Vector3(15.0f, 0.0f, 100.0f - i);       //���W
            Page.transform.localScale = new Vector3(100.0f, 0.5f, 100.0f);        //�傫��
            Page.transform.rotation = Quaternion.Euler(90, 0, 0);               //��]
            
            //���O�̐ݒ�
            string szName = i.ToString();
            Page.name = szName;



            //��ԍŏ��ɒǉ��������̂ɂ͐F��t���ĕ�����₷������
            if (i == 0)
            {
                //�F�̐ݒ�
                Page.GetComponent<MeshRenderer>().material = ColorSet[0];

                //��ԍŏ��̃y�[�W�̔ԍ���ۑ����Ă���
                m_nPageNum = 0;
            }
            else {
                //�����蔻�����Ԏ�O�ȊO��������
                var Coll = Page.GetComponent<BoxCollider>();
                Coll.enabled = false;
            }

            //���X�g�ɒǉ�����
            PageList.Add(Page);
        }

        //���X�g�ɒǉ����ꂽ����\��
        foreach (GameObject i in PageList)
        {
            Debug.Log(i.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //�y�[�W�̐؂�ւ�
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //�����蔻����폜����
            var Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = false;

            //�|�C���^�I�Ȏg����(���X�g�̐擪�̃I�u�W�F�N�g�����������)
            SaveObject = PageList[0];
            //�擪�̃��X�g���폜����
            PageList.RemoveAt(0);

            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f - (1* m_nPageMax));
            //���X�g�̍Ō���ɒǉ�����
            PageList.Add(SaveObject);

            //���W��ύX�ł���p�̕ϐ���p�ӂ���
            Vector3 pos;

            //�S�̂��������O�ɏo��
            for (int i = 0;i < m_nPageMax; i++ ) {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position ;
                //�������炷
                pos.z += 1.0f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }

            //�y�[�W�̐擪���ړ������̂Ɠ����������l��ς���
            //��������ƌ��ɍs���������O�ɖ߂��Ă����̂Ń}�C�i�X���Ă���(�擪�̂����Ԍ��ɑ���)
            m_nPageNum--;
            //0��菬�����Ȃ�����Ō���ɍs�����߂ɐ��l�𑽂�����
            if (m_nPageNum < 0) {
                //�}�C�i�X1����̂�0����n�߂Ă��邽��
                m_nPageNum = (m_nPageMax-1);
            }

            //��Ԏ�O�̃y�[�W�ɓ����蔻����I���ɂ���
            Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = true;

        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //�擪�̓����蔻�������
            var Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = false;

            //�Ō���̃��X�g�̗v�f���|�C���^�I�ɓ��Ă͂߂�
            SaveObject = PageList[m_nPageMax - 1];
            //�Ō���̃��X�g�̂���폜����
            PageList.RemoveAt(m_nPageMax - 1);

            //���X�g�̐擪�ɒǉ�����
            PageList.Insert(0, SaveObject);
            
            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f);


            //�I�u�W�F�N�g�̏ꏊ��ۑ�����ׂ̕ϐ�
            Vector3 pos;

            //�y�[�W�S�̂����ɉ�����
            for (int i = m_nPageMax - 1; i > 0;i--) {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position;
                //�������炷
                pos.z -= 1.0f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }

            //�y�[�W�̐擪���ړ������̂Ɠ����������l��ς���
            //���������ƑO�ɍs�����������ɐi��ł����̂Ńv���X���Ă����i�Ō���̂��O�Ɏ����Ă���j
            m_nPageNum++;
            if (m_nPageNum > (m_nPageMax-1)) {
                m_nPageNum = 0;
            }

            //�擪�̓����蔻����I���ɂ���
            Coll = PageList[0].GetComponent<BoxCollider>();
            Coll.enabled = true;
        }

        //�G���^�[������������я������ʂ�ɖ߂�
        if (Input.GetKeyDown(KeyCode.Return) && m_nPageNum != 0) {

            //���������点��悤�ȃJ�E���g
            int ShiftCount = 0; 

            //���W��ύX�ł���p�̕ϐ���p�ӂ���
            Vector3 pos;

            //��Ԏ�O(����)�̃y�[�W�������̐l�������܂��͈ړ�������
            for (int i = m_nPageNum; i < m_nPageMax; i++, ShiftCount++)
            {
                //���X�g�̏���ޔ�������
                SaveObject = PageList[i];

                //�����̃��X�g�̏�������
                PageList.RemoveAt(i);

                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = SaveObject.transform.position;

                //�ԍ��ɉ��������̋������ړ�������
                pos.z += (1.0f * m_nPageNum);

                //�v�Z����������K�p������
                SaveObject.transform.position = pos;


                //���X�g�̕��я����ς��Ȃ���΂Ȃ�Ȃ�
                //���X�g�̐擪�ɒǉ�����
                PageList.Insert(ShiftCount, SaveObject);

            }

            //��Ԏ�O(����)�̃y�[�W�����O�ɂ���l�������ړ�������
            for (int i = (m_nPageMax-m_nPageNum); i < m_nPageMax; i++,ShiftCount++)
            {
                //���X�g�̈ړ��ׂ̈ɑ������
                SaveObject = PageList[i];

                //���X�g�ɂ��������̂��폜����
                PageList.RemoveAt(i);

                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = SaveObject.transform.position;

                //�ԍ��ɉ��������̋������ړ�������(�����y�[�W�̂������ʒu������)
                pos.z -= (1.0f * (m_nPageMax - m_nPageNum));

                //�v�Z����������K�p������
                SaveObject.transform.position = pos;

                //���X�g�̕��я����ς��Ȃ���΂Ȃ�Ȃ�
                //���X�g�̍Ō���ɒǉ�����
                PageList.Insert(ShiftCount,SaveObject);
            }

            //�y�[�W�̓��Z�b�g����邽��0�ɂ���
            m_nPageNum = 0;
        }



        //�X�y�[�X�{�^���������ƈ�Ԏ�O��������
        if (Input.GetKeyDown(KeyCode.Space)){
            ////��y�[�W�ڂ���j���Ă����܂�
            //Destroy(PageList[0]);
            ////���I�u�W�F�N�g���Ԃ����������X�g����폜����
            //PageList.RemoveAt(0);   

            //���X�g�ɒǉ����ꂽ����\��
            //foreach (GameObject i in PageList)
            //{
            //    Debug.Log(i.name);
            //}

            //�f�o�b�N�p�ԃy�[�W�Ɛ����̏ꏊ�������Ă��邩
            Debug.Log(m_nPageNum);
        }

        //5���ڂ����̓A�N�e�B�u���I�t�ɂ���
        //�܂茩���Ȃ�����
        for (int i = 0;i < m_nPageMax ;i++) {
            if (i > 4)
            {
                PageList[i].SetActive(false);
            }
            else {
                PageList[i].SetActive(true);
            }
        }
    }
    
}
