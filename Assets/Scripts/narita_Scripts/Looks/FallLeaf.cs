using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeaf : Looks
{
    // ��ʉ��ɂ�������������C��
    float deletePosY = -22.0f;

    void Start()
    {
        // ����������
        CreateSpace = new Vector3(
            CreateGridScript.gridSizeX * CreateGridScript.horizon,
            CreateGridScript.gridSizeY * CreateGridScript.virtical, 10.0f);

        // 0.5�b���Ƃɐ���
        createFrame = 30;
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
        //foreach (var looksObject in looksObjects)
        //{
        //    // �Ƃ肠��������������
        //    looksObject.transform.position += new Vector3(0.0f, -0.2f, 0.0f); // Random.Range(0.5f,-1.5f)
        //
		//	// ��ʉ��ɂ����������
        //    //if (looksObject.transform.position.y < deletePosY)
        //    //{
        //    //    looksObjects.Remove(looksObject);
        //    //    Destroy(looksObject);
        //    //}
        //}

        for (int i = looksObjects.Count - 1; i >= 0; --i)
        {
            int random = Random.Range(0, 20);
            if(random >= 10)
            {
                looksObjects[i].transform.position += new Vector3(Mathf.Sin(Random.Range(0.0f, 360.0f)) * 0.04f, -0.2f, 0.0f);
            }
            else
            {
                looksObjects[i].transform.position += new Vector3(0.0f, -0.2f, 0.0f); // Random.Range(0.5f,-1.5f)
            }

            //looksObjects[i].transform.position += new Vector3(x, -0.15f, 0.0f); // Random.Range(0.5f,-1.5f)


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

        foreach (var camera  in cameras)
        {
            // �I�u�W�F�N�g����
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // �R���|�[�l���g�̒ǉ�
            leaf.GetComponent<Renderer>().materials = mats;
            // x�����J�����ƍ��킹��
            leaf.transform.position = camera.transform.position;
            // �����蔻�������
            leaf.GetComponent<Collider>().enabled = false;

            leaf.transform.position += new Vector3(
                rundumPosX,  // ��ʓ��Ń����_��
                CreateSpace.y * 0.5f,           // �J���������ʔ����̍����𑫂�
                22.0f + rundumPosZ);   // z����0��

            // ���X�g�ɒǉ�
            AddLooksObject(leaf);
        }

    }
}
