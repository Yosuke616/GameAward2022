using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAlpha : Looks
{
    // ��ʉ��ɂ�������������C��
    //float deletePosY = -22.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // ����
        //createFrame = 20;

        // �����_���̃V�[�h�l������
        Random.InitState(Time.frameCount);

        for (int i = 0; i < 20; ++i)
            CreateKira();

        // �����_���̃V�[�h�l������
        //Random.InitState(Time.frameCount);
    }

    public override void Createlooks()
    {
        frameCnt++;

        //if (frameCnt > createFrame)
        //{
        //    // �I�u�W�F�N�g����
        //    CreateKira();
        //
        //    // �J�E���g���Z�b�g
        //    frameCnt = 0;
        //}
    }

    // �����̍X�V
    public override void Updatelooks()
    {
        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            looksObjects[i].GetComponent<DeleteTime>().UpdateKira();
        }
    }

    private void CreateKira()
    {
        // �T�u�J������T���A���̃J�������ɃI�u�W�F�N�g�𐶐�����
        List<GameObject> cameras = new List<GameObject>();
        cameras.AddRange(GameObject.FindGameObjectsWithTag("SubCamera"));

        // for���ŉ񂷑O��Randum���g�p
        float rundumPosX = Random.Range(-CreateSpace.x * 0.5f, CreateSpace.x * 0.5f);
        float rundumPosY = Random.Range(-CreateSpace.y * 0.5f, CreateSpace.y * 0.5f);
        float rundumPosZ = Random.Range(-6.0f, 6.0f);
        int nTime = Random.Range(30, 90);
        int rundum = Random.Range(0, 1);
        float add = Random.Range(0.0f, 0.08f);

        foreach (var camera in cameras)
        {
            // �I�u�W�F�N�g����
            GameObject kira = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // �R���|�[�l���g�̒ǉ�
            kira.GetComponent<Renderer>().materials = mats;
            // x�����J�����ƍ��킹��
            kira.transform.position = camera.transform.position;
            // �����蔻�������
            kira.GetComponent<Collider>().enabled = false;

            // ���Ԑݒ�
            kira.AddComponent<DeleteTime>().SetTime(nTime);
            // ���l�ݒ�
            if (rundum == 0)
            {
                kira.GetComponent<DeleteTime>().SetAlpha(1.0f, false);
            }
            else
            {
                kira.GetComponent<DeleteTime>().SetAlpha(0.0f, true);
            }

            // ���Z�ݒ�
            kira.GetComponent<DeleteTime>().SetAdd(add);

            // ���W�ݒ�
            kira.transform.position += new Vector3(
                rundumPosX,             // ��ʓ��Ń����_��
                rundumPosY,             // �J���������ʔ����̍����𑫂�
                22.0f + rundumPosZ);    // z����0��

            // �傫���ݒ�
            kira.transform.localScale = new Vector3(
                3.0f,
                3.0f,
                3.0f
                );

            // ���X�g�ɒǉ�
            AddLooksObject(kira);
        }

    }
}
