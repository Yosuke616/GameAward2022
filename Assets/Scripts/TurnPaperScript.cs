/*
 2022/3/1 �u���z�S 
 �����߂��邽�߂̃X�N���v�g
 ���I�z��Ńy�[�W�ԍ��𐧌䂷��悤�ɂ���
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPaperScript : MonoBehaviour
{
    //�y�[�W�̈ړ�����X�s�[�h
    [SerializeField] private float TurnSpeed = 3.0f;

    //�I�u�W�F�N�g�̖��O���擾����ׂ̃��m
    public GameObject g_Page;

    //���X�g�Ő��䂷�邽�߂ɓ��I�z������
    public List<GameObject> PageList = new List<GameObject>();

    //�y�[�W�������߂邽�߂̕ϐ�(�I�u�W�F�N�g�̐�)
    private int m_nPageNum = 100;

    //���X�g�̍ŏ����Ō��ۑ����邽�߂̕ϐ�
    private GameObject SaveObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0;i < m_nPageNum ;i++) {

            //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
            GameObject Page = GameObject.Instantiate(g_Page) as GameObject;   //����
            Page.transform.position = new Vector3(15.0f,0.0f,100.0f - i);       //���W
            Page.transform.localScale = new Vector3(100.0f,0.5f,100.0f);        //�傫��
            Page.transform.rotation = Quaternion.Euler(90, 0, 0);               //��]

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
            //�|�C���^�I�Ȏg����(���X�g�̐擪�̃I�u�W�F�N�g�����������)
            SaveObject = PageList[0];
            //�擪�̃��X�g���폜����
            PageList.RemoveAt(0);

            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f - (1* m_nPageNum));
            //���X�g�̍Ō���ɒǉ�����
            PageList.Add(SaveObject);

            //���W��ύX�ł���p�̕ϐ���p�ӂ���
            Vector3 pos;

            //�S�̂��������O�ɏo��
            for (int i = 0;i < m_nPageNum;i++ ) {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position ;
                //�������炷
                pos.z += 1.0f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }

        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //�Ō���̃��X�g�̗v�f���|�C���^�I�ɓ��Ă͂߂�
            SaveObject = PageList[m_nPageNum - 1];
            //�Ō���̃��X�g�̂���폜����
            PageList.RemoveAt(m_nPageNum -1);

            //���X�g�̐擪�ɒǉ�����
            PageList.Insert(0, SaveObject);
            
            //�ꏊ��ς���
            SaveObject.transform.position = new Vector3(15.0f, 0.0f, 100.0f);


            //�I�u�W�F�N�g�̏ꏊ��ۑ�����ׂ̕ϐ�
            Vector3 pos;

            //�y�[�W�S�̂����ɉ�����
            for (int i = m_nPageNum-1; i > 0;i--) {
                //�ϐ��ɂ��ꂼ��̍��W��������
                pos = PageList[i].transform.position;
                //�������炷
                pos.z -= 1.0f;
                //���炵�����ʂ�������
                PageList[i].transform.position = pos;
            }
        }

        //�X�y�[�X�{�^���������ƈ�Ԏ�O��������
        if (Input.GetKeyDown(KeyCode.Space)){            
                //��y�[�W�ڂ���j���Ă����܂�
                Destroy(PageList[0]);
                PageList.RemoveAt(0);   
        }
    }
}
