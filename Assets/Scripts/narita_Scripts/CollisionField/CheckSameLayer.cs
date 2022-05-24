using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSameLayer : MonoBehaviour
{
    [SerializeField] private int m_nOriginalLayer = 1;
    [SerializeField] private int m_nCurrentLayer = 0;
    MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // ���݂̃��C���[���X�V
        m_nCurrentLayer = CurrentLayer(transform.position);

        if(m_nOriginalLayer != m_nCurrentLayer)
        {
            Debug.Log("�R���C�_�[�Ȃ�");
            // ���C���[���Ⴄ�ꍇ�Afalse
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
        else
        {
            Debug.Log("�R���C�_�[");

            // ���ꃌ�C���̏ꍇ
            meshRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }


   // �����蔻��I�u�W�F�N�g�̎��̒��ɂ���̂��A�j��ꂽ�ꏊ�ɂ���̂�
    public int CurrentLayer(Vector3 pos)
    {
        // �J������������
        GameObject cameraObj = GameObject.Find("SubCamera0");
        pos = cameraObj.transform.InverseTransformPoint(pos);

        // �}�X�̉��E�c�̐�
        float gridSizeX = CreateGridScript.gridSizeX;
        float gridSizeY = CreateGridScript.gridSizeY;
        int gridNumX = CreateGridScript.horizon;
        int gridNumY = CreateGridScript.virtical;

        // ray���΂����W
        Vector2 start;
        start.x = -gridSizeX * gridNumX / 2.0f + (gridSizeX * 0.5f);
        start.y =  gridSizeY * gridNumY / 2.0f - (gridSizeY * 0.5f);
        // �O���b�h�̉����ƍ���
        Vector2 length;
        length.x = gridSizeX * gridNumX;
        length.y = gridSizeY * gridNumY;
        // �L���������߂�
        Vector2 distance;
        distance.x = pos.x - start.x;
        distance.y = start.y - pos.y;
        // ��������䗦�����߂�
        Vector2 rate;
        rate.x = distance.x / length.x;
        rate.y = distance.y / length.y;
        // �䗦���猻�݂̃}�X�̔ԍ������߂�
        int massX, massY;
        massX = (int)(gridNumX * rate.x);
        massY = (int)(gridNumY * rate.y);
        //Debug.LogWarning("x:" + massX + "   y:" + massY);
        massY++;
        if (massX < 0 || massY < 0) return -1;

        // paper�ŁA�w�肳�ꂽ�ԍ��̃O���b�h�Ƀ��C���΂��Ă����̎��̃��C���[�ԍ����擾����


        // ���̃O���b�h�̑傫��
        float paperGridSizeX = CreateGridScript.paperGridSizeX;
        float paperGridSizeY = CreateGridScript.paperGridSizeY;

        //��ׂ��q�G�����L�[�����������o���Ă���
        GameObject mainCamera = GameObject.Find("MainCamera");

        // �`��J�n�ʒu = �J�������W - grid�̉��� * ���̔���
        Vector3 StartPoint;
        StartPoint.x = mainCamera.transform.position.x - paperGridSizeX * gridNumX * 0.5f + (paperGridSizeX * 0.5f);
        StartPoint.y = mainCamera.transform.position.y + paperGridSizeY * gridNumY * 0.5f - (paperGridSizeY * 0.5f);

        float x, y;
        x = StartPoint.x + (paperGridSizeX * massX);
        y = StartPoint.y - (paperGridSizeY * massY);
        // �}�X���Ƃɂ����蔻������p�̃I�u�W�F�N�g�𐶐�
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, new Vector3(x, y, 10.2f), out hit))
        {
            if(hit.collider.gameObject.tag == "paper")
            {
                //Debug.DrawRay(mainCamera.transform.position, new Vector3(x, y, 10.2f));
                //Debug.LogError("");
                //Debug.LogWarning("yes" + hit.collider.gameObject.GetComponent<DivideTriangle>().GetNumber());
                return hit.collider.gameObject.GetComponent<DivideTriangle>().GetNumber();
            }
        }

        return 0;
    }

    public void SetLayer(int layer)
    {
        m_nOriginalLayer = layer;
    }
}
