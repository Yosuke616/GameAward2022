using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakingMove : FairyState
{
    // �e�I�u�W�F�N�g
    GameObject fairy;
    // 
    public static BreakingMove Instantiate(GameObject game)
    {
        BreakingMove bm = new BreakingMove();
        // �d�������ݒ肵�Ă���
        bm.fairy = game;

        return bm;
    }


    // �d�����ړ�������W���X�g
    static private List<Vector3> MovePoints;
    Vector3 m_vVel;
    Vector3 m_vAcc;
    const float FAIRY_ACC_VALUE = 0.002f;
    const float MAX_VELOCITY = 0.05f;

    // �U���t���O
    private bool bFlg = false;



    // ������
    private void Start()
    {
        MovePoints = new List<Vector3>();
        m_vAcc = m_vVel = Vector3.zero;
    }

    // �X�V
    public override void UpdateFairy()
    {
        if(MovePoints.Count <= 1) { Debug.LogWarning("�d�����ړ�������W���X�g�̃T�C�Y��1�ȉ��ł�"); return; }

        // --- ���W���X�g�̒ʂ�Ɉړ�����
        // �l�����F���W���X�g�̗v�f0����v�f1�Ɉړ�����
        //       �v�f1�̍��W�ɓ���������v�f0�������B
        //       ���X�g�̗v�f��1�ɂȂ�܂ŌJ��Ԃ��B

        //--- �ړ�������������߂�
        Vector3 direction = MovePoints[1] - MovePoints[0];
        //--- �ړ����x�����߂�
        m_vAcc = direction.normalized * FAIRY_ACC_VALUE;
        m_vVel += m_vAcc;

        if (m_vVel.magnitude > 0.09f) m_vVel -= m_vAcc;

        // ���ݍ��W-�ړI���W�A�ŏ��̈ʒu-�ړI���W �̔䗦��7���𒴂�����
        // ���x�����X�ɗ��Ƃ��悤�ɂ���
        float t, t1, t2;
        t1 = Vector3.Distance(fairy.transform.position, MovePoints[1]);
        t2 = Vector3.Distance(MovePoints[0], MovePoints[1]);
        float rate = t1 / t2;
        if (rate < 0.3f)
        {
            m_vVel *= 0.95f;
        }

        //���W���X�V
        fairy.transform.position += m_vVel;


        //--- �ړI�ʒu�ɓ���������o���n�_�ł���v�f0�̃x�N�g�������X�g����폜����
        //    �����݂̗v�f1���o���n�_�ƂȂ�
        Vector3 distance = MovePoints[1] - fairy.transform.position;
        Vector3 nDistance = distance.normalized;
        Vector3 nDirection = direction.normalized;
        float ab = Vector3.Distance(nDistance, nDirection);

        var game = Gamepad.current;
        // �U��
        if (!bFlg)
        {
            if (game != null)
            {
                var gamepad = Gamepad.current;
                //StartCoroutine("Vibration2");
                gamepad.SetMotorSpeeds(0.1f, 0.1f);
            }
            bFlg = true;
        }

        if (ab > 0.0001f)
        {

            MovePoints.RemoveAt(0);

            // ���̂܂܂��ƌ덷���L����̂ŏC�����Ă���
            fairy.transform.position = MovePoints[0];
            fairy.transform.position += new Vector3(0, 0, 0.0f);
            m_vVel = m_vAcc = Vector3.zero;
        }
       
        // �d�����ړ����I�������
        if (MovePoints.Count <= 1)
        {
            // �U���I��
            if(game != null)
            {
                game.SetMotorSpeeds(0.0f, 0.0f);
            }
            // �U���t���OON
            bFlg = false;

            //--- �j�鏈��
            DivideTriangle.Breaking();

            //--- �v���C���[�Ǐ]�ɑJ��
            var fs = fairy.GetComponent<Fiary_Script>();
            fs.SetState(Fiary_Script.eFairyState.STATE_FOLLOWING_PLAYER);
            fs.SmallStart();

            //--- �T�u�J�����̗d����傫������
            List<GameObject> fairys = new List<GameObject>();
            fairys.AddRange(GameObject.FindGameObjectsWithTag("Fiary"));
            foreach (var fairy in fairys)
            {
                // ---�����ŃX�P�[��������������t���O��ON�ɂ���
                fairy.GetComponent<Fiary_Move>().BigStart();
            }

            //--- �d�����ړ�������W���X�g����ɂ���
            MovePoints.Clear();
        }
    }

    // �d�����ړ�������W���X�g���Z�b�g
    static public void SetFairyMovePoints(List<Vector3> points)
    {
        List<Vector3> newList = new List<Vector3>();
        newList.AddRange(points);
        MovePoints = newList;
    }
}
