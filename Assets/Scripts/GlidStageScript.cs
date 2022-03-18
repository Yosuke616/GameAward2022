/*
 2022/3/18 ShimizuYosuke 
 ��ʂ����ς��ɓ����蔻��̂��铧���l�p���I�u�W�F�N�g�𐶐�����(��400�A�c300)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidStageScript : MonoBehaviour
{
    //�I�u�W�F�N�g�̖��O���擾���邽�߂̃��m
    public GameObject g_GlidColl;

    //==============================
    //�F�z�u����}�e���A���̐ݒ�
    public Material[] ColorSet = new Material[2];

    //�F��ς���t���O(true��������ԐF�ɕς���)
    private bool m_nSetColor =  true;
    //==============================

    //�c�̐��Ɖ��̐���ݒ肷��ׂ̕ϐ�
    [SerializeField] private int GlidColl_Num_X = 400;
    [SerializeField] private int GlidColl_Num_Y = 300;

    [SerializeField] private float Position_Z = 100;

    // Start is called before the first frame update
    void Start()
    {
        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject Camera = GameObject.Find("MainCamera");

        //���O��ς��邽�߂ɃJ�E���g�����
        int nNameCnt = 0;

        for (int i = 0;i < GlidColl_Num_Y; i++) {
            for (int u = 0; u < GlidColl_Num_X; u++,nNameCnt++)
            {
                //�X�N���v�g�ŃI�u�W�F�N�g��ǉ�����
                GameObject GlidColl = GameObject.Instantiate(g_GlidColl) as GameObject;         //����
                         
                //�e�Ǝq�̐ݒ������(�����łͶ�ׂ�e�ɂ���)
                GlidColl.transform.SetParent(Camera.transform);

                //�I�u�W�F�N�g�̐ݒ�
                GlidColl.transform.position = new Vector3((Camera.transform.position.x - 211.0f)+(10.0f*u), (Camera.transform.position.y + 116.0f)- (10.0f * i), Position_Z);                      //���W
                GlidColl.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);                    //�傫��
                GlidColl.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);                 //��]

                //���O�̐ݒ�(�Ō��i�͉��s�ڂɂ��邩)
                GlidColl.name = "Glid_No." + nNameCnt + ":" + i;

                //�F�̐ݒ�(�����l�͐�)
                GlidColl.GetComponent<MeshRenderer>().material = ColorSet[1];

                //�}�E�X�̓����蔻��p�̃R���|�[�l���g��ǉ�����
                GlidColl.AddComponent<BoxCollider>();
                var Coll = GlidColl.GetComponent<BoxCollider>();
                Coll.enabled = true;

                //�^�O��t����
                GlidColl.tag = "GlidObject";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�E�N���b�N���ăI�u�W�F�N�g��������悤�ɂ���(��������)
        // �}�E�X���{�^�����N���b�N������
        if (Input.GetMouseButtonDown(0))
        {
            // �X�N���[���ʒu����3D�I�u�W�F�N�g�ɑ΂���Ray�i�����j�𔭎�
            Ray rayOrigine = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // Ray���I�u�W�F�N�g�Ƀq�b�g�����ꍇ
            if (Physics.Raycast(rayOrigine, out RaycastHit hitInfo))
            {
                //GameObject�Ƃ��Ĉ������߂ɕϐ�������đ������
                GameObject HitObject = hitInfo.collider.gameObject;

                //�����Ŋ֐����Ăяo��
                ClickGlid(HitObject);
            }
        }
    }

    //�N���b�N������F�X�ς��֐�(��ɓ����蔻��̗L���ƐF)
    private void ClickGlid(GameObject HitObject) {

        //�j��p�̓����蔻����擾����
        var Coll = HitObject.GetComponent<MeshCollider>();

        //�}�E�X��Ray�����o����p�̓����蔻��
        var MouseColl = HitObject.GetComponent<BoxCollider>();

        //�F��ς���
        if (Coll.enabled == false)
        {
            //�ԐF�ɕς���
            HitObject.GetComponent<MeshRenderer>().material = ColorSet[1];
            //�����蔻����I���ɂ���
            Coll = HitObject.GetComponent<MeshCollider>();
            Coll.enabled = true;
        }
        else if(Coll.enabled ==true){
            //�F�ɕς���
            HitObject.GetComponent<MeshRenderer>().material = ColorSet[0];
            //�����蔻����I�t�ɂ���
            Coll = HitObject.GetComponent<MeshCollider>();
            Coll.enabled = false;

        }

        //�f�o�b�O�p���O�̌��o
        Debug.Log(HitObject.name);
    } 
}
