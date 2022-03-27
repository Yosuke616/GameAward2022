using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���C�������_���[�Ƀ}�E�X���W�����Ă���
[RequireComponent(typeof(LineRenderer))]
public class LineRendererOperator : MonoBehaviour
{
    // ���C�������_���[
    [SerializeField] private LineRenderer lineRenderer;
    // ���W���X�g�̃T�C�Y
    [SerializeField] private int positionCount;
    [SerializeField] List<Vector3> breakline = new List<Vector3>();
    // �J�����Ƃ̋���
    private float posZ;
    // �ݒ肵���J�����̑O�ɐ��������Ă���
    private Camera mainCamera;

    // ������
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        positionCount = 0;

        // ���C���J����
        mainCamera = Camera.main;

        posZ = 10.0f;
    }

    // �X�V
    void Update()
    {
        //// ���W�ۑ�
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // ���̃��C���I�u�W�F�N�g���A�ʒu�̓J�����O��10m�A��]�̓J�����Ɠ����ɂȂ�悤�L�[�v������
        //    transform.position = mainCamera.transform.position + mainCamera.transform.forward * posZ;
        //    transform.rotation = mainCamera.transform.rotation;
        //
        //    // ���W�w��̐ݒ�����[�J�����W�n�ɂ������߁A�^������W�ɂ����������
        //    Vector3 pos = Input.mousePosition;
        //    pos.z = posZ;
        //    // �}�E�X�X�N���[�����W�����[���h���W�ɒ���
        //    pos = mainCamera.ScreenToWorldPoint(pos);
        //    // ����ɂ�������[�J�����W�ɒ����B
        //    pos = transform.InverseTransformPoint(pos);
        //    // ����ꂽ���[�J�����W�����C�������_���[�ɒǉ�����
        //    positionCount++;
        //    lineRenderer.positionCount = positionCount;
        //    lineRenderer.SetPosition(positionCount - 1, pos);
        //}
    }

    // �J�����Ƃ̋���
    //public void SetZ(float z)
    //{
    //    posZ = z;
    //}

    // ���̃R���|�[�l���g�̋@�\��OFF�ɂ���
    //public void Remove(GameObject obj)
    //{
    //    obj.GetComponent<LineRendererOperator>().enabled = false;
    //}

    // ���W���X�g���Z�b�g����
    public void SetPoints(List<Vector3> breakLines)
    {
        breakline = breakLines;
        // ���W���X�g�̑傫����ݒ�
        lineRenderer.positionCount = breakLines.Count;

        int cnt = 0;
        foreach (var pos in breakLines)
        {
            cnt++;
            // ���W�̐ݒ�
            lineRenderer.SetPosition(cnt - 1, pos);
        }
    }

    // ���W���X�g�̎擾
    public List<Vector3> GetLines()
    {
        return breakline;
    }

    // ���W���X�g�ɍ��W��ǉ�
    //public void AddPoint(Vector3 pos)
    //{
    //    positionCount++;
    //    lineRenderer.positionCount = positionCount;
    //    lineRenderer.SetPosition(positionCount - 1, pos);
    //
    //    //lineRenderer.GetPosition(0);
    //}
}