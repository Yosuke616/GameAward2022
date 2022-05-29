using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWater : Looks
{
    // ��ʉ��ɂ�������������C��
    float deletePosY = -22.0f;

    void Start()
    {
        // ����������
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // ����
        createFrame = 20;

        // �����_���̃V�[�h�l������
        Random.InitState(Time.frameCount);
    }

    public override void Createlooks()
    {
        frameCnt++;

        if (frameCnt > createFrame)
        {
            // �I�u�W�F�N�g����
            CreateLeaf();

            // �J�E���g���Z�b�g
            frameCnt = 0;
        }
    }

    // �����̍X�V
    public override void Updatelooks()
    {
        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            // ���H����
            looksObjects[i].transform.position += new Vector3(0.0f, -0.3f, 0.0f); // Random.Range(0.5f,-1.5f)

            // ��ʉ��ɂ����������
            if (looksObjects[i].transform.position.y < deletePosY)
            {
                Destroy(looksObjects[i]);
                looksObjects.RemoveAt(i);
                looksObjects[i].gameObject.SetActive(false);
            }
        }
    }

    private void CreateLeaf()
    {
        // �T�u�J������T���A���̃J�������ɃI�u�W�F�N�g�𐶐�����
        List<GameObject> cameras = new List<GameObject>();
        cameras.AddRange(GameObject.FindGameObjectsWithTag("SubCamera"));

        // for���ŉ񂷑O��Randum���g�p
        float rundumPosX = Random.Range(-CreateSpace.x * 0.5f, CreateSpace.x * 0.5f);
        float rundumPosZ = Random.Range(-6.0f, 6.0f);

        foreach (var camera in cameras)
        {
            // �I�u�W�F�N�g����
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // �R���|�[�l���g�̒ǉ�
            water.GetComponent<Renderer>().materials = mats;
            // x�����J�����ƍ��킹��
            water.transform.position = camera.transform.position;
            // �����蔻�������
            water.GetComponent<Collider>().enabled = false;

            water.transform.position += new Vector3(
                rundumPosX,  // ��ʓ��Ń����_��
                CreateSpace.y * 0.5f,           // �J���������ʔ����̍����𑫂�
                22.0f + rundumPosZ);   // z����0��

            // ���X�g�ɒǉ�
            AddLooksObject(water);
        }

    }
}
